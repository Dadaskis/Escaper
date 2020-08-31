using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

class VolumeSliderSaveData {
	public float volume = 0.0f;
}

public class GUIVolumeSlider : MonoBehaviour {

	public Slider slider;

	public void Load() {
		try {
			string json = System.IO.File.ReadAllText("Saves/Sound.settings");
			VolumeSliderSaveData data = JsonConvert.DeserializeObject<VolumeSliderSaveData>(json);
			slider.value = data.volume;
		} catch(System.Exception ex) {
			// ... fuck
		}
	}

	public void Save() {
		try {
			VolumeSliderSaveData save = new VolumeSliderSaveData ();
			save.volume = slider.value;
			string json = JsonConvert.SerializeObject(save);
			System.IO.File.WriteAllText("Saves/Sound.settings", json);
		} catch(System.Exception ex) {
			// What do you mean? What is exception? I dont know what is this either
		}
	}

	public void OnValueChanged() {
		AudioListener.volume = slider.value;
		Save ();
	}

	void Start() {
		Load ();
	}

}
