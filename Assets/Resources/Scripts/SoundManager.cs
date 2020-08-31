using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

[System.Serializable]
public class SoundType {
	public string typeName = "";
	public float volumeMultiplier = 1.0f;
}

[System.Serializable]
public class SoundMaterialType {
	public string typeName = "";
	public List<string> hitClipNames = new List<string> ();
	public List<string> walkClipNames = new List<string> ();
	public List<string> runClipNames = new List<string> ();
	public List<string> landingClipNames = new List<string> ();
}

public class SoundManager : MonoBehaviour {

	public static SoundManager instance;

	void Awake() {
		InitializeClips ();
		instance = this;

		try {
			string json = System.IO.File.ReadAllText("Saves/Sound.settings");
			VolumeSliderSaveData data = JsonConvert.DeserializeObject<VolumeSliderSaveData>(json);
			AudioListener.volume = data.volume;
		} catch(System.Exception ex) {
			// ... fuck
		}
	}

	public Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();
	public Dictionary<string, float> clipVolumes = new Dictionary<string, float>();
	public Dictionary<string, SoundMaterialType> materialClips = new Dictionary<string, SoundMaterialType>();
	public GameObject soundObjectPrefab;
	public List<SoundType> types = new List<SoundType>();
	public List<SoundMaterialType> materialTypes = new List<SoundMaterialType>();

	void InitializeClips() {
		AudioClip[] objects = Resources.LoadAll ("", typeof(AudioClip)).Cast<AudioClip> ().ToArray ();
		foreach (AudioClip obj in objects) {
			clips [obj.name] = obj;
			Debug.Log ("[SoundManager] Loaded sound: " + obj.name);
		}
		InitializeClipVolumes ();
		InitializeMaterialClips ();
	}

	void InitializeClipVolumes() {
		foreach (KeyValuePair<string, AudioClip> clip in clips) {
			AudioClip obj = clip.Value;
			float volume = 1.0f;
			foreach (SoundType type in types) {
				if (obj.name.Contains (type.typeName)) {
					volume *= type.volumeMultiplier;
				}
			}
			clipVolumes [obj.name] = volume;
		}
	}

	void InitializeMaterialClips() {
		Material[] objects = Resources.LoadAll ("", typeof(Material)).Cast<Material>().ToArray();
		foreach (Material material in objects) {	
			foreach (SoundMaterialType type in materialTypes) {
				if (material.name.Contains (type.typeName)) {
					materialClips [material.name] = type;
					break;
				}
			}
		}
	}

	public SoundObjectData GetBasicSoundObjectData(string soundName) {
		SoundObjectData data = new SoundObjectData ();
		AudioClip clip;
		if (clips.TryGetValue (soundName, out clip)) {
			data.clip = clip;
			data.volume = clipVolumes [soundName];
		}
		return data;
	}

	public SoundMaterialType GetSoundMaterialType(string material) {
		SoundMaterialType type;
		if (materialClips.TryGetValue (material, out type)) {
			return type;
		} 
		Debug.LogError ("[SoundManager] Cant get sound material type: " + material);
		return null;
	}

	public SoundMaterialType GetSoundMaterialType(Material material) {
		return GetSoundMaterialType (material.name);
	}

	public void CreateSound(SoundObjectData soundData, Vector3 position = default(Vector3), Transform parent = null) {
		if (soundData.clip == null) {
			return;
		}

		GameObject soundObject = Instantiate (soundObjectPrefab);
		SoundObject sound = soundObject.GetComponent<SoundObject> ();
		sound.SetClip (soundData);
		soundObject.transform.position = position;
		soundObject.transform.SetParent (parent);
	}

}
