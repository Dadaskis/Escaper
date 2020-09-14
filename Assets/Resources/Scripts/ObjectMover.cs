using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour {

	public Vector3 offsetDestination;
	public float speed = 1.0f;

	private bool moving = false;
	private Vector3 previousPosition;

	public void StartMove() {
		previousPosition = transform.position;
		moving = true;
	}

	void Update() {
		if (moving) {
			transform.position = Vector3.Lerp (transform.position, previousPosition + offsetDestination, Time.deltaTime * speed);
		}
	}

}
