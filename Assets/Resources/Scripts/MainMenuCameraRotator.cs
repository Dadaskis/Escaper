using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraRotator : MonoBehaviour {
	public float rotatePower = 0.1f;
	public float rotateSpeed = 0.2f;
	public GameObject canvas;

	void Update () {
		Vector3 mousePosition = Input.mousePosition;
		Vector3 forward = canvas.transform.position;
		forward.x += (mousePosition.x / Screen.width) * rotatePower;
		forward.y += (mousePosition.y / Screen.height) * rotatePower;
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation (forward - transform.position), rotateSpeed);

		Vector3 localPosition = transform.localPosition;
		localPosition.y = Mathf.Sin (Time.time) / 50.0f;
		transform.localPosition = localPosition;
	}
}
