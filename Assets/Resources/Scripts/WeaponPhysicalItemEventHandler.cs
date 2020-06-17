using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPhysicalItemEventHandler : MonoBehaviour {

	public PhysicalItem item;

	void SetData(GUIItem item, ItemData itemData) {
		WeaponItemData weaponItemData = itemData as WeaponItemData;
		WeaponGUIItemEventHandler weaponUIData = item.GetComponent<WeaponGUIItemEventHandler> ();
		weaponUIData.weaponClass = weaponItemData.weaponClass;
	}

	void Start() {
		item.customItemSetDataEvent.AddListener (SetData);
	}

}
