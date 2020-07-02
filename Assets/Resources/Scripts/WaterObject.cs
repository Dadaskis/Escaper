using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class WaterObject : MonoBehaviour {

	//public bool canClimb = false;
	//public float speed = 1.0f;
	private float gravity = 0.0f;
	private bool playerInWater = false;
	private float timerWithoutPlayer = 0.0f;

	void OnTriggerEnter(Collider collider) {

		if (collider.gameObject.isStatic) {
			return;
		}

		if (collider.transform.gameObject.tag == "PlayerWaterEffect") {
			Player.instance.underWaterPostProcessing.enabled = true;
		}

		if (playerInWater == true) {
			return;
		}

		if (collider.transform.gameObject.tag == "PlayerWater") {
			playerInWater = true;
			//gravity = Player.instance.controller.gravityMultiplier;
			//Player.instance.controller.gravityMultiplier = 0.0f;
			//Player.instance.controller.inertionInAir = false;
			Player.instance.controller.enableLogic = false;
			//Player.instance.underWaterPostProcessing.enabled = true;
			if (timerWithoutPlayer > 1.0f) {
				speed = Player.instance.controller.characterController.velocity.normalized * Time.deltaTime * 8.0f;
				timerWithoutPlayer = 0.0f;
			}
			//Player.instance.rigidBody.useGravity = false;
		}
		
	}

	void OnTriggerStay(Collider collider) {
		Rigidbody body = collider.attachedRigidbody;
		if (body != null) {
			body.AddForce (Vector3.up * 1.8f);
		}
	}
		
	void OnTriggerExit(Collider collider) {

		if (collider.gameObject.isStatic) {
			return;
		}

		if (collider.transform.gameObject.tag == "PlayerWaterEffect") {
			Player.instance.underWaterPostProcessing.enabled = false;
		}

		if (playerInWater == false) {
			return;
		}

		if (collider.transform.gameObject.tag == "PlayerWater") {
			playerInWater = false;
			//Player.instance.controller.gravityMultiplier = gravity;
			//Player.instance.controller.inertionInAir = true;
			Player.instance.controller.enableLogic = true;
			Player.instance.controller.moveDir = speed;
			//Player.instance.controller.characterController.Move(speed);
			//Player.instance.rigidBody.useGravity = true;
		}
	}

	private Vector3 speed = Vector3.zero;
	void Update() {
		if (playerInWater) {
			Vector3 forward = Player.instance.character.raycaster.forward;

			bool buttonUsed = false;

			if (InputManager.GetButton ("PlayerForward")) {
				speed = new Vector3 (0.0f, forward.y * 1.0f, 1.0f);
				buttonUsed = true;
			}

			if (InputManager.GetButton ("PlayerBackward")) {
				speed = new Vector3 (0.0f, -forward.y * 1.0f, -1.0f);
				buttonUsed = true;
			}

			if (InputManager.GetButton ("PlayerRight")) {
				speed = new Vector3 (1.0f, 0.0f, 0.0f);
				buttonUsed = true;
			}

			if (InputManager.GetButton ("PlayerLeft")) {
				speed = new Vector3 (-1.0f, 0.0f, 0.0f);
				buttonUsed = true;
			}

			if (InputManager.GetButton ("PlayerJump")) {
				speed = new Vector3 (0.0f, 1.0f, 0.0f);
				buttonUsed = true;
			}
				
			if (buttonUsed) {
				speed *= Time.deltaTime * 4.0f;

				if (InputManager.GetButton ("PlayerRun")) {
					speed *= 2.0f;
				}
			}

			Player.instance.transform.Translate (speed);

			speed = Vector3.Lerp (speed, Vector3.zero, Time.deltaTime * 2f);
		} else {
			timerWithoutPlayer += Time.deltaTime;
		}
	}

}
