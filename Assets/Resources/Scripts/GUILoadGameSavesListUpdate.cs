using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUILoadGameSavesListUpdate : MonoBehaviour {

	public Transform saveList;
	public GameObject saveElementLoadButton;
	public Text selectedTextUI;
	public Button loadButtonUI;
	public GUILoadWindowButton loadWindowButton;

	public void UpdateList() {
		foreach (KeyValuePair<string, SaveData> pair in Serializer.instance.saves) {
			GameObject saveElementLoadButtonClone = Instantiate (saveElementLoadButton, saveList);
			GUISaveElementLoadButton data = saveElementLoadButtonClone.GetComponent<GUISaveElementLoadButton> ();
			data.selectedTextUI = selectedTextUI;
			data.loadButtonUI = loadButtonUI;
			data.saveNameTextUI.text = pair.Key;
			data.loadWindowButton = loadWindowButton;
		}
	}

}
