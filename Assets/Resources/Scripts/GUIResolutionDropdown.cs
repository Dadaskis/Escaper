using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIResolutionDropdown : MonoBehaviour {

	public Dropdown dropdown;
	private Dictionary<string, bool> addedResolution = new Dictionary<string, bool> ();
	private List<Resolution> resolutions = new List<Resolution>();

	void Start() {
		foreach (Resolution resolution in Screen.resolutions) {
			bool added;
			if (!addedResolution.TryGetValue (resolution.ToString (), out added)) {
				dropdown.AddOptions (new List<string> () { resolution.ToString () });
				addedResolution [resolution.ToString ()] = true;
				resolutions.Add(resolution);
			}
		}
		dropdown.captionText.text = Screen.width + " x " + Screen.height + " @ 59Hz";
	}

	public void Change() {
		Resolution resolution = resolutions[dropdown.value];
		Screen.SetResolution (resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
	}

}

