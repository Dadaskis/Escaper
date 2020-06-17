using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonWeaponChanger : IWeaponChanger {

	public Transform weaponHolder;
	public Character owner;
	[HideInInspector] public string weaponClass;
	[HideInInspector] public IWeapon weapon;

	public override void SetWeapon (string weaponClass) {
		for(int index = 0; index < weaponHolder.childCount; index++) {
			Destroy (weaponHolder.GetChild (index).gameObject);
		}
		if (weaponClass.Length == 0) {
			weapon = null;
			weaponClass = "";
			return;
		}
		WeaponData data = WeaponManager.GetWeaponData (weaponClass);
		GameObject obj = Instantiate (data.firstPerson, weaponHolder);
		weapon = obj.GetComponentInChildren<IWeapon> ();
		weapon.firstPerson = true;
		weapon.owner = owner;
		this.weaponClass = weaponClass;
	}

	public override IWeapon GetWeapon () {
		return weapon;
	}

	public override string GetWeaponClass () {
		return weaponClass;
	}

}
