using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIPlaySound : MonoBehaviour {

	public string soundName;

	public void Play() {
		SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData (soundName);
		data.spatialBlend = 0.0f;
		SoundManager.instance.CreateSound (data);
	}

}
