using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapingTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider collider) { 
		Player player = collider.transform.root.GetComponent<Player> ();
		if (player != null) {
			GameLogic.PlayerEscaping ();
		}
	}

}
