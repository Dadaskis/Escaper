using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIActiveWhenEscapingStatus : MonoBehaviour {
	
	void Start () {
		if (GameLogic.GetPlayerStatus () != PlayerStatus.ESCAPING) {
			gameObject.SetActive (false);
		}

		if (GameLogic.GetCurrentLocationSettings ().nextLocationName.Length < 1) {
			gameObject.SetActive (false);
		}
	}

}
