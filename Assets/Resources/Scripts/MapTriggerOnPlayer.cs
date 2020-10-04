using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTriggerOnPlayer : MonoBehaviour {

	public MapTriggerCaller caller;

	public bool oneTimeUsable = false;
	public float delay = 0.0f;
	private float timer = 999999.0f;
	private bool used = false;

	void Update() {
		if (used && oneTimeUsable) {
			return;
		}

		timer += Time.deltaTime;
	}

	void OnTriggerStay(Collider collider) {

		if (used && oneTimeUsable) {
			return;
		}

		if (collider.gameObject.isStatic) {
			return;
		}

		if (collider.transform.gameObject.tag != "Player") {
			return;
		}

		if (timer > delay) {
			caller.Call ();
			timer = 0.0f;
			used = true;
		}

	}
}
