using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonWeaponChanger : IWeaponChanger {

	public Transform weaponHolder;
	public Character owner;
	public bool restrictChanging = false;
	public bool changing = false;
	[HideInInspector] public IWeapon weapon;

	IEnumerator WeaponSetting(float seconds) {
		changing = true;
		yield return new WaitForSeconds (seconds);
		yield return new WaitForEndOfFrame ();
		for(int index = 0; index < weaponHolder.childCount; index++) {
			Destroy (weaponHolder.GetChild (index).gameObject);
		}
		if (weaponClass.Length == 0) {
			weapon = null;
			weaponClass = "";
		} else {
			WeaponData data = WeaponManager.GetWeaponData (weaponClass);
			GameObject obj = Instantiate (data.firstPerson, weaponHolder);
			weapon = obj.GetComponentInChildren<IWeapon> ();
			weapon.firstPerson = true;
			weapon.owner = owner;
			weapon.weaponClass = weaponClass;
			weapon.slot = slot;
			weapon.currentAmmo = currentAmmo;
			this.weaponClass = weaponClass;
		}
		changing = false;
	}

	public override float SetWeapon (string weaponClass) {
		if (changing) {
			return -1.0f;
		}
		if (restrictChanging) {
			return -1.0f;
		}
		float seconds = 0.1f; 
		if (weapon != null) { 
			seconds = weapon.Takeout ();
			if (seconds == -1.0f) {
				return -1.0f;
			}
		}
		this.weaponClass = weaponClass;
		StartCoroutine (WeaponSetting (seconds));
		return seconds;
	}

	public override IWeapon GetWeapon () {
		return weapon;
	}

	public override string GetWeaponClass () {
		return weaponClass;
	}

}
