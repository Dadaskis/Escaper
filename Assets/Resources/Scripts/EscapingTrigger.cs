using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapingTrigger : MonoBehaviour {

	private bool escaped = false;

	void OnTriggerEnter(Collider collider) { 
		if (escaped) {
			return;
		}
		Player player = collider.transform.root.GetComponent<Player> ();
		if (player != null) {
			GameLogic.PlayerEscaping ();
			escaped = true;
		}
	}

}
