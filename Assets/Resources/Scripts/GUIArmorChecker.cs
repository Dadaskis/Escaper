using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIArmorChecker : MonoBehaviour {

	public GameObject textToDisable;
	public GameObject iconToDisable;
	public Text text;

	void Start(){
		Player.instance.character.onArmorChange.AddListener (ArmorChanged);
		textToDisable.SetActive (false);
		iconToDisable.SetActive (false);
	}

	void ArmorChanged(int armor){
		if (armor <= 0) {
			textToDisable.SetActive (false);
			iconToDisable.SetActive (false);
		} else {
			textToDisable.SetActive (true);
			iconToDisable.SetActive (true);
			text.text = "" + armor;
		}
	}
}
