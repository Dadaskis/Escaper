using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIEquipmentAdder : MonoBehaviour {

	public GUIInventory inventory;
	private Dictionary<GUIItem, int> itemToIndex = new Dictionary<GUIItem, int> ();

	//public List<string> items;
	public Color selectedColor;
	public Color notSelectedColor;

	public List<int> GetSelectedItemIndexes() {
		List<int> itemIndexes = new List<int> ();
		foreach (GUIItem item in inventory.items) {
			if (item.selected) {
				itemIndexes.Add (itemToIndex [item]);
			}
		}
		return itemIndexes;
	}

	IEnumerator AddItems() {
		foreach (GUIItem item in inventory.items) {
			if (item != null) {
				Destroy (item.gameObject);
			}
		}
		inventory.items = new List<GUIItem> ();
		yield return new WaitForEndOfFrame ();
		LocationStartSettings settings = GameLogic.GetCurrentLocationSettings ();
		List<EquipmentItem> addedItems = new List<EquipmentItem> ();
		Dictionary<EquipmentItem, GUIItem> equipmentDataToItem = new Dictionary<EquipmentItem, GUIItem> ();
		foreach (EquipmentItem item in settings.items) {
			GUIItem itemObject = inventory.AddItem (ItemManager.GetItem (item.name));
			if (itemObject != null) {
				addedItems.Add (item);
				equipmentDataToItem [item] = itemObject;
			}
		}

		yield return new WaitForEndOfFrame ();

		int index = 0;
		foreach (EquipmentItem item in addedItems) {
			GUIItem itemObject = equipmentDataToItem [item];

			itemObject.selectable = true;
			itemObject.selectedColor = selectedColor;
			itemObject.notSelectedColor = notSelectedColor;
			if (itemObject.background != null) {
				itemObject.background.color = notSelectedColor;
			}
			itemObject.additionalDataText.text = item.price.ToString();
			itemToIndex [itemObject] = index;

			index++;
		}
	}

	void OnEnable() {
		StartCoroutine (AddItems ());
	}

}
