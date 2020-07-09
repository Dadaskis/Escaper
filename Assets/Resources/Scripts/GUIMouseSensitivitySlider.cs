using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIMouseSensitivitySlider : MonoBehaviour {

	public Slider slider;

	void Start() {
		slider.value = InputManager.instance.mouseSensitivity;
	}

	public void OnValueChanged() {
		InputManager.instance.SetMouseSensitivity (slider.value);
		InputManager.instance.Save ();
	}

}
