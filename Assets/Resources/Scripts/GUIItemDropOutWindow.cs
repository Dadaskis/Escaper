using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GUIItemDropOutWindow : MonoBehaviour {
	public RectTransform rectTransform;
	public GUIItemDropOutButton button;

	void SelfDisable() {
		gameObject.SetActive (false);
	}

	public void CreateNewButton(string name, UnityAction callback) {
		GameObject newButton = Instantiate (button.gameObject, transform);
		GUIItemDropOutButton buttonData = newButton.GetComponent<GUIItemDropOutButton> ();
		buttonData.RegisterCallback (callback);
		buttonData.RegisterCallback (SelfDisable);
		buttonData.text.text = name;
	}

	public void Work(GUIItemActionBase action, GUIItem item) {
		for (int index = 0; index < transform.childCount; index++) {
			Destroy(transform.GetChild (index).gameObject);
		}
		action.Apply (this, item);
	}
}
