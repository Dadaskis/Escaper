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
	public float maxXRecoil = 12.0f;
	public float minXRecoil = 2.0f;
	public float maxYRecoil = 5.0f;
	public float minYRecoil = 2.0f;
	public float maxZRecoil = 2.0f;
	public float minZRecoil = 2.0f;
	public GameObject tracerPrefab;

	public List<GameObject> onHitObjects;

	public override void FirearmStart () {
		
	}

	public override void FirearmUpdate () {
		
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

		currentAmmo--;
	}

	public override void SinglePrimaryFire () {
		
	}

	public override void SecondaryFire () {
		
	}

	public override void FirearmReloadAnimationEnd () {
		currentAmmo = maxAmmo;

		EventManager.RunEventListeners<Events.RaycastFirearmStartReload> (this);
	}

	public override void FirearmReloadEnd () {
		EventManager.RunEventListeners<Events.RaycastFirearmEndReload> (this);
	}

	public override bool FirearmReloadStart () {
		return true;
	}

	public override void CustomStartOnEnd () {
	
	}

	public override void PunchHit (RaycastHit hit) {
		
	}

	public override void PunchNotHit (RaycastHit hit) {

	}
}
