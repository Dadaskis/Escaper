using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIDisableIfLocationLocked : MonoBehaviour {

	public string locationName = "";

	void Check() {
		if (!GameLogic.instance.locationStartSettings [locationName].unlocked) {
			gameObject.SetActive (false);
		}
	}

	void Start () {
		Check ();	
	}

	void OnEnable() {
		Check ();
	}
}
