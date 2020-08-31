using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUICorpseInventorySwitch : MonoBehaviour {

	public static GUICorpseInventorySwitch instance;

	public GameObject equipment;
	public GameObject corpseInventoryObject;
	public GUIInventory corpseInventory;

	void Awake() {
		instance = this;
	}

	public void EnableNormalInventory() {
		equipment.SetActive (true);
		corpseInventoryObject.SetActive (false);
	}

	public void EnableCorpseInventory() {
		equipment.SetActive (false);
		corpseInventoryObject.SetActive (true);
	}

}
