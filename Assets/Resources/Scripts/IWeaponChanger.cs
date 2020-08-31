using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IWeaponChanger : MonoBehaviour {

	[HideInInspector] public string weaponClass;
	[HideInInspector] public int slot;
	[HideInInspector] public int currentAmmo = 0;

	public virtual void SetWeapon(string weaponClass) {}

	public virtual IWeapon GetWeapon() { 
		return null;
	}

	public virtual string GetWeaponClass() {
		return "null";
	}

}
