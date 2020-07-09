using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIKeyControl : MonoBehaviour {

	public GUIControlsTab controls;
	public string attachedInput;
	public Text textUI;
	public Text nameUI;

	public void OnPress() {
		controls.ListenButton (this);
	}

	public void UpdateText() {
		textUI.text = System.Enum.GetName (typeof(KeyCode), InputManager.instance.keys[attachedInput]);
		nameUI.text = InputManager.instance.normalNames [attachedInput];
	}

	public void AttachInput(string input = null) {
		if (input == null) {
			input = attachedInput;
		} else {
			attachedInput = input;
		}
		UpdateText ();
	}

	public void ChangeInputKey(KeyCode code) {
		InputManager.instance.keys [attachedInput] = code;
		UpdateText ();
		InputManager.instance.Save ();
	}

}
