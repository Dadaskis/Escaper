using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUITabMemory : MonoBehaviour {
	
	public GameObject previousTab;

	public void Change(GameObject tabElement) {
		if (previousTab != null) {
			previousTab.SetActive (false);
		}
		tabElement.SetActive (true);
		previousTab = tabElement;
	}

}
