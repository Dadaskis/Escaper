using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events {
	class RaycastFirearmStartShoot {}
	class RaycastFirearmBulletHit {}
	class RaycastFirearmStartReload {}
	class RaycastFirearmEndReload {}
}

public class RaycastFirearm : IFirearm {
	[Header("RaycastFirearm")]

	[Header("Animations")]
	public string fullReloadOverrideNameFP = "WeaponFullReload";
	public string fullReloadAnimationFindNameFP = "WeaponFullReload";
	public AnimationClip fullReloadClipFP;
	public string jammingShootAnimationFindNameFP = "WeaponJammingShoot";
	public AnimationClip jammingShootClipFP;

	[Header("Sounds")]
	public string jammingSoundName = "";
	public string fullReloadSoundName = "";

	[Header("Recoil")]
	public float maxXRecoil = 12.0f;
	public float minXRecoil = 2.0f;
	public float maxYRecoil = 5.0f;
	public float minYRecoil = 2.0f;
	public float maxZRecoil = 2.0f;
	public float minZRecoil = 2.0f;

	[Header("Jamming")]
	public bool jammable = true;
	public int jammed = 0;
	public int minJammingChance = 1;
	public int maxJammingChance = 10;

	[Header("Other")]
	public GameObject tracerPrefab;

	public bool CheckJamming() {
		if (jammable) {
			if (jammingShootClipFP == null) {
				return false;
			}
			int procent = Mathf.RoundToInt (((float)currentDurability / (float)maxDurability) * 100.0f);
			int jammingChance = Random.Range (minJammingChance, maxJammingChance);
			if (jammingChance > procent) {
				return true;
			}
		}
		return false;
	}

	public override bool ShootAnimationOverride () {
		if (jammed > 0) {
			return true;
		}

		if (CheckJamming ()) {
			jammed++;
			SetCurrentAnimationSpeed (1.0f);
			ChangeAnimation (fireOverrideNameFP, jammingShootClipFP);
			PlayAnimation (fireOverrideNameFP, jammingShootClipFP.length, delegate {
				jammed--;
			});
			SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData (jammingSoundName);
			if (firstPerson) {
				data.spatialBlend = 0.0f;
			}
			SoundManager.instance.CreateSound (data, Vector3.zero, transform);
			StandardAfterShootAnimation ();
			return true;
		}

		return false;
	}

	public override bool ReloadOverrideAnimation () {
		if (currentAmmo <= 0) {
			ChangeAnimation (reloadOverrideNameFP, fullReloadClipFP);
			PlayAnimation (reloadOverrideNameFP, fullReloadClipFP.length, ReloadEnd);
			SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData (fullReloadSoundName);
			if (firstPerson) {
				data.spatialBlend = 0.0f;
			}
			SoundManager.instance.CreateSound (data, Vector3.zero, transform);
			return true;
		}

		return false;
	}

	public override void Shoot () {
		EventData eventData = EventManager.RunEventListeners<Events.RaycastFirearmStartShoot> (this);
		if (eventData.Count > 0) {
			return;
		}

		if (firstPerson) {
			CameraPuncher.instance.Punch (
				new Vector3 (
					Random.Range (-maxXRecoil, minXRecoil), 
					Random.Range (-maxYRecoil, minYRecoil), 
					Random.Range (-maxZRecoil, minZRecoil)
				)
			);
		}
		RaycastHit hit = this.owner.Raycast ();

		Vector3 hitPos = hit.point;

		GameObject tracer = Instantiate (tracerPrefab);
		TracerObject tracerObject = tracer.GetComponent<TracerObject> ();
		tracerObject.SetLineSettings (this.owner.raycaster.position, hitPos);
		//GameObject obj = Instantiate(onHitObjects [Random.Range (0, onHitObjects.Count)]);
		//obj.transform.position = hitPos;

		EventManager.RunEventListeners<Events.RaycastFirearmBulletHit> (hitPos);
	}
	
}
