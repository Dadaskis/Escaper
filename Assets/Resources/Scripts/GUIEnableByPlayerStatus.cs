using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIEnableByPlayerStatus : MonoBehaviour {

	public GameObject escaping;
	public GameObject dead;

	void Start() {
		if (GameLogic.GetPlayerStatus () == PlayerStatus.DEAD) {
			dead.SetActive (true);
		} else if (GameLogic.GetPlayerStatus () == PlayerStatus.ESCAPING) {
			escaping.SetActive (true);
		}
	}

}
