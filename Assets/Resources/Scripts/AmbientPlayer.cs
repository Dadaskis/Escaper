using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientPlayer : MonoBehaviour {
	private SoundManager soundManager = null;
	public float delay = 0.0f;
	private float timer = 0.0f;
	public List<string> soundNames;

	void Start() {
		soundManager = SoundManager.instance;
	}

	SoundObjectData GetRandomSound(){
		string name = soundNames [Random.Range (0, soundNames.Count - 1)];
		SoundObjectData data = soundManager.GetBasicSoundObjectData(name);
		return data;
	}

	void PlayRandomAmbientSound() {
		SoundObjectData data = GetRandomSound ();
		timer -= data.clip.length;
		data.spatialBlend = 0.0f;
		soundManager.CreateSound (data);
	}

	void Update() {
		if(timer > delay) {
			PlayRandomAmbientSound ();
		}
		timer += Time.deltaTime;
	}
}
