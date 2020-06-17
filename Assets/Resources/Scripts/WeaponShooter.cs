using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShooter : MonoBehaviour {

	private IWeapon weapon;

	void Update() {
		if (weapon != null) {
			if (InputManager.GetButton ("PlayerShoot")) {
				weapon.PrimaryFire ();
			} 

			if(InputManager.GetButton("PlayerSight")) {
				weapon.SecondaryFire ();
			}
		} else {
			if (transform.childCount > 0) {
				weapon = GetComponentInChildren<IWeapon> ();
			}
		}
	}

}
