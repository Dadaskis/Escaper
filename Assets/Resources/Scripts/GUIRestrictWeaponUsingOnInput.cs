using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIRestrictWeaponUsingOnInput : MonoBehaviour {
	public string inputName = "";
	public Player player;
	public bool isOpened = false;

	void Update() {
		if(InputManager.GetButtonDown(inputName)) {
			isOpened = !isOpened;	
			if (!isOpened) {
				IWeapon weapon = player.weaponHolder.GetComponentInChildren<IWeapon> ();
				if (weapon != null) {
					weapon.restrictUsing = false;
				}
			}
		}
		if (isOpened) {
			IWeapon weapon = player.weaponHolder.GetComponentInChildren<IWeapon> ();
			if (weapon != null) {
				weapon.restrictUsing = true;
			}
		} 
	}
}
