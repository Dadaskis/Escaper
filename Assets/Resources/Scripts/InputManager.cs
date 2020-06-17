using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

public class InputManager : MonoBehaviour {

	public class KeyPressedEvent : UnityEvent<KeyCode> {}
	public KeyPressedEvent onKeyPressed = new KeyPressedEvent();

	public InputData data;

	public static InputManager instance;

	public InputManagerDictionary keys = new InputManagerDictionary ();
	public Dictionary<string, string> normalNames = new Dictionary<string, string>();

	void Awake() {
		instance = this;
		foreach (InputKey key in data.keys) {
			keys.Add (key.name, key.key);
			normalNames.Add (key.name, key.normalName);
		}
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

	public static bool GetButton(string name) {
		if (instance != null) {
			return Input.GetKey (instance.keys [name]);
		}
		return false;
	}

}
