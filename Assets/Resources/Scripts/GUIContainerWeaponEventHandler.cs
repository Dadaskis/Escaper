using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIContainerWeaponEventHandler : MonoBehaviour {

	public GUIContainer container;
	public WeaponGUIItemEventHandler handler;
	public string weaponClass;
	public int weaponSlot;

	EventData SlotAtPrimaryFire(EventData data) {
		if (data.Get<int> (0) == weaponSlot) {
			IWeapon weapon = data.Get<IWeapon> (1);
			//handler.item.additionalDataText.text = weapon.currentAmmo + "/" + weapon.maxAmmo;
			handler.currentAmmo = weapon.currentAmmo;
			handler.UpdateText();
		}
		return new EventData ();
	}

	EventData SlotAtReload(EventData data) {
		if (data.Get<int> (0) == weaponSlot) {
			IWeapon weapon = data.Get<IWeapon> (1);
			handler.currentAmmo = weapon.currentAmmo;
			handler.UpdateText();
		}
		return new EventData ();
	}

	void SetItemEvent(GUIItem item) {
		if (item != null) {
			WeaponItemData data = item.physicalData as WeaponItemData;
			weaponClass = data.weaponClass;
			handler = item.GetComponent<WeaponGUIItemEventHandler> ();
		} else {
			weaponClass = "";
		}
		Player.instance.weaponSlots.SetWeaponInSlot (weaponClass, weaponSlot, handler.currentAmmo);
	}

	void Start() {
		container.setItemEvent.AddListener (SetItemEvent);
		EventManager.AddEventListener<Events.PlayerWeaponSlots.SlotAtPrimaryFire> (SlotAtPrimaryFire);
		EventManager.AddEventListener<Events.PlayerWeaponSlots.SlotAtReload> (SlotAtReload);
	}

}
