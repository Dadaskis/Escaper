using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIQualityLower : MonoBehaviour {
	public GUIQualityController controller;

	public void OnPress() {
		controller.LowerQuality ();
	}
}
