using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class LadderObject : MonoBehaviour {

	public bool canClimb = false;
	public float speed = 1.0f;
	public float pushForwardSpeed = 0.01f;
	public BoxCollider collider;
	private GameObject player;
	private float mass = 0.0f;
	private float timer = 0.0f;

	void Start() {
		collider = GetComponent<BoxCollider> ();
	}

	void OnTriggerEnter(Collider collider) {
		if (canClimb) {
			return;
		}

		if (timer > 0.0f) {
			return;
		}

		if (collider.gameObject.isStatic) {
			return;
		}

		if (collider.transform.root.gameObject.tag == "Player") {
			player = collider.gameObject;
			//Player.instance.rigidBody.useGravity = false;
			Player.instance.controller.enableLogic = false;
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
			//Player.instance.rigidBody.useGravity = true;
			Player.instance.controller.enableLogic = true;
			//Player.instance.rigidBody.AddRelativeForce (speedVector * 150.0f);
			timer = 1.0f;
		}
	}

	Vector3 speedVector = Vector3.zero;
	void Update() {
		timer -= Time.deltaTime;
		if (canClimb) {
			if (InputManager.GetButton ("PlayerForward")) {
				float distance = Vector3.Distance (player.transform.position, collider.bounds.center + collider.bounds.extents);
				float upDistance = Mathf.Max(0.0f, ((collider.bounds.extents.y * 0.65f) - distance));
				Vector3 speedVector = new Vector3 (
					                0.0f, 
					                1.0f + (upDistance * 2.0f), 
					                pushForwardSpeed + upDistance
				                ) * Time.deltaTime * speed;
				player.transform.Translate(speedVector);
			}

			if (InputManager.GetButton ("PlayerBackward")) {
				float distance = Vector3.Distance (player.transform.position, collider.bounds.center - collider.bounds.extents);
				player.transform.Translate(new Vector3(0.0f, -1.0f, Mathf.Min(0.0f, distance - collider.bounds.extents.y)) * Time.deltaTime * speed);
				speedVector = Vector3.zero;
			}
		}
	}

}
