using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class WaterObject : MonoBehaviour {

	//public bool canClimb = false;
	//public float speed = 1.0f;
	private float gravity = 0.0f;
	private bool playerInWater = false;

	void OnTriggerEnter(Collider collider) {
		if (playerInWater == true) {
			return;
		}

		if (collider.gameObject.isStatic) {
			return;
		}

		if (collider.transform.root.gameObject.tag == "Player") {
			playerInWater = true;
			//gravity = Player.instance.controller.gravityMultiplier;
			//Player.instance.controller.gravityMultiplier = 0.0f;
			Player.instance.controller.inertionInAir = false;
		}
	}

	void OnTriggerExit(Collider collider) {
		if (playerInWater == false) {
			return;
		}

		if (collider.gameObject.isStatic) {
			return;
		}

		if (collider.transform.root.gameObject.tag == "Player") {
			playerInWater = false;
			//Player.instance.controller.gravityMultiplier = gravity;
			Player.instance.controller.inertionInAir = true;
		}
	}

	void Update() {
		if (playerInWater) {
			if (InputManager.GetButton ("PlayerForward")) {
				//body.AddForce (Player.instance.character.raycaster.forward * 10.0f);
				Vector3 forward = Player.instance.character.raycaster.forward;
				CharacterController controller = Player.instance.controller.characterController;
				controller.SimpleMove (forward);
			}
		}
	}

}
