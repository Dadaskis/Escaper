using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveRandomWeaponToCharacter : MonoBehaviour {

	public List<string> weapons;
	public Character character;

	void Start () {
		string weaponClass = weapons [Random.Range (0, weapons.Count - 1)];
		character.weapon.SetWeapon (weaponClass);
	}
}
