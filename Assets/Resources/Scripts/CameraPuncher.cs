using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPuncher : MonoBehaviour {

	public Transform camera;
	public Vector3 punchVector = Vector3.zero;
	public static CameraPuncher instance;

	void Start() {
		instance = this;
	}

	public void Punch(Vector3 power) {
		punchVector += power;
	}

	void Update () {
		punchVector = Vector3.Slerp (punchVector, Vector3.zero, Time.deltaTime * 5.0f);
		camera.localRotation = Quaternion.Slerp(camera.localRotation, Quaternion.Euler (punchVector), Time.deltaTime * 10.0f);
	}
}
