using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonWeaponChanger : IWeaponChanger {

	public Transform weaponHolder;
	public Character owner;
	[HideInInspector] public IWeapon weapon;

	IEnumerator WeaponSetting(float seconds) {
		yield return new WaitForSeconds (seconds);
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
	}

	public override void SetWeapon (string weaponClass) {
		float seconds = 0.1f; 
		if (weapon != null) { 
			seconds = weapon.Takeout ();
			if (seconds == -1.0f) {
				return;
			}
		}
		this.weaponClass = weaponClass;
		StartCoroutine (WeaponSetting (seconds));
	}

	public override IWeapon GetWeapon () {
		return weapon;
	}

	public override string GetWeaponClass () {
		return weaponClass;
	}

}
