using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastFirearm : IFirearm {
	public float maxXRecoil = 12.0f;
	public float minXRecoil = 2.0f;
	public float maxYRecoil = 5.0f;
	public float minYRecoil = 2.0f;
	public float maxZRecoil = 2.0f;
	public float minZRecoil = 2.0f;

	public List<GameObject> onHitObjects;

	public override void FirearmStart () {
		
	}

	public override void FirearmUpdate () {
		
	}

	public override void Shoot () {
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
		//GameObject obj = Instantiate(onHitObjects [Random.Range (0, onHitObjects.Count)]);
		//obj.transform.position = hitPos;

		EventManager.RunEventListeners ("BulletHit", hitPos);
	}

	public override void SinglePrimaryFire () {
		
	}

	public override void SecondaryFire () {
		
	}

	public override void Reload () {
		
	}

	public override void CustomStartOnEnd () {
	
	}
}
