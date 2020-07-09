using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIVolumeSlider : MonoBehaviour {

	public Slider slider;

	public void OnValueChanged() {
		AudioListener.volume = slider.value;
	}

}
