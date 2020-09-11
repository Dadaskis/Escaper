using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIGamePrestartLaunchLocation : MonoBehaviour {

	public GUIEquipmentAdder equipment;

	public GameObject window;

	public float waitSeconds = 0.0f;

	IEnumerator LaunchTheLocationAfterAwaiting() {
		yield return new WaitForSeconds (waitSeconds);
		PlayerStartData data = new PlayerStartData ();
		data.items = equipment.GetSelectedItemIndexes ();
		if (!GameLogic.LaunchLocation (data)) {
			window.SetActive (true);
		}
	}

	public void LaunchTheLocation() {
		StartCoroutine (LaunchTheLocationAfterAwaiting ());
	}

}
