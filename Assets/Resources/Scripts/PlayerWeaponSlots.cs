using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events.PlayerWeaponSlots {
	class SlotAtPrimaryFire {}
	class SlotAtReload {}
}

public class WeaponSlot {
	public string weaponClass = "";
	public int ammo = 0;

	public WeaponSlot(string weaponClass, int ammo) {
		this.weaponClass = weaponClass;
		this.ammo = ammo;
	}
}

public class PlayerWeaponSlots : MonoBehaviour {

	public Character player;

	public List<KeyCode> bindableKeys = new List<KeyCode> () {
		KeyCode.Alpha0,
		KeyCode.Alpha1,
		KeyCode.Alpha2,
		KeyCode.Alpha3,
		KeyCode.Alpha4,
		KeyCode.Alpha5,
		KeyCode.Alpha6,
		KeyCode.Alpha7,
		KeyCode.Alpha8,
		KeyCode.Alpha9
	};

	public List<WeaponSlot> slots;

	public int currentSlot = -1;

	public void SetWeaponInSlot(string weapon, int slot, int ammo = 0) {
		if (slot >= 0 && slot <= 10) {
			slots [slot].weaponClass = weapon;
			slots [slot].ammo = ammo;
		} else {
			Debug.LogError ("[PlayerWeaponSlots] Cant set weapon in slot because its less than 0 or greater than 10: " + slot);
		}
	}

	public void TakeWeapon(int slot) {
		if (slot >= 0 && slot <= 10) {
			if (slot == currentSlot) {
				player.weapon.SetWeapon ("");
				currentSlot = -1;
			} else {
				player.weapon.weaponClass = GetWeaponInSlot (slot).weaponClass;
				player.weapon.slot = slot;
				player.weapon.currentAmmo = GetWeaponInSlot (slot).ammo;
				player.weapon.SetWeapon (GetWeaponInSlot (slot).weaponClass);
				currentSlot = slot;
			}
		} else {
			Debug.LogError ("[PlayerWeaponSlots] Cant take weapon because its less than 0 or greater than 10: " + slot);
		}
	}

	public WeaponSlot GetWeaponInSlot(int slot) {
		if (slot >= 0 && slot <= 10) {
			return slots [slot];
		} else {
			Debug.LogError ("[PlayerWeaponSlots] Cant get weapon in slot because its less than 0 or greater than 10: " + slot);
		}
		return null;
	}

	EventData IWeaponPrimaryFire(EventData data) {
		IWeapon weapon = data.Get<IWeapon> (0);
		if (weapon.firstPerson) {
			GetWeaponInSlot (weapon.slot).ammo = weapon.currentAmmo;
			EventManager.RunEventListeners<Events.PlayerWeaponSlots.SlotAtPrimaryFire> (weapon.slot, weapon);
		}
		return new EventData ();
	}

	EventData IWeaponReload(EventData data) {
		IWeapon weapon = data.Get<IWeapon> (0);
		if (weapon.firstPerson) {
			GetWeaponInSlot (weapon.slot).ammo = weapon.currentAmmo;
			EventManager.RunEventListeners<Events.PlayerWeaponSlots.SlotAtReload> (weapon.slot, weapon);
		}
		return new EventData ();
	}

	void Start() {
		slots = new List<WeaponSlot> ();
		for (int counter = 0; counter < 10; counter++) {
			slots.Add (new WeaponSlot ("", 0));
		}
		EventManager.AddEventListener<Events.IWeapon.PrimaryFire> (IWeaponPrimaryFire);
		EventManager.AddEventListener<Events.IWeapon.Reload> (IWeaponReload);
	}

	void Update() {
		for (int index = 0; index < bindableKeys.Count; index++) {
			KeyCode code = bindableKeys [index];
			if (Input.GetKeyDown (code)) {
				TakeWeapon (index);
			}
		}
	}

}
