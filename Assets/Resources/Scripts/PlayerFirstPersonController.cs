using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerFirstPersonController : MonoBehaviour {

	public Rigidbody body;
	public Transform mouseLookTransform;
	public float speed = 1.0f;
	public MouseLook mouseLook = new MouseLook ();

	void Start() {
		mouseLook.Init (transform, mouseLookTransform);
	}

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

	private void RotateView()
	{
		//avoids the mouse looking if the game is effectively paused
		if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

		// get the rotation before it's changed
		//float oldYRotation = transform.eulerAngles.y;

		mouseLook.LookRotation (transform, mouseLookTransform);

		//if (m_IsGrounded || advancedSettings.airControl)
		//{
		//	// Rotate the rigidbody velocity to match the new direction that the character is looking
		//	Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
		//	m_RigidBody.velocity = velRotation*m_RigidBody.velocity;
		//}
	}

	void Update () {
		Vector3 inputSpeed = GetInput ();
		RotateView ();

		Vector3 speedVector = transform.forward;
		inputSpeed = Vector3.ProjectOnPlane (inputSpeed, transform.forward);
		speedVector.x *= inputSpeed.x * speed;
		speedVector.z *= inputSpeed.z * speed;

		body.AddForce (speedVector);
	}
}
