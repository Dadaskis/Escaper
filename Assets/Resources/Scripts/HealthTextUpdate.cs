using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthTextUpdate : MonoBehaviour {
	public Text text;

	void Start() {
		Player.instance.character.onHealthChange.AddListener(OnHealthChange);
		text.text = Player.instance.character.Health.ToString ();
	}

	EventData OnHealthChange(EventData args) {
		int health = args.Get<int> (0);
		text.text = health.ToString ();
		return new EventData ();
	}
}
