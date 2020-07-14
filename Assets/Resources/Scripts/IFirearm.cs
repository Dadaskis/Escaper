using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Events {
	class IFirearmInSight {}
}

public abstract class IFirearm : IWeapon {
	public int currentDurability = 600;
	public int maxDurability = 600;
	public int fireRate = 100;

	public string idleOverrideNameFP;
	public AnimationClip idleClipFP;
	public string fireOverrideNameFP;
	public AnimationClip fireClipFP;
	public string drawOverrideNameFP;
	public AnimationClip drawClipFP;
	public string runOverrideNameFP;
	public AnimationClip runClipFP;
	public string reloadOverrideNameFP;
	public AnimationClip reloadClipFP;
	public string fullReloadOverrideNameFP;
	public AnimationClip fullReloadClipFP;
	public string jammingOverrideNameFP;
	public AnimationClip jammingClipFP;
	public string jammedIdleOverrideNameFP;
	public AnimationClip jammedIdleClipFP;
	public string unjammingOverrideNameFP;
	public AnimationClip unjammingClipFP;
	public string punchOverrideNameFP;
	public AnimationClip punchClipFP;
	public float punchHitSeconds;

	/*public AnimationClip idleClipTP;
	public AnimationClip fireClipTP;
	public float fireTPDuration;
	public AnimationClip drawClipTP;
	public float drawTPDuration;
	public AnimationClip runClipTP;
	public float runTPDuration;
	public AnimationClip reloadClipTP;
	public float reloadTPDuration;
	public AnimationClip fullReloadClipTP;
	public float fullReloadTPDuration;*/

	public Transform offsetTransform;
	public Vector3 offsetInitialPosition;
	public bool inSight = false;
	public float goingInSightSpeed = 5.0f;
	public float goingOutSightSpeed = 7.0f;
	public bool jammable = true;
	public bool jammed = false;
	public int minJammingChance = 1;
	public int maxJammingChance = 10;

	public class AfterShootEvent : UnityEvent {}
	public AfterShootEvent afterShootEvent = new AfterShootEvent ();

	public float nextShootTime = 0.0f;

	public override void ApplyAnimationOverrides (ref AnimationClipOverrides overrides)
	{
		if (firstPerson) {
			overrides [idleOverrideNameFP] = idleClipFP;
			overrides [fireOverrideNameFP] = fireClipFP;
			overrides [drawOverrideNameFP] = drawClipFP;
			overrides [runOverrideNameFP] = runClipFP;
			overrides [reloadOverrideNameFP] = reloadClipFP;
			overrides [fullReloadOverrideNameFP] = fullReloadClipFP;
			overrides [unjammingOverrideNameFP] = unjammingClipFP;
			overrides [jammingOverrideNameFP] = jammingClipFP;
			overrides [jammedIdleOverrideNameFP] = jammedIdleClipFP;
			overrides [punchOverrideNameFP] = punchClipFP;
		} else {
			/*overrides ["Idle"] = idleClipTP;
			overrides ["Fire"] = fireClipTP;
			overrides ["Draw"] = drawClipTP;
			overrides ["Run"] = runClipTP;
			overrides ["Reload"] = reloadClipTP;
			overrides ["FullReload"] = fullReloadClipTP;*/
		}
	}

	public abstract void Shoot ();
	public abstract void FirearmStart ();
	public abstract void FirearmUpdate ();
	public abstract bool FirearmReloadStart ();
	public abstract void FirearmReloadEnd ();
	public abstract void FirearmReloadAnimationEnd ();

	void NullifyNextShootTime() {
		nextShootTime = 60.0f / fireRate;
	}

	public void NullifyJammedState() {
		jammed = false;
	}

	public override void PrimaryFire () {
		if (currentAmmo == 0) {
			return;
		}

		if (restrictUsing == true) {
			return;
		}

		if (jammed) {
			return;
		}

		if (nextShootTime > (60.0f / fireRate)) {
			nextShootTime = -1000000.0f;
			if (firstPerson) {
				if (jammable) {
					int procent = Mathf.RoundToInt (((float)currentDurability / (float)maxDurability) * 100.0f);
					int jammingChance = Random.Range (minJammingChance, maxJammingChance);
					if (jammingChance > procent) {
						Debug.LogError (procent + " " + jammingChance);
						jammed = true;
					}
				}
				if (jammed) {
					StartCoroutine (PlayAnimation (jammingOverrideNameFP, jammingClipFP.length));
					NullifyNextShootTime ();
				} else {
					StartCoroutine (PlayAnimation (fireOverrideNameFP, fireClipFP.length, NullifyNextShootTime));
				}
			}
			afterShootEvent.Invoke ();
			currentDurability--;
			Shoot ();
		}
	}

	public override void SingleSecondaryFire () {
		if (restrictUsing == true) {
			return;
		}
		inSight = !inSight;
	}

	void Update() {
		if (firstPerson) {
			bool inSightApproved = false;
			if (inSight) {
				EventData data = EventManager.RunEventListeners<Events.IFirearmInSight> (this);
				if (data.Count == 0) {
					inSightApproved = true;
				}
			}
			if (inSightApproved) {
				offsetTransform.localPosition = Vector3.Lerp (offsetTransform.localPosition, Vector3.zero, Time.deltaTime * goingInSightSpeed);
			} else {
				offsetTransform.localPosition = Vector3.Lerp (offsetTransform.localPosition, offsetInitialPosition, Time.deltaTime * goingOutSightSpeed);
			}
		}
		nextShootTime += Time.deltaTime;
		FirearmUpdate ();
	}

	public override void Reload() {
		if (restrictUsing == true) {
			return;
		}

		if (FirearmReloadStart () == false) {
			return;
		}

		if(firstPerson){
			if (jammed) {
				StartCoroutine(PlayAnimation(unjammingOverrideNameFP, unjammingClipFP.length, NullifyJammedState)); 
			} else {
				if (currentAmmo > 0) {
					StartCoroutine (PlayAnimation (reloadOverrideNameFP, reloadClipFP.length, FirearmReloadAnimationEnd));
				} else {
					StartCoroutine (PlayAnimation (fullReloadOverrideNameFP, fullReloadClipFP.length, FirearmReloadAnimationEnd));
				}
			}
		}

		FirearmReloadEnd ();
	}

	public override void CustomStartOnBegin () {
		offsetInitialPosition = offsetTransform.localPosition;

		FirearmStart ();
	}
}
