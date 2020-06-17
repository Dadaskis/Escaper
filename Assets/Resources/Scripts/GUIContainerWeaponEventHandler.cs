using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIContainerWeaponEventHandler : MonoBehaviour {

	public GUIContainer container;
	public WeaponGUIItemEventHandler handler;
	public string weaponClass;
	public int weaponSlot;

	void SetItemEvent(GUIItem item) {
		if (item != null) {
			handler = item.GetComponent<WeaponGUIItemEventHandler> ();
			weaponClass = handler.weaponClass;
		} else {
			weaponClass = "";
		}
		Player.instance.weaponSlots.SetWeaponInSlot (weaponClass, weaponSlot);
	}

	void Start() {
		container.setItemEvent.AddListener (SetItemEvent);
	}

}
