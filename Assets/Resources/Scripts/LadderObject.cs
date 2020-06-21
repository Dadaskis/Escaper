using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class LadderObject : MonoBehaviour {

	public bool canClimb = false;
	public float speed = 1.0f;
	private GameObject player;
	private float gravity = 0.0f;

	void OnTriggerEnter(Collider collider) {
		if (canClimb) {
			return;
		}

		if (collider.gameObject.isStatic) {
			return;
		}

		if (collider.transform.root.gameObject.tag == "Player") {
			player = collider.gameObject;
			//Player.instance.controller.enabled = false;
			gravity = Player.instance.controller.gravityMultiplier;
			Player.instance.controller.gravityMultiplier = 0.0f;
			canClimb = true;
		}
	}

	void OnTriggerExit(Collider collider) {
		if (!canClimb) {
			return;
		}

		if (collider.gameObject.isStatic) {
			return;
		}
		if (collider.transform.root.gameObject.tag == "Player") {
			canClimb = false;
			Player.instance.controller.gravityMultiplier = gravity;
			player = null;
		}
	}

	void Update() {
		if (canClimb) {
			if (InputManager.GetButton ("PlayerForward")) {
				player.transform.Translate(new Vector3(0.0f, 1.0f, 0.0f) * Time.deltaTime * speed);
			}

			if (InputManager.GetButton ("PlayerBackward")) {
				player.transform.Translate(new Vector3(0.0f, -1.0f, 0.0f) * Time.deltaTime * speed);
			}
		}
	}

}
