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
	public AnimationClip jammedIdleClipFP;
	public string fireOverrideNameFP;
	public AnimationClip fireClipFP;
	public string drawOverrideNameFP;
	public AnimationClip drawClipFP;
	public string reloadOverrideNameFP;
	public AnimationClip reloadClipFP;
	public string fullReloadOverrideNameFP;
	public AnimationClip fullReloadClipFP;
	public string jammingOverrideNameFP;
	public AnimationClip jammingClipFP;
	public string unjammingOverrideNameFP;
	public AnimationClip unjammingClipFP;
	public string punchOverrideNameFP;
	public AnimationClip punchNotHitClipFP;
	public AnimationClip punchHitClipFP;
	public string saveOverrideNameFP;
	public bool save = false;
	public AnimationClip saveClipFP;
	public AnimationClip saveJammedClipFP;

	public float fireAnimationSpeed = 1.0f;
	public float drawAnimationSpeed = 1.0f;
	public float reloadAnimationSpeed = 1.0f;
	public float fullReloadAnimationSpeed = 1.0f;
	public float unjammingAnimationSpeed = 1.0f;
	public float punchAnimationSpeed = 1.0f;

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
	public float punchHitDistanceCheck = 1.5f;
	public GameObject bulletDropPrefab;
	public Transform bulletDropLaunch;
	public float bulletDropWaitSecondsAfterShoot = 1.0f;
	public float bulletDropForce = 1.0f;
	public float bulletDropKillTime = 10.0f;
	public GameObject shootFireParticlePrefab;
	public Transform shootFireTransform;
	[HideInInspector] public ParticleSystem shootFireParticles;

	public class AfterShootEvent : UnityEvent {}
	public AfterShootEvent afterShootEvent = new AfterShootEvent ();

	public float nextShootTime = 0.0f;

	public override void ApplyAnimationOverrides (ref AnimationClipOverrides overrides)
	{
		if (firstPerson) {
			overrides [idleOverrideNameFP] = idleClipFP;
			overrides [fireOverrideNameFP] = fireClipFP;
			overrides [drawOverrideNameFP] = drawClipFP;
			overrides [reloadOverrideNameFP] = reloadClipFP;
			overrides [fullReloadOverrideNameFP] = fullReloadClipFP;
			overrides [unjammingOverrideNameFP] = unjammingClipFP;
			overrides [jammingOverrideNameFP] = jammingClipFP;
			overrides [saveOverrideNameFP] = saveClipFP;
			overrides [punchOverrideNameFP] = punchNotHitClipFP;
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
		ChangeAnimation (idleOverrideNameFP, idleClipFP);
		ChangeAnimation (saveOverrideNameFP, saveClipFP);
	}

	public void Jam() {
		StartCoroutine (PlayAnimation (jammingOverrideNameFP, jammingClipFP.length));
		ChangeAnimation (idleOverrideNameFP, jammedIdleClipFP);
		ChangeAnimation (saveOverrideNameFP, saveJammedClipFP);
	}

	public IEnumerator LaunchBulletDrop() {
		yield return new WaitForSeconds (bulletDropWaitSecondsAfterShoot * fireAnimationSpeed);
		GameObject bulletDrop = Instantiate (bulletDropPrefab);
		bulletDrop.transform.position = bulletDropLaunch.position;
		Rigidbody body = bulletDrop.GetComponent<Rigidbody> ();
		body.AddForce (bulletDropLaunch.forward * bulletDropForce, ForceMode.Impulse);
		Destroy (bulletDrop, bulletDropKillTime);
	}

	public override void PrimaryFire () {
		if (save) {
			return;
		}

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
						jammed = true;
					}
				}
				if (jammed) {
					Jam ();
					NullifyNextShootTime ();
				} else {
					SetCurrentAnimationSpeed (fireAnimationSpeed);
					StartCoroutine (PlayAnimation (fireOverrideNameFP, fireClipFP.length, NullifyNextShootTime));
					StartCoroutine (LaunchBulletDrop ());
				}
			}
			afterShootEvent.Invoke ();
			currentDurability--;
			shootFireParticles.Play ();
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
			if (inSightApproved && !save) {
				offsetTransform.localPosition = Vector3.Lerp (offsetTransform.localPosition, Vector3.zero, Time.deltaTime * goingInSightSpeed);
			} else {
				offsetTransform.localPosition = Vector3.Lerp (offsetTransform.localPosition, offsetInitialPosition, Time.deltaTime * goingOutSightSpeed);
			}
		}
		nextShootTime += Time.deltaTime;
		FirearmUpdate ();
	}

	public override void Reload() {
		if (save) {
			return;
		} 

		if (restrictUsing == true) {
			return;
		}

		if (FirearmReloadStart () == false) {
			return;
		}

		if (animationPlaying) {
			return;
		}

		if(firstPerson){
			if (jammed) {
				SetCurrentAnimationSpeed (unjammingAnimationSpeed);
				StartCoroutine(PlayAnimation(unjammingOverrideNameFP, unjammingClipFP.length, NullifyJammedState)); 
			} else {
				if (currentAmmo > 0) {
					SetCurrentAnimationSpeed (reloadAnimationSpeed);
					StartCoroutine (PlayAnimation (reloadOverrideNameFP, reloadClipFP.length, FirearmReloadAnimationEnd));
				} else {
					SetCurrentAnimationSpeed (fullReloadAnimationSpeed);
					StartCoroutine (PlayAnimation (fullReloadOverrideNameFP, fullReloadClipFP.length, FirearmReloadAnimationEnd));
				}
			}
		}

		FirearmReloadEnd ();
	}

	public override void Punch() {
		if (animationPlaying) {
			return;
		}

		if (restrictUsing == true) {
			return;
		}

		if (firstPerson) {
			RaycastHit hit = Player.instance.character.Raycast ();
			AnimationClip currentClip = null;
			if (hit.distance > punchHitDistanceCheck) {
				ChangeAnimation (punchOverrideNameFP, punchNotHitClipFP);
				currentClip = punchNotHitClipFP;
				PunchNotHit (hit);
			} else {
				ChangeAnimation (punchOverrideNameFP, punchHitClipFP);
				currentClip = punchHitClipFP;
				PunchHit (hit);
			}
			SetCurrentAnimationSpeed (punchAnimationSpeed);
			StartCoroutine (PlayAnimation (punchOverrideNameFP, currentClip.length));
		}
	}

	public abstract void PunchHit (RaycastHit hit);
	public abstract void PunchNotHit (RaycastHit hit);

	public override void CustomStartOnBegin () {
		offsetInitialPosition = offsetTransform.localPosition;

		GameObject shootFire = Instantiate (shootFireParticlePrefab, shootFireTransform);
		shootFireParticles = shootFire.GetComponent<ParticleSystem> ();

		FirearmStart ();
	}

	public override void Save () {
		save = true;
		animator.SetBool (saveOverrideNameFP, save);
	}

	public override void UnSave () {
		save = false;
		animator.SetBool (saveOverrideNameFP, save);
	}
}
