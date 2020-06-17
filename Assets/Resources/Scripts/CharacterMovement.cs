using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

	public CharacterController controller;
	public Transform forward;
	public float speed = 1.0f;
	public float jumpPower = 1.0f;

	Vector3 move = new Vector3();

	void Update () {
		if (!controller.isGrounded) {
			move.y -= 0.02f;
		} else {
			float horizontal = Input.GetAxis ("Horizontal");
			float vertical = Input.GetAxis ("Vertical");
			move = forward.forward * vertical;
			move += forward.right * horizontal;
			move *= speed;
			if (Input.GetKey (KeyCode.Space)) {
				move.y += jumpPower;
			}
		}
		controller.Move (move);
	}
}
