using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singletons : MonoBehaviour {

	private bool isChecked = false;

	void Awake () {
		if (!isChecked) {
			Singletons[] singletonsList = FindObjectsOfType<Singletons> ();
			foreach (Singletons singletons in singletonsList) {
				if (singletons != null && singletons.gameObject != gameObject) {
					Destroy (gameObject);
					return;
				} 
			}
			isChecked = true;
			DontDestroyOnLoad (gameObject);
		}
	}

}
