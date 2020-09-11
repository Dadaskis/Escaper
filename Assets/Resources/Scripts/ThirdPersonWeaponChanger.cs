using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonWeaponChanger : IWeaponChanger {

	public Transform weaponHolder;
	public Character owner;
	[HideInInspector] public IWeapon weapon;

	public override float SetWeapon (string weaponClass) {
		WeaponData data = WeaponManager.GetWeaponData (weaponClass);
		GameObject obj = Instantiate (data.thirdPerson, weaponHolder);
		weapon = obj.GetComponentInChildren<IWeapon> ();
		weapon.firstPerson = false;
		weapon.owner = owner;
		this.weaponClass = weaponClass;
		return 0.0f;
	}

	public override IWeapon GetWeapon () {
		return weapon;
	}

	public override string GetWeaponClass () {
		return weaponClass;
	}

}
