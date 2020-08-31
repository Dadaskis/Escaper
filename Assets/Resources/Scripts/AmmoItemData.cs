using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Create weapon ammo item data", menuName = "New weapon ammo item data")]
public class AmmoItemData : ItemData {
	public int maxAmmo = 1;
	public string ammoType = "";
}
