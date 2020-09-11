using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIGuideUnlock : MonoBehaviour {

	private GUIGuidePanel panel;

	public void Buy() {
		panel = GetComponentInParent<GUIGuidePanel> ();
		LocationStartSettings settings = GameLogic.GetCurrentLocationSettings ();
		if (settings.guidePrice < GameLogic.GetCurrentEXP()) {
			settings.guideUnlocked = true;
			panel.UpdateInside ();
			GameLogic.SetCurrentEXP (GameLogic.GetCurrentEXP () - settings.guidePrice);
		}
	}

}
