using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Events {
	class IFirearmInSight {}
}

public class IFirearm : IWeapon {
	public string modelPath;
	public string idleOverrideNameFP = "WeaponIdle";
	public string idleAnimationFindNameFP = "WeaponIdle";
	public string jammedIdleAnimationFindNameFP = "WeaponJammedIdle";
	public AnimationClip idleClipFP;
	public AnimationClip jammedIdleClipFP;
	public string fireOverrideNameFP = "WeaponShoot";
	public string fireAnimationFindNameFP = "WeaponShoot";
	public AnimationClip fireClipFP;
	public string drawOverrideNameFP = "WeaponDraw";
	public string takeoutOverrideNameFP = "WeaponTakeout";
	public string drawAnimationFindNameFP = "WeaponDraw";
	public AnimationClip drawClipFP;
	public string reloadOverrideNameFP = "WeaponReload";
	public string reloadAnimationFindNameFP = "WeaponReload";
	public AnimationClip reloadClipFP;
	public string fullReloadOverrideNameFP = "WeaponFullReload";
	public string fullReloadAnimationFindNameFP = "WeaponFullReload";
	public AnimationClip fullReloadClipFP;
	public string jammingOverrideNameFP = "WeaponJamming";
	public string jammingAnimationFindNameFP = "WeaponJamming";
	public AnimationClip jammingClipFP;
	public string unjammingOverrideNameFP = "WeaponUnjamming";
	public string unjammingAnimationFindNameFP = "WeaponUnjamming";
	public AnimationClip unjammingClipFP;
	public string punchOverrideNameFP = "WeaponPunch";
	public string punchNotHitAnimationFindNameFP = "WeaponPunchHit";
	public string punchHitAnimationFindNameFP = "WeaponPunchNotHit";
	public AnimationClip punchNotHitClipFP;
	public AnimationClip punchHitClipFP;
	public string saveOverrideNameFP = "WeaponSave";
	public string saveAnimationFindNameFP = "WeaponSave";
	public string saveJammedAnimationFindNameFP = "WeaponJammedSave";
	public AnimationClip saveClipFP;
	public AnimationClip saveJammedClipFP;
	public string checkMagOverrideNameFP = "WeaponCheckMag";
	public string checkMagAnimationFindNameFP = "WeaponCheckMag";
	public AnimationClip checkMagClipFP;

	public string startOverrideNameTP = "WeaponStart";
	public string startAnimationFindNameTP = "WeaponStart";
	public AnimationClip startClipTP;
	public string endOverrideNameTP = "WeaponEnd";
	public string endAnimationFindNameTP = "WeaponEnd";
	public AnimationClip endClipTP;
	public string saveOverrideNameTP = "WeaponSave";
	public string saveAnimationFindNameTP = "WeaponSave";
	public AnimationClip saveClipTP;
	public string idleOverrideNameTP = "WeaponIdle";
	public string idleAnimationFindNameTP = "WeaponIdle";
	public AnimationClip idleClipTP;
	public string shootOverrideNameTP = "WeaponShoot";
	public string shootAnimationFindNameTP = "WeaponShoot";
	public AnimationClip shootClipTP;
	public string unjammingOverrideNameTP = "WeaponUnjamming";
	public string unjammingAnimationFindNameTP = "WeaponUnjamming";
	public AnimationClip unjammingClipTP;
	public string reloadOverrideNameTP = "WeaponReload";
	public string reloadAnimationFindNameTP = "WeaponReload";
	public AnimationClip reloadClipTP;

	public bool save = false;
	public bool reloading = false;

	public float fireAnimationSpeed = 1.0f;
	public float drawAnimationSpeed = 1.0f;
	public float reloadAnimationSpeed = 1.0f;
	public float fullReloadAnimationSpeed = 1.0f;
	public float unjammingAnimationSpeed = 1.0f;
	public float punchAnimationSpeed = 1.0f;
	public float checkMagAnimationSpeed = 1.0f;

	public string reloadSoundName = "";
	public string fullReloadSoundName = "";
	public string shootSoundName = "";
	public string unjammingSoundName = "";
	public string magCheckSoundName = "";

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
	public Transform ammoTransform;
	public bool drawAmmoOnlyOnCheckMag = true;
	public float ammoFulfillSecondsOnReload = 1.0f;

	public class AfterShootEvent : UnityEvent {}
	public AfterShootEvent afterShootEvent = new AfterShootEvent ();

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
			overrides [checkMagOverrideNameFP] = checkMagClipFP;
		} else {
			overrides [idleOverrideNameTP] = idleClipTP;
			overrides [shootOverrideNameTP] = shootClipTP;
			overrides [startOverrideNameTP] = startClipTP;
			overrides [endOverrideNameTP] = endClipTP;
			overrides [unjammingOverrideNameTP] = unjammingClipTP;
			overrides [endOverrideNameTP] = endClipTP;
			overrides [reloadOverrideNameTP] = reloadClipTP;
		}
	}

	public virtual void Shoot () {}
	public virtual void FirearmStart () {}
	public virtual void FirearmUpdate () {}
	public virtual bool FirearmReloadStart () { return true; }
	public virtual void FirearmReloadEnd () {}
	public virtual void FirearmReloadAnimationEnd () {}

	public void NullifyJammedState() {
		jammed = false;
		ChangeAnimation (idleOverrideNameFP, idleClipFP);
		ChangeAnimation (saveOverrideNameFP, saveClipFP);
	}

	public void UnJam() {
		if (firstPerson) {
			SetCurrentAnimationSpeed (unjammingAnimationSpeed);
			StartCoroutine (PlayAnimation (unjammingOverrideNameFP, unjammingClipFP.length, NullifyJammedState)); 
			SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData (unjammingSoundName);
			data.spatialBlend = 0.0f;
			SoundManager.instance.CreateSound (data, Vector3.zero, transform);
		}
	}

	public void UnJamTP() {
		if (!firstPerson) {
			jammed = false;
		}
	}

	public void Jam() {
		if (firstPerson) {
			StartCoroutine (PlayAnimation (jammingOverrideNameFP, jammingClipFP.length, UnJam));
			ChangeAnimation (idleOverrideNameFP, jammedIdleClipFP);
			ChangeAnimation (saveOverrideNameFP, saveJammedClipFP);
		} else {
			StartCoroutine (PlayAnimation (unjammingOverrideNameTP, unjammingClipTP.length, UnJamTP));
			SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData (unjammingSoundName);
			data.minDistance = 50.0f;
			data.maxDistance = 100.0f;
			SoundManager.instance.CreateSound (data, Vector3.zero, transform);
		}
	}

	public IEnumerator LaunchBulletDrop() {
		if (bulletDropPrefab == null) { 
			yield break;
		}

		yield return new WaitForSeconds (bulletDropWaitSecondsAfterShoot / fireAnimationSpeed / overallWeaponsAnimationSpeedModifier);
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

		if (animationPlaying) {
			return;
		}

		if (jammable) {
			int procent = Mathf.RoundToInt (((float)currentDurability / (float)maxDurability) * 100.0f);
			int jammingChance = Random.Range (minJammingChance, maxJammingChance);
			if (jammingChance > procent) {
				jammed = true;
			}
		}
		if (jammed) {
			Jam ();
		} else {
			SetCurrentAnimationSpeed (fireAnimationSpeed);
			if (firstPerson) {
				StartCoroutine (PlayAnimation (fireOverrideNameFP, fireClipFP.length));
				StartCoroutine (LaunchBulletDrop ());
			} else {
				StartCoroutine (PlayAnimation (shootOverrideNameTP, shootClipTP.length));
				StartCoroutine (LaunchBulletDrop ());
			}
		}
		currentDurability--;
		if (shootFireParticles != null) {
			shootFireParticles.Play ();
		}
		SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData (shootSoundName);
		data.minDistance = 4.0f;
		data.maxDistance = 40.0f;
		if (firstPerson) {
			data.spatialBlend = 0.0f;
		}
		SoundManager.instance.CreateSound (data, transform.position, transform);
		Shoot ();
		base.PrimaryFire ();
	}

	public override void SingleSecondaryFire () {
		if (restrictUsing == true) {
			return;
		}
		inSight = !inSight;
		PlayTurningSound ();
	}

	private Quaternion previousRotation = Quaternion.identity;
	public float turningValueCheck = 0.2f;
	public float turningValueCheckMax = 0.5f;
	public List<string> turningSoundNames = new List<string>();
	public float turningTimer = 0.0f;
	public float turningDelay = 0.6f;

	public int fulfillAmmo;

	public virtual void PlayTurningSound() {
		if (turningSoundNames.Count > 0) {
			turningTimer = 0.0f;
			int index = Random.Range (0, turningSoundNames.Count);
			SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData (turningSoundNames [index]);
			data.spatialBlend = 0.0f;
			SoundManager.instance.CreateSound (data, Vector3.zero, transform);
		}
	}

	public virtual void UpdateTurningSound() {
		turningTimer += Time.deltaTime;

		if (turningTimer > turningDelay) {
			float xRotation = Mathf.Abs(previousRotation.x - transform.rotation.x);
			float yRotation = Mathf.Abs(previousRotation.y - transform.rotation.y);

			if (xRotation > turningValueCheck && xRotation < turningValueCheckMax) {
				PlayTurningSound ();
			} else if (yRotation > turningValueCheck && yRotation < turningValueCheckMax) {
				PlayTurningSound ();
			}
		}

		previousRotation = transform.rotation;
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
			if (inSightApproved && !save && !reloading && !jammed) {
				offsetTransform.localPosition = Vector3.Lerp (offsetTransform.localPosition, Vector3.zero, Time.deltaTime * goingInSightSpeed);
			} else {
				offsetTransform.localPosition = Vector3.Lerp (offsetTransform.localPosition, offsetInitialPosition, Time.deltaTime * goingOutSightSpeed);
			}
		}
		UpdateTurningSound ();
		FirearmUpdate ();
	}

	public override void Reload() {
		if (currentAmmo >= maxAmmo) {
			return;
		}

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

		if (ammoFiller != null) {
			fulfillAmmo = ammoFiller.Reload (this);
			if (fulfillAmmo <= currentAmmo) {
				return;
			}
		} else {
			fulfillAmmo = maxAmmo;
		}

		reloading = true;

		if (firstPerson) {
			if (ammoTransform != null) {
				ammoTransform.gameObject.SetActive (true);
				DrawOnlyExistAmmo ();
			}

			if (currentAmmo > 0) {
				SetCurrentAnimationSpeed (reloadAnimationSpeed);
				StartCoroutine (PlayAnimation (reloadOverrideNameFP, reloadClipFP.length, ReloadEnd));
				SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData (reloadSoundName);
				data.spatialBlend = 0.0f;
				SoundManager.instance.CreateSound (data, Vector3.zero, transform);
			} else {
				SetCurrentAnimationSpeed (fullReloadAnimationSpeed);
				StartCoroutine (PlayAnimation (fullReloadOverrideNameFP, fullReloadClipFP.length, ReloadEnd));
				SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData (fullReloadSoundName);
				data.spatialBlend = 0.0f;
				SoundManager.instance.CreateSound (data, Vector3.zero, transform);
			}

			if (ammoTransform != null) {
				StartCoroutine (FulfillAmmoOnReload ());
			}
		} else {
			SetCurrentAnimationSpeed (reloadAnimationSpeed);
			StartCoroutine (PlayAnimation (reloadOverrideNameTP, reloadClipTP.length, ReloadEnd));
			SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData (reloadSoundName);
			data.minDistance = 4.0f;
			data.maxDistance = 10.0f;
			SoundManager.instance.CreateSound (data, Vector3.zero, transform);
		}

		FirearmReloadEnd ();
		base.Reload ();
	}

	public void ReloadEnd() {
		if (firstPerson) {
			if (ammoTransform != null) {
				ammoTransform.gameObject.SetActive (false);
			}
		}
		reloading = false;
		FirearmReloadAnimationEnd ();
	}

	public override void Punch() {
		if (jammed) {
			return;
		}

		if (animationPlaying) {
			return;
		}

		if (restrictUsing == true) {
			return;
		}

		if (firstPerson) {
			PlayTurningSound ();
			//RaycastHit hit = Player.instance.character.Raycast ();
			RaycastHit hit;
			bool status = Physics.BoxCast (Player.instance.character.raycaster.position, Vector3.one, Player.instance.character.raycaster.forward, out hit);
			AnimationClip currentClip = null;
			if (!status || hit.distance > punchHitDistanceCheck) {
				ChangeAnimation (punchOverrideNameFP, punchNotHitClipFP);
				currentClip = punchNotHitClipFP;
				PunchNotHit (hit);
			} else if(status) {
				ChangeAnimation (punchOverrideNameFP, punchHitClipFP);
				currentClip = punchHitClipFP;
				PunchHit (hit);
			}
			SetCurrentAnimationSpeed (punchAnimationSpeed);
			StartCoroutine (PlayAnimation (punchOverrideNameFP, currentClip.length));
		}
	}

	public virtual void PunchHit (RaycastHit hit) {}
	public virtual void PunchNotHit (RaycastHit hit) {}

	public override void CustomStartOnBegin () {
		if (offsetTransform == null) {
			offsetTransform = transform.Find ("__Offset");
			if (offsetTransform) {
				offsetInitialPosition = offsetTransform.localPosition;
			}
		}

		if (shootFireParticlePrefab) {
			if (shootFireTransform == null) {
				shootFireTransform = offsetTransform.Find ("__ShootParticlePosition");
			}

			GameObject shootFire = Instantiate (shootFireParticlePrefab, shootFireTransform);
			shootFireParticles = shootFire.GetComponent<ParticleSystem> ();
		}

		if (bulletDropLaunch == null) {
			bulletDropLaunch = offsetTransform.Find ("__BulletDrop");
		}

		if (firstPerson) {
			if (ammoTransform == null) {
				ammoTransform = offsetTransform.Find ("__Ammo");
			}

			if (ammoTransform != null && drawAmmoOnlyOnCheckMag) {
				ammoTransform.gameObject.SetActive (false);
			}
		}

		FirearmStart ();
	}

	public override void Save () {
		if (firstPerson) {
			save = true;
			animator.SetBool (saveOverrideNameFP, save);
		} else {
			save = true;
			animator.SetBool (saveOverrideNameTP, save);
		}
		PlayTurningSound ();
	}

	public override void UnSave () {
		if (firstPerson) {
			save = false;
			animator.SetBool (saveOverrideNameFP, save);
		} else {
			save = false;
			animator.SetBool (saveOverrideNameTP, save);
		}
		PlayTurningSound ();
	}

	public IEnumerator FulfillAmmoOnReload() {
		yield return new WaitForSeconds (ammoFulfillSecondsOnReload);
		if (ammoTransform != null) {
			for (int index = 0; index < ammoTransform.childCount; index++) {
				ammoTransform.GetChild (index).gameObject.SetActive (true);
			}
		}
	}

	public void DrawOnlyExistAmmo () {
		if (ammoTransform == null) {
			return;
		}

		for (int index = 0; index <= maxAmmo; index++) {
			int ammoIndex = maxAmmo - index;
			Transform ammo = ammoTransform.Find (ammoIndex.ToString());
			if (ammo != null) {
				if (ammoIndex >= currentAmmo) {
					ammo.gameObject.SetActive (false);
				} else {
					ammo.gameObject.SetActive (true);
				}
			}
		}
	}

	public override void MagCheck () {
		if (firstPerson) {
			if (jammed) {
				return;
			}

			if (drawAmmoOnlyOnCheckMag && ammoTransform != null) {
				ammoTransform.gameObject.SetActive (true);
			}
			DrawOnlyExistAmmo ();

			reloading = true;
			SetCurrentAnimationSpeed (checkMagAnimationSpeed);
			StartCoroutine (PlayAnimation (checkMagOverrideNameFP, checkMagClipFP.length, delegate {
				reloading = false;
			}));

			SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData (magCheckSoundName);
			data.minDistance = 2.0f;
			data.maxDistance = 4.0f;
			if (firstPerson) {
				data.spatialBlend = 0.0f;
			}
			SoundManager.instance.CreateSound (data, Vector3.zero, transform);

			GUIAmmoText.instance.ShowText (this);
		}
	}

	public override float Takeout () {
		if (animationPlaying) {
			return -1.0f;
		}
		StartCoroutine (PlayAnimation (takeoutOverrideNameFP, drawClipFP.length));
		return drawClipFP.length;
	}

	public override float Draw () {
		if (animationPlaying) {
			return -1.0f;
		}
		StartCoroutine (PlayAnimation (drawOverrideNameFP, drawClipFP.length));
		return drawClipFP.length;
	}
}