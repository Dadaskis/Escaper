using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenOnUse : MonoBehaviour {

	public bool opened = false;
	public Vector3 openedAngle;
	public Vector3 closedAngle;
	public float speed = 0.1f;

	private Transform currentTransform;

	void Start () {
		currentTransform = transform;
	}

	void FixedUpdate() {
		Quaternion rotation = currentTransform.localRotation;
		if (opened) {
			rotation = Quaternion.Lerp (rotation, Quaternion.Euler(openedAngle), speed);
		} else {
			rotation = Quaternion.Lerp (rotation, Quaternion.Euler(closedAngle), speed);
		}
		currentTransform.localRotation = rotation;
	}

	public void Use() {
		opened = !opened;
	}

}
