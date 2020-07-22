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

			if (InputManager.GetButton("PlayerSight")) {
				weapon.SecondaryFire ();
			}

			if (InputManager.GetButtonDown ("PlayerShoot")) {
				weapon.SinglePrimaryFire ();
			}

			if (InputManager.GetButtonDown ("PlayerSight")) {
				weapon.SingleSecondaryFire ();
			}

			if (InputManager.GetButtonDown ("PlayerReload")) {
				weapon.Reload ();
			}

			if (InputManager.GetButtonDown ("PlayerPunch")) {
				weapon.Punch ();
			}

			if (InputManager.GetButtonDown("PlayerRun")) {
				weapon.Save ();
			}

			if (InputManager.GetButtonUp("PlayerRun")) {
				weapon.UnSave ();
			}

			if (InputManager.GetButtonDown("PlayerMagCheck")) {
				weapon.MagCheck ();
			}
		} else {
			if (transform.childCount > 0) {
				weapon = GetComponentInChildren<IWeapon> ();
				if (weapon != null) {
					weapon.owner = Player.instance.character;
				}
			}
		}
	}

}
