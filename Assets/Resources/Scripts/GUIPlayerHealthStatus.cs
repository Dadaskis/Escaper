using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlayerHealthStatus : MonoBehaviour {

	public Image image;

	public Color fullHealthColor;
	public Color slightlyDamagedHealthColor;
	public Color damagedHealthColor;
	public Color noHealthColor;

	public float slightlyDamaged = 0.9f;
	public float damaged = 0.6f;
	public float noHealth = 0.2f;

	public bool criticalHealthLevel = false;
	
	EventData OnHealthChanged(EventData data) {
		Character character = data.Get<Character> (0);
		if (character == Player.instance.character) {
			float procent = ((float)character.Health) / ((float)character.MaxHealth);
			if (procent > slightlyDamaged) {
				image.color = fullHealthColor;
				criticalHealthLevel = false;
			} else if (procent < slightlyDamaged && procent > damaged) {
				image.color = slightlyDamagedHealthColor;
				criticalHealthLevel = false;
			} else if (procent < damaged && procent > noHealth) {
				image.color = damagedHealthColor;
				criticalHealthLevel = false;
			} else if (procent < noHealth) {
				image.color = noHealthColor;
				criticalHealthLevel = true;
			}
		}

		return new EventData ();
	}

	void Update() {
		if (criticalHealthLevel) {
			image.color = Color.Lerp (noHealthColor, damagedHealthColor, Mathf.Clamp01(0.5f + Mathf.Sin (Time.time * 3.0f) * 0.5f));
		}
	}

	void Start () {
		EventManager.AddEventListener<Events.Character.HealthChanged> (OnHealthChanged);
	}

}
