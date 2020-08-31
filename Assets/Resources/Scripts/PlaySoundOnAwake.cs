using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnAwake : MonoBehaviour {

	public string soundName;
	public float minDistance = 5.0f;
	public float maxDistance = 20.0f;

	void Start () {
		SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData (soundName);
		data.minDistance = minDistance;
		data.maxDistance = maxDistance;
		SoundManager.instance.CreateSound (data, transform.position, transform);
	}

}
