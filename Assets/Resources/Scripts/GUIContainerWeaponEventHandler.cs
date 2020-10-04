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
			handler.currentDurability = weapon.currentDurability;
			handler.UpdateText();
		}
		return new EventData ();
	}

	IEnumerator CheckAmmoUpdateAndStop(IWeapon weapon) {
		int ammo = weapon.currentAmmo;
		while (ammo == weapon.currentAmmo) {
			yield return new WaitForSeconds (0.3f);
		}
		handler.currentAmmo = weapon.currentAmmo;
		handler.currentDurability = weapon.currentDurability;
		handler.UpdateText();
	}

	EventData SlotAtReload(EventData data) {
		if (data.Get<int> (0) == weaponSlot) {
			IWeapon weapon = data.Get<IWeapon> (1);
			StartCoroutine (CheckAmmoUpdateAndStop (weapon));
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

	EventData OnItemDrop(EventData data) {
		GUIItem item = data.Get<GUIItem> (0);
		if (container.contains == item) {
			if (Player.instance.weaponSlots.currentSlot == weaponSlot) {
				Player.instance.weaponSlots.TakeWeapon (weaponSlot);
			}
			Player.instance.weaponSlots.SetWeaponInSlot ("", weaponSlot, 0);
		}

		return new EventData ();
	}

	void Start() {
		container.setItemEvent.AddListener (SetItemEvent);
		EventManager.AddEventListener<Events.PlayerWeaponSlots.SlotAtPrimaryFire> (SlotAtPrimaryFire);
		EventManager.AddEventListener<Events.PlayerWeaponSlots.SlotAtReload> (SlotAtReload);
		EventManager.AddEventListener<Events.GUIItemActionSimple.Drop> (OnItemDrop);
	}

}
