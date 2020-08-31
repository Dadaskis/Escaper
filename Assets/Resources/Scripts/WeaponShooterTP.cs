using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShooterTP : MonoBehaviour {

	public IWeapon weapon;

	void Start() {
		CheckWeapon ();
	}

	//void Update() {}

	bool CheckWeapon() {
		if (weapon != null) {
			return true;
		}
		if (transform.childCount > 0) {
			weapon = GetComponentInChildren<IWeapon> ();
			if (weapon != null) {
				Character character = GetComponentInParent<Character> ();
				if (character != null) {
					weapon.owner = character;
				}
			}
		}
		if (weapon != null) {
			return true;
		}
		return false;
	}

	public void Shoot(GameObject target, Vector3 customHitPosition = default(Vector3)) {
		if (CheckWeapon ()) {
			weapon.target = target;
			weapon.overrideTargetHitPosition = customHitPosition;
			weapon.PrimaryFire ();
		}
	}

	public void Punch(GameObject target) {
		if (CheckWeapon ()) {
			weapon.target = target;
			weapon.Punch ();
		}
	}

	public void Reload() {
		if (CheckWeapon ()) {
			weapon.Reload ();
		}
	}

	public void Jam() {
		if (CheckWeapon ()) {
			IFirearm firearm = weapon as IFirearm;
			if (firearm != null) {
				firearm.Jam ();
			}
		}
	}

	public bool IsJammed() {
		if (CheckWeapon ()) {
			IFirearm firearm = weapon as IFirearm;
			if (firearm != null) {
				return firearm.jammed;
			}
		}
		return false;
	}

	public void Save() {
		if (CheckWeapon ()) {
			weapon.Save ();
		}
	}

	public void UnSave() {
		if (CheckWeapon ()) {
			weapon.UnSave ();
		}
	}

}
