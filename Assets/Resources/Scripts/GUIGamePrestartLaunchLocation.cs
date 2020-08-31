using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIGamePrestartLaunchLocation : MonoBehaviour {

	public GUIEquipmentAdder equipment;

	public GameObject window;

	public void LaunchTheLocation() {
		PlayerStartData data = new PlayerStartData ();
		data.items = equipment.GetSelectedItemIndexes ();
		if (!GameLogic.LaunchLocation (data)) {
			window.SetActive (true);
		}
	}

}
