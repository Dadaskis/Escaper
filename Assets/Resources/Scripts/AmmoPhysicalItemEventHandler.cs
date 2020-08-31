using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPhysicalItemEventHandler : MonoBehaviour {

	public int currentAmmo = -1;
	public PhysicalItem item;

	void SetData(GUIItem item, ItemData itemData) {
		AmmoItemData ammoItemData = itemData as AmmoItemData;
		AmmoGUIItemEventHandler weaponUIData = item.GetComponent<AmmoGUIItemEventHandler> ();
		if (currentAmmo == -1) {
			weaponUIData.currentAmmo = ammoItemData.maxAmmo;
		} else {
			weaponUIData.currentAmmo = currentAmmo;
		}
		weaponUIData.UpdateText ();
	}

	void Start() {
		item.customItemSetDataEvent.AddListener (SetData);
	}

}
