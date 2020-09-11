using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIStartPriceUpdater : MonoBehaviour {

	public GUIEquipmentAdder equipment;
	public Text text;
	public int zeroCount = 4;

	public void UpdateText() {
		PlayerStartData startData = new PlayerStartData ();
		startData.items = equipment.GetSelectedItemIndexes ();
		int price = GameLogic.GetStartPrice (startData);
		string number = price.ToString ();
		if (number.Length < zeroCount) {
			int length = zeroCount - number.Length;
			for (int counter = 0; counter < length; counter++) {
				number = "0" + number;
			}
		}
		text.text = "Start price: " + number + " EXP";
	}

	EventData UpdateOnEvent(EventData data) {
		UpdateText ();
		return new EventData ();
	}

	void OnEnable() {
		UpdateText ();
	}

	void Start() {
		EventManager.AddEventListener<Events.GUIItem.Selected> (UpdateOnEvent);
		EventManager.AddEventListener<Events.GameLogic.BoostPointChanged> (UpdateOnEvent);
	}

}
