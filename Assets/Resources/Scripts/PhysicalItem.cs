using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PhysicalItemData {
	public string itemDataName;
}

[RequireComponent(typeof(SerializableTransform))]
public class PhysicalItem : IUsableObject {

	public class CustomItemSetDataEvent : UnityEvent<GUIItem, ItemData> {}
	public CustomItemSetDataEvent customItemSetDataEvent = new CustomItemSetDataEvent();

	public ItemData data;
	public bool destroyAfterPickingUp = true;

	void Start() {
		SerializableTransform transform = GetComponent<SerializableTransform> ();
		saveName += transform.saveName + "PhysicalItem";
	}

	string ConvertInputKey(string input) {
		return System.Enum.GetName (typeof(KeyCode), InputManager.instance.keys [input]);
	}

	public override string ShowText () {
		return "[" + ConvertInputKey ("Use") + "] Pickup: " + data.showName;
	}

	public override void Use () {
		GameObject itemObject = Instantiate (data.prefabUI, Player.instance.inventory.transform);

		GUIItem item = itemObject.GetComponent<GUIItem> ();
		item.nameText.text = data.nameText;
		item.additionalDataText.text = data.additionalText;
		item.icon.sprite = data.icon;
		item.size = data.size;
		item.itemTag = data.tag;
		item.fullName = data.fullName;
		item.description = data.description;
		item.actionsType = data.actionType;
		item.physicalData = data;
		item.Resize ();

		customItemSetDataEvent.Invoke (item, data);

		GUISlot slot = null;
		foreach (GUIInventorySlots slots in Player.instance.inventory.slots) {
			slot = slots.GetFreePlace (item);
			if (slot != null) {
				break;
			}
		}
		if (slot == null) {
			Destroy (itemObject);
			return;
		}
		item.BindToSlot (slot);
		if (destroyAfterPickingUp) {
			Destroy (gameObject);
		}
	}

	public override SerializableData GetSerializableData () {
		SerializableData rawData = base.GetSerializableData ();
		PhysicalItemData data = new PhysicalItemData ();
		data.itemDataName = this.data.name;
		rawData.target = data;
		rawData.type = typeof(PhysicalItem);
		return rawData;
	}

	public override void SetSerializableData (SerializableData rawData) {
		PhysicalItemData data = ConvertTargetObject<PhysicalItemData> (rawData.target);
		ItemData itemData = Serializer.GetScriptableObject (data.itemDataName) as ItemData;
		this.data = itemData;
	}

}
