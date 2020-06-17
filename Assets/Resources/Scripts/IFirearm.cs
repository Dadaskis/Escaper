using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public abstract class IFirearm : IWeapon {
	public int durability = 100;
	public int fireRate = 100;

	public AnimationClip idleClipFP;
	public AnimationClip fireClipFP;
	public float fireFPDuration;
	public AnimationClip drawClipFP;
	public float drawFPDuration;
	public AnimationClip runClipFP;
	public float runFPDuration;
	public AnimationClip reloadClipFP;
	public float reloadFPDuration;
	public AnimationClip fullReloadClipFP;
	public float fullReloadFPDuration;

	public AnimationClip idleClipTP;
	public AnimationClip fireClipTP;
	public float fireTPDuration;
	public AnimationClip drawClipTP;
	public float drawTPDuration;
	public AnimationClip runClipTP;
	public float runTPDuration;
	public AnimationClip reloadClipTP;
	public float reloadTPDuration;
	public AnimationClip fullReloadClipTP;
	public float fullReloadTPDuration;

	public Vector3 sightPosition;
	public Vector3 nonSightPosition;
	public bool inSight = false;

	public class AfterShootEvent : UnityEvent {}
	public AfterShootEvent afterShootEvent = new AfterShootEvent ();

	public float nextShootTime = 0.0f;

	public override void ApplyAnimationOverrides (ref AnimationClipOverrides overrides)
	{
		if (firstPerson) {
			overrides ["Idle"] = idleClipFP;
			overrides ["Fire"] = fireClipFP;
			overrides ["Draw"] = drawClipFP;
			overrides ["Run"] = runClipFP;
			overrides ["Reload"] = reloadClipFP;
			overrides ["FullReload"] = fullReloadClipFP;
		} else {
			overrides ["Idle"] = idleClipTP;
			overrides ["Fire"] = fireClipTP;
			overrides ["Draw"] = drawClipTP;
			overrides ["Run"] = runClipTP;
			overrides ["Reload"] = reloadClipTP;
			overrides ["FullReload"] = fullReloadClipTP;
		}
	}

	public abstract void Shoot ();
	public abstract void FirearmStart ();
	public abstract void FirearmUpdate ();

	public override void PrimaryFire () {
		if (nextShootTime > (60.0f / fireRate)) {
			nextShootTime = 0.0f;
			Shoot ();
			afterShootEvent.Invoke ();
		}
	}

	public override void SingleSecondaryFire () {
		inSight = !inSight;
	}

	void Update() {
		if (firstPerson) {
			if (inSight) {
				transform.localPosition = Vector3.Lerp (transform.localPosition, sightPosition, Time.deltaTime * 5.0f);
			} else {
				transform.localPosition = Vector3.Lerp (transform.localPosition, nonSightPosition, Time.deltaTime * 3.0f);
			}
		}
		nextShootTime += Time.deltaTime;
		FirearmUpdate ();
	}

	public override void CustomStartOnBegin () {
		sightPosition = this.CalculateSightPosition(transform, transform.Find ("__Sight"));
		nonSightPosition = transform.localPosition;
		FirearmStart ();
	}
}
