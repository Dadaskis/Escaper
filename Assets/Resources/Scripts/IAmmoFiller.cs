using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAmmoFiller : MonoBehaviour {

	public virtual int Reload(IWeapon weapon) {
		return weapon.maxAmmo;
	}

}
