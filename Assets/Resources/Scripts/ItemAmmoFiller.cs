using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAmmoFiller : IAmmoFiller {

	public override int Reload (IWeapon weapon) {
		int fulfillAmmo = weapon.maxAmmo - weapon.currentAmmo;
		bool foundNeededItem = false;
		foreach (GUIItem item in Player.instance.inventory.items) {
			if (item == null) {
				continue;
			}
			if ((weapon.currentAmmo + fulfillAmmo) >= weapon.maxAmmo && foundNeededItem) {
				break;
			}
			AmmoItemData data = item.physicalData as AmmoItemData;
			if (data != null) {
				if (data.ammoType == weapon.ammoType) {
					AmmoGUIItemEventHandler handler = item.GetComponent<AmmoGUIItemEventHandler> ();
					if (handler != null && handler.currentAmmo > 0) {
						handler.currentAmmo -= fulfillAmmo;
						if (handler.currentAmmo < 0) {
							fulfillAmmo += handler.currentAmmo;
							handler.currentAmmo = 0;
						}
						handler.UpdateText ();
						foundNeededItem = true;
					}
				}
			}
		}
		if (!foundNeededItem) {
			return weapon.currentAmmo;
		}
		return weapon.currentAmmo + fulfillAmmo;
	}

}
