using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIControlsTab : MonoBehaviour {

	public Transform keysContentUI;
	public GameObject keyContolUIPrefab;

	private bool isListeningButton = false;
	private GUIKeyControl keyControl = null;

	void OnKeyPressed(KeyCode code) {
		if (isListeningButton) {
			keyControl.ChangeInputKey (code);
			isListeningButton = false;
			keyControl = null;
		}
	}

	public void ListenButton(GUIKeyControl key) {
		keyControl = key;
		keyControl.textUI.text = "---";
		isListeningButton = true;
	}

	void Start() {
		InputManager.instance.onKeyPressed.AddListener (OnKeyPressed);

		InputData data = InputManager.instance.data;
		foreach (InputKey key in data.keys) {
			GameObject keyControlUI = Instantiate (keyContolUIPrefab, keysContentUI);
			GUIKeyControl control = keyControlUI.GetComponentInChildren<GUIKeyControl> ();
			control.controls = this;
			control.AttachInput (key.name);
		}
	}

}
