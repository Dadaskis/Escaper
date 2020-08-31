using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InSightDisabler : MonoBehaviour {
	public Image image;
	public static InSightDisabler instance;

	void Awake() {
		instance = this;
	}

	void Update () {
		if(Input.GetMouseButtonDown (1)) {
			image.enabled = !image.enabled; 
		}
	}
}
