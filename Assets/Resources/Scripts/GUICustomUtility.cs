﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class GUICustomUtility {

	public static List<RaycastResult> RaycastMouse() {
		PointerEventData pointerData = new PointerEventData(EventSystem.current) {
			pointerId = -1
		};
		pointerData.position = Input.mousePosition;

		List<RaycastResult> results = new List<RaycastResult> ();
		EventSystem.current.RaycastAll (pointerData, results);

		return results;
	}

}
