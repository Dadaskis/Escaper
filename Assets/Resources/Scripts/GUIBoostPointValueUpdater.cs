using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIBoostPointValueUpdater : MonoBehaviour {

	public Text text;
	public string boostName;

	public void UpdateText() {
		int points = GameLogic.GetBoostPoints (boostName);
		int maximum = GameLogic.GetBoostPointsMaximum (boostName);
		text.text = points + "/" + maximum;
	}

	void OnEnable() {
		UpdateText ();
	}

	EventData UpdateOnEvent(EventData eventData) {
		UpdateText ();
		return new EventData();
	}

	void Start() {
		EventManager.AddEventListener<Events.GameLogic.BoostPointChanged> (UpdateOnEvent);
	}

}
