using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIDisableOnInput : MonoBehaviour {
	public string inputName;
	public GameObject objectToDisable;

	void Update () {
		if (InputManager.GetButtonDown (inputName)) {
			objectToDisable.SetActive (false);
		}
	}
}
