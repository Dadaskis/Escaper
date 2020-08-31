using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIBoostAddPoint : MonoBehaviour {

	public string boostName;

	public void OnPress() {
		GameLogic.AddBoostPoint (boostName);
	}

}
