using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIOpenInventoryOnInput : MonoBehaviour {

	public GameObject objectToEnable;
	public string inputName;
	public string openMenuInputName;

	void Update () {
		if (InputManager.GetButtonDown (inputName) == true) {
			if (GUIInventories.instance.corpseInventoryOpened) {
				GUIInventories.Close ();
			} else if (GUIInventories.instance.normalInventoryOpened) {
				GUIInventories.Close ();
				//if (GUIInventories.instance.inventoryMouseLookDisabler.isOpened) {
				//	GUIInventories.instance.inventoryMouseLookDisabler.Switch ();
				//}
			} else {
				GUIInventories.OpenNormalInventory ();
			}
		}
		if (InputManager.GetButtonDown (openMenuInputName) == true) {
			if (GUIInventories.instance.corpseInventoryOpened || GUIInventories.instance.normalInventoryOpened) {
				GUIInventories.Close (false);
			}
		}
	}

}
