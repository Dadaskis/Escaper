using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponNearWallPushBack : MonoBehaviour {
	public float minDistance = 1.0f;
	public float speed = 8.0f;
	private Transform myTransform;
	private bool colliding = false;

	void Start() {
		myTransform = transform;
		EventManager.AddEventListener<Events.IFirearmInSight> (InSightChecker);
	}

	EventData InSightChecker(EventData args) {
		if (colliding) {
			EventData data = new EventData (true);
			return data;
		}
		return null;
	}

	void Update () {
		RaycastHit hit = Player.instance.character.Raycast ();	
		if (hit.distance < minDistance) {
			Vector3 position = myTransform.localPosition;
			position.z = Mathf.Lerp(position.z, -(minDistance - hit.distance), Time.deltaTime * speed);
			myTransform.localPosition = position;
			colliding = true;
		} else {
			myTransform.localPosition = Vector3.Lerp(myTransform.localPosition, Vector3.zero, Time.deltaTime * speed);
			colliding = false;
		}
	}
}
