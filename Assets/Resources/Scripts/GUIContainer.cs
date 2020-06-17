using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class GUIContainerData {
	public List<string> whiteList = new List<string>();
}

[RequireComponent(typeof(SerializableTransform))]
public class GUIContainer : SerializableMonoBehaviour {
	
	public Color idleInContainerColor;
	public Color puttableInContainerColor;
	public Color nonPuttableInContainerColor;
	public List<string> whiteList = new List<string>();
	public GUIItem contains = null;
	public GUIInventory inventory;
	public Image image;

	public class SetItemEvent : UnityEvent<GUIItem> {}
	public SetItemEvent setItemEvent = new SetItemEvent();

	void Start() {
		inventory = GetComponentInParent<GUIInventory> ();
		inventory.containers.Add (this);

		SerializableTransform transform = GetComponent<SerializableTransform> ();
		saveName += transform.saveName + "InventoryContainer";
	}

	public bool CheckWhitelist(GUIItem item) {
		return whiteList.BinarySearch (item.itemTag) > -1;
	}

	public override SerializableData GetSerializableData () {
		SerializableData rawData = base.GetSerializableData ();
		GUIContainerData data = new GUIContainerData ();
		data.whiteList = whiteList;
		rawData.target = data;
		rawData.type = typeof(GUIContainer);
		return rawData;
	}

	public override void SetSerializableData (SerializableData rawData) {
		GUIContainerData data = ConvertTargetObject<GUIContainerData> (rawData.target);
		contains = null;
		whiteList = data.whiteList;
	}

	public void SetItem(GUIItem item) {
		contains = item;
		setItemEvent.Invoke (item);
	}

	public GUIItem GetItem() {
		return contains;
	}

}
