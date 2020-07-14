using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothInfrontPlacer : MonoBehaviour {
	public Transform target;
	public float interpretationSpeed = 10.0f;
	private Transform myTransform;

	void Start() {
		myTransform = transform;
	}

	void Update () {
		myTransform.position = Vector3.Slerp(myTransform.position, target.forward, Time.deltaTime * interpretationSpeed);
	}
}
