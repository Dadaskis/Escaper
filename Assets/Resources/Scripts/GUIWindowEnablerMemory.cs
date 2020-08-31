using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIWindowEnablerMemory : MonoBehaviour {
	public GameObject lastEnabled = null;

	public void Enable(GameObject obj) {
		if(lastEnabled != null) {
			lastEnabled.SetActive (false);
		}
		lastEnabled = obj;
		lastEnabled.SetActive (true);
	}
}
