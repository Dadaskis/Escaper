using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUISetTextCurrentEXP : MonoBehaviour {

	public Text text;
	public int zeroCount = 4;

	public void UpdateText() {
		string number = GameLogic.instance.currentEXP.ToString ();
		if (number.Length < zeroCount) {
			int length = zeroCount - number.Length;
			for (int counter = 0; counter < length; counter++) {
				number = "0" + number;
			}
		}
		text.text = number + " EXP";
	}

	void OnEnable() {
		UpdateText ();
	}

	void Start() {
		UpdateText ();
	}

}
