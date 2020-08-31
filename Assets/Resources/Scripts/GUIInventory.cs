using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class GUIInventoryData {

	public List<string> containers = new List<string>();
	public List<string> slots = new List<string>();

}

[RequireComponent(typeof(SerializableTransform))]
public class GUIInventory : SerializableMonoBehaviour {

	public List<GUIContainer> containers;
	public List<GUIInventorySlots> slots;
	public List<GUIItem> items = new List<GUIItem>();
	public GUIInventoryData inventory;
	public RectTransform dropOutTransform;
	public GUIItemDropOutWindow dropOutWindow;
	public GUIItemInfoWindow itemInfoWindow;

	private Canvas canvas;

	public GUIItem AddItem(ItemData data) {
		GameObject itemObject = Instantiate (data.prefabUI, transform);

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

		//customItemSetDataEvent.Invoke (item, data);

		GUISlot slot = null;
		foreach (GUIInventorySlots slots in this.slots) {
			slot = slots.GetFreePlace (item);
			if (slot != null) {
				break;
			}
		}
		if (slot == null) {
			item.Rotate ();
			foreach (GUIInventorySlots slots in this.slots) {
				slot = slots.GetFreePlace (item);
				if (slot != null) {
					break;
				}
			}
			if (slot == null) {
				Destroy (itemObject);
				return null;
			}
		}
		item.BindToSlot (slot);

		return item;
	}

	void Start () {
		canvas = GetComponentInParent<Canvas> ();
		SerializableTransform transform = GetComponent<SerializableTransform> ();
		saveName += transform.saveName + "Inventory";
	}

	void Update() {
		if (InputManager.GetButtonDown ("PlayerSight") == true) {
			List<RaycastResult> raycastResults = GUICustomUtility.RaycastMouse ();

			foreach (RaycastResult raycast in raycastResults) {
				GUIItem item = raycast.gameObject.GetComponent<GUIItem> ();
				if (item != null) {
					if (item.inventory != this) {
						break;
					}
					Transform previousParent = dropOutTransform.parent;
					dropOutTransform.SetParent (null, true);
					dropOutTransform.SetParent (previousParent, true);

					dropOutTransform.gameObject.SetActive (true);
					Vector2 mousePosition = Input.mousePosition;
					mousePosition.x -= Screen.width * 0.5f;
					mousePosition.y -= Screen.height * 0.5f;
					mousePosition /= canvas.scaleFactor;
					dropOutTransform.anchoredPosition = mousePosition;

					dropOutWindow.Work (GUIDropOutActionRegister.Get(item.actionsType), item);
				}
			}
		}
	}

	public override SerializableData GetSerializableData () {
		SerializableData rawData = base.GetSerializableData ();
		GUIInventoryData data = new GUIInventoryData ();

		foreach (GUIContainer container in containers) {
			data.containers.Add (container.saveName);
		}

		foreach (GUIInventorySlots slots in slots) {
			data.slots.Add (slots.saveName);
		}

		rawData.type = typeof(GUIInventory);
		rawData.target = data;
		return rawData;
	}

	public override void SetSerializableData (SerializableData rawData) {
		GUIInventoryData data = ConvertTargetObject<GUIInventoryData> (rawData.target);

		foreach (string containerSaveName in data.containers) {
			GUIContainer container = Serializer.GetComponent (containerSaveName) as GUIContainer;
			containers.Add (container);
			container.inventory = this;
		}
	}

	public void OpenItemInfoWindow(GUIItem item) {
		itemInfoWindow.gameObject.SetActive (true);
		itemInfoWindow.SetItem (item);
		transform.SetAsLastSibling ();
	}

}
