using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlayerHealthValue : MonoBehaviour {

	public Text text;

	EventData OnHealthChanged(EventData data) {
		Character character = data.Get<Character> (0);
		if (character == Player.instance.character) {
			text.text = character.Health + "/" + character.MaxHealth;
		}

		return new EventData ();
	}

	void Start () {
		EventManager.AddEventListener<Events.Character.HealthChanged> (OnHealthChanged);
	}

}
