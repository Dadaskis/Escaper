using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotate : MonoBehaviour {

	public float sensitivity = 0.0f;

	float rotationY = 0.0f;

	void Start() {
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update () {
		float mouseX = Input.GetAxis ("Mouse X");
		float mouseY = Input.GetAxis ("Mouse Y"); 

		float rotationX = transform.localEulerAngles.y + mouseX * sensitivity;
		rotationY += mouseY * sensitivity;
		rotationY = Mathf.Clamp (rotationY, -75.0f, 90.0f);

		transform.localEulerAngles = new Vector3 (-rotationY, rotationX, 0.0f);
	}

}
