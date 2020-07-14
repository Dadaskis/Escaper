using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIRestrictWeaponUsingOnInput : MonoBehaviour {
	public string inputName = "";
	public Player player;
	public bool isOpened = false;
	public WeaponShooter shooter;

	void Update() {
		if(InputManager.GetButtonDown(inputName)) {
			isOpened = !isOpened;
		}
		shooter.enabled = !isOpened;
	}
}
