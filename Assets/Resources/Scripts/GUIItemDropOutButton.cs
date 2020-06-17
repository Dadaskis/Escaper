using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GUIItemDropOutButton : MonoBehaviour {

	public Text text;
	public Button button;

	public void RegisterCallback(UnityAction action) {
		button.onClick.AddListener (action);
	}

}
