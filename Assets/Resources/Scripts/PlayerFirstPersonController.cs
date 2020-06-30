using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFirstPersonController : MonoBehaviour {

	public Rigidbody body;
	public float speed = 1.0f;

	Vector3 GetInput() {
		Vector3 inputSpeed = new Vector3 ();

		if (InputManager.GetButton ("PlayerForward")) {
			inputSpeed.z += 1.0f;
		}

		if (InputManager.GetButton ("PlayerBackward")) {
			inputSpeed.z -= 1.0f;
		}

		if (InputManager.GetButton ("PlayerLeft")) {
			inputSpeed.x -= 1.0f;
		}

		if (InputManager.GetButton ("PlayerRight")) {
			inputSpeed.x += 1.0f;
		}

		if (InputManager.GetButton ("PlayerJump")) {
			inputSpeed.y += 1.0f;
		}

		return inputSpeed;
	}

	void Update () {
		Vector3 inputSpeed = GetInput ();

		body.AddForce (inputSpeed * speed);
	}
}
