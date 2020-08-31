using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPhysicalItemEventHandler : MonoBehaviour {

	public PhysicalItem item;
	public int currentAmmo = 0;

	void SetData(GUIItem item, ItemData itemData) {
		WeaponItemData weaponItemData = itemData as WeaponItemData;
		WeaponGUIItemEventHandler weaponUIData = item.GetComponent<WeaponGUIItemEventHandler> ();
		IWeapon weapon = GetComponent<IWeapon> ();
		if (weapon != null) {
			currentAmmo = Mathf.Max (currentAmmo, weapon.currentAmmo);
		}
		weaponUIData.currentAmmo = currentAmmo;
		weaponUIData.UpdateText ();
	}

	void Start() {
		item.customItemSetDataEvent.AddListener (SetData);
	}

}
