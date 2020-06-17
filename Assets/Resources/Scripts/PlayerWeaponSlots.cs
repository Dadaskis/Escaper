using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public List<string> slots = new List<string>(10);

	public int currentSlot = -1;

	public void SetWeaponInSlot(string weapon, int slot) {
		if (slot >= 0 && slot <= 10) {
			slots [slot] = weapon;
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
				player.weapon.SetWeapon (GetWeaponInSlot (slot));
				currentSlot = slot;
			}
		} else {
			Debug.LogError ("[PlayerWeaponSlots] Cant take weapon because its less than 0 or greater than 10: " + slot);
		}
	}

	public string GetWeaponInSlot(int slot) {
		if (slot >= 0 && slot <= 10) {
			return slots [slot];
		} else {
			Debug.LogError ("[PlayerWeaponSlots] Cant get weapon in slot because its less than 0 or greater than 10: " + slot);
		}
		return "";
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
