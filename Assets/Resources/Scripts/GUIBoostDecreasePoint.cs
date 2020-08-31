using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIBoostDecreasePoint : MonoBehaviour {

	public string boostName;

	public void OnPress() {
		GameLogic.DecreaseBoostPoint (boostName);
	}

}
