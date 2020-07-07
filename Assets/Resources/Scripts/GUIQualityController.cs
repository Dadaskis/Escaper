using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GUIQualityController : MonoBehaviour {

	public int quality = 0;
	public List<string> qualityNaming = new List<string>();
	public Text textUI;

	public void GreaterQuality() {
		if (quality < qualityNaming.Count - 1) {
			quality++;
		} else {
			quality = 0;
		}
		UpdateText ();
	}

	public void LowerQuality() {
		if (quality > 0) {
			quality--;
		} else {
			quality = qualityNaming.Count - 1;
		}
		UpdateText ();
	}

	public string GetNaming() {
		return qualityNaming[quality];
	}

	public void UpdateText() {
		textUI.text = GetNaming ();
	}

}
