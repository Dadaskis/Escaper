using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUISetCurrentLocationSettings : MonoBehaviour {

	public string locationName;

	public void SetCurrentSettings() {
		GameLogic.SetCurrentLocationSettings (locationName);
	}

}
