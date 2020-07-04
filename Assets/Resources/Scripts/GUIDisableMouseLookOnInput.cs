using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GUIDisableMouseLookOnInput : MonoBehaviour {

	public string inputName = "";
	public FirstPersonController controller;
	public bool isOpened = false;

	public void Switch() {
		isOpened = !isOpened;
		if (controller != null) {
			controller.enableMouseLook = !isOpened;
			controller.mouseLook.SetCursorLock (!isOpened);
		} else {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	void Update() {
		if(InputManager.GetButtonDown(inputName)) {
			Switch ();
		}
	}

}
