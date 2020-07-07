using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIQualityGreater : MonoBehaviour {
	public GUIQualityController controller;

	public void OnPress() {
		controller.GreaterQuality ();
	}
}
