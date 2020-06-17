using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIEnableOnKeyPress : MonoBehaviour {

	public GameObject objectToEnable;
	public string inputName;

	void Update () {
		if (InputManager.GetButtonDown (inputName) == true) {
			objectToEnable.SetActive (!objectToEnable.activeInHierarchy);
		}
	}
}
