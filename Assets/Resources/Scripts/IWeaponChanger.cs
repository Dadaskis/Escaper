using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IWeaponChanger : MonoBehaviour {

	public virtual void SetWeapon(string weaponClass) {}

	public virtual IWeapon GetWeapon() { 
		return null;
	}

	public virtual string GetWeaponClass() {
		return "null";
	}

}
