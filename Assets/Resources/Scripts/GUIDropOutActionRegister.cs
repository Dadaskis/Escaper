using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIDropOutActionRegister : MonoBehaviour {
	public static GUIDropOutActionRegister instance;
	public int count = 0;
	public Dictionary<string, GUIItemActionBase> actions = new Dictionary<string, GUIItemActionBase>();

	void Awake () {
		instance = this;
	}

	void Update() {
		count = actions.Count;
	}

	public static GUIItemActionBase Get(string name) {
		//return instance.actions [name];
		GUIItemActionBase action;
		if (instance.actions.TryGetValue (name, out action)) {
			return action;
		}
		Debug.LogError ("[GUIDropOutActionRegister] Cant get action: " + name);
		return null;
	}
}
