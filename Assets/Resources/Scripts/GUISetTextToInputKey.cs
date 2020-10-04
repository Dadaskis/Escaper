using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUISetTextToInputKey : MonoBehaviour {

	public Text text;
	public string input;

	string ConvertInputKey(string input) {
		return System.Enum.GetName (typeof(KeyCode), InputManager.instance.keys [input]);
	}

	void Start () {
		text.text = "[" + ConvertInputKey (input) + "]";
	}
}
