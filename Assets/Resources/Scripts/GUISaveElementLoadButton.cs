using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUISaveElementLoadButton : MonoBehaviour {

	public Text selectedTextUI;
	public Button loadButtonUI;
	public Text saveNameTextUI;
	public GUILoadWindowButton loadWindowButton;

	public void SetSaveToLoadWindow() {
		selectedTextUI.text = "Selected: " + saveNameTextUI.text;
		loadButtonUI.interactable = true;
		loadWindowButton.saveName = saveNameTextUI.text;
	}

}
