using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CorpseRandomItem {
	public string name = "";
	public int repeatCount = 1;
	public int chance = 50;
}

public class Corpse : IUsableObject {

	public Character character;
	public List<string> items = new List<string>();
	public List<CorpseRandomItem> randomItems = new List<CorpseRandomItem> ();
	public bool used = false;

	void Start() {
		character = GetComponent<Character> ();
		EventManager.AddEventListener<Events.GUIInventories.Close> (OnInventoryClose);
		EventManager.AddEventListener<Events.GUIInventories.OpenNormalInventory> (OnNormalInventoryOpen);
		foreach (CorpseRandomItem item in randomItems) {
			for (int counter = 0; counter < item.repeatCount; counter++) {
				if (Random.Range (0, 100) < item.chance) {
					items.Add (item.name);
				}
			}
		}
	}

	void UpdateItems() {
		GUIInventory inventory = GUICorpseInventorySwitch.instance.corpseInventory;
		items.Clear ();
		Dictionary<int, bool> isChecked = new Dictionary<int, bool> ();
		foreach (GUIItem item in inventory.items) {
			if (item == null) {
				continue;
			}

			if (item.inventory != inventory) {
				continue;
			}

			bool isCheckedItem = false;
			if (isChecked.TryGetValue (item.GetHashCode (), out isCheckedItem)) {
				continue;
			}

			items.Add (item.physicalData.name);
			isChecked [item.GetHashCode ()] = true;
		}
	}

	EventData OnInventoryClose(EventData data) {

		if (used) {
			UpdateItems ();
		}

		used = false;
		
		return new EventData ();
	}

	EventData OnNormalInventoryOpen(EventData data) {

		if (used) {
			if (GUIInventories.instance.corpseInventoryOpened) {
				UpdateItems ();
			}
		}

		used = false;

		return new EventData();
	}

	void FulfillCorpseInventory() {
		GUIInventory inventory = GUICorpseInventorySwitch.instance.corpseInventory;
		foreach (GUIInventorySlots slots in inventory.slots) {
			slots.putRestricted = false;
		}
		foreach(GUIItem item in inventory.items) {
			if (item != null) {
				if (item.inventory == inventory) {
					Destroy (item.gameObject);
				}
			}
		}
		inventory.items.Clear ();
		foreach (string item in items) {
			inventory.AddItem (ItemManager.GetItem (item));
		}
		foreach (GUIInventorySlots slots in inventory.slots) {
			slots.putRestricted = true;
		}
	}

	public override void Use () {
		if (character == null || character.killed) {
			if (!GUIInventories.instance.corpseInventoryOpened) {
				used = true;
				GUIInventories.OpenCorpseInventory (true);
				FulfillCorpseInventory ();
			} else {
				GUIInventories.Close (true);
			}
		}
	}

	string ConvertInputKey(string input) {
		return System.Enum.GetName (typeof(KeyCode), InputManager.instance.keys [input]);
	}

	public override string ShowText () {
		if (!character.killed) {
			return "";
		}
		return "[" + ConvertInputKey ("Use") + "] Loot the corpse";
	}

}
