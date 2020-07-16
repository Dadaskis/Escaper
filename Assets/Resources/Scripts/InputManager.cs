using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.Characters.FirstPerson;
using Newtonsoft.Json;

[System.Serializable]
public class InputManagerDictionary : Dictionary<string, KeyCode> {

	public SerializableKeyValues<string, int> GetSerializableVariant() {
		SerializableDictionary<string, int> data = new SerializableDictionary<string, int> ();

		foreach (KeyValuePair<string, KeyCode> pair in this) {
			data.Add (pair.Key, (int)pair.Value);
		}

		return data.GetSerializableKeyValues();
	}

	public void SetFromSerializableVariant(SerializableKeyValues<string, int> data){
		this.Clear ();
		for(int index = 0; index < data.keys.Count; index++){
			this.Add (data.keys [index], (KeyCode)data.values [index]);
		}
	}

}

[System.Serializable]
public class InputSave {
	public SerializableKeyValues<string, int> keys;
	public float mouseSensitivity;
}

public class InputManager : MonoBehaviour {

	public class KeyPressedEvent : UnityEvent<KeyCode> {}
	public KeyPressedEvent onKeyPressed = new KeyPressedEvent();

	public InputData data;

	public static InputManager instance;

	public InputManagerDictionary keys = new InputManagerDictionary ();
	public Dictionary<string, string> normalNames = new Dictionary<string, string>();
	public float mouseSensitivity = 2.0f;


	public void Load() {
		try {
			string json = System.IO.File.ReadAllText("Saves/Input.settings");
			InputSave data = JsonConvert.DeserializeObject<InputSave>(json);
			keys.SetFromSerializableVariant(data.keys);
			mouseSensitivity = data.mouseSensitivity;
		} catch(System.Exception ex) {
			// ... fuck
		}
	}

	public void Save() {
		try {
			InputSave save = new InputSave();
			save.keys = keys.GetSerializableVariant();
			save.mouseSensitivity = mouseSensitivity;
			string json = JsonConvert.SerializeObject(save);
			System.IO.File.WriteAllText("Saves/Input.settings", json);
		} catch(System.Exception ex) {
			// What do you mean? What is exception? I dont know what is this either
		}
	}

	void Awake() {
		instance = this;
		foreach (InputKey key in data.keys) {
			keys.Add (key.name, key.key);
			normalNames.Add (key.name, key.normalName);
		}
		Load ();
	}
		
	void CheckKeys() {
		foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode))) {
			if (Input.GetKey (key)) {
				onKeyPressed.Invoke (key);
			}
		}
	}

	void Update() {
		CheckKeys ();
	}

	public static bool GetButtonDown(string name) {
		return Input.GetKeyDown (instance.keys [name]);
	}

	public static bool GetButtonUp(string name) {
		return Input.GetKeyUp (instance.keys [name]);
	}

	public static bool GetButton(string name) {
		if (instance != null) {
			return Input.GetKey (instance.keys [name]);
		}
		return false;
	}

	public void SetMouseSensitivity(float sensitivity) {
		mouseSensitivity = sensitivity;
		MouseLook mouseLook = Player.instance.controller.mouseLook;
		mouseLook.XSensitivity = sensitivity;
		mouseLook.YSensitivity = sensitivity;
		Player.instance.controller.mouseLook = mouseLook;
	}

}
