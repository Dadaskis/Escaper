using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestroyer : MonoBehaviour {

	public GameObject eventSystem;

	void Awake () {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Player player = FindObjectOfType<Player> ();
		if (player != null) {
			Destroy (player.gameObject);
			//Instantiate (eventSystem);
			Destroy (gameObject);
		}
	}

}
