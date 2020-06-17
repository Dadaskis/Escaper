using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class GUIItemData {

	public Vector2 size = Vector2.one;
	public string nameText;
	public string additionalDataText;
	public string actionsType = "";
	public string tag = "";
	public string sprite = "";

	public string inventory = "";

	public string rectTransform = "";

	public bool placedInSlots = false;
	public string inventorySlots = "";
	public Vector2 slotPosition = Vector2.zero;

	public bool placedInContainer = false;
	public string container = "";

}

[ExecuteInEditMode]
[RequireComponent(typeof(SerializableTransform))]
public class GUIItem : SerializableMonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public Vector2 size = Vector2.one;
	public Image icon;
	public Text nameText;
	public Text additionalDataText;
	public string actionsType = "";
	public string itemTag = "";
	public GUIInventory inventory;

	private RectTransform rectTransform;
	private Canvas canvas;
	private GUIInventorySlots previousSlots;
	private GUISlot bindedSlot = null;
	private GUIInventorySlots bindedInventorySlots = null;
	private GUIContainer bindedContainer = null;
	private SerializableTransform serializableTransform;

	public class PlacedInContainerEvent : UnityEvent<GUIContainer> {}
	public PlacedInContainerEvent placedInContainerEvent = new PlacedInContainerEvent();

	public class PlacedInSlotsEvent : UnityEvent<GUIInventorySlots, GUISlot> {}
	public PlacedInSlotsEvent placedInSlotsEvent = new PlacedInSlotsEvent();

	void Start() {
		rectTransform = GetComponent<RectTransform> ();
		canvas = GetComponentInParent<Canvas> ();
		inventory = GetComponentInParent<GUIInventory> ();
		serializableTransform = GetComponent<SerializableTransform> ();
		saveName += serializableTransform.saveName + "Item";
	}

	public override void SetSerializableData (SerializableData rawData) {
		GUIItemData data = ConvertTargetObject<GUIItemData>(rawData.target);

		actionsType = data.actionsType;
		additionalDataText.text = data.additionalDataText;
		nameText.text = data.nameText;
		size = data.size;
		itemTag = data.tag;
		icon.sprite = SpriteManager.GetSprite (data.sprite);

		if (data.placedInSlots) {
			GUIInventorySlots slots = Serializer.GetComponent (data.inventorySlots) as GUIInventorySlots;
			GUISlot slot = slots.slots [(int) data.slotPosition.x, (int) data.slotPosition.y];
			BindToSlot (slot);
		} else if (data.placedInContainer) {
			GUIContainer container = Serializer.GetComponent (data.container) as GUIContainer;
			BindToContainer (container);
		}

		inventory = Serializer.GetComponent (data.inventory) as GUIInventory;

		rectTransform = GetComponent<RectTransform> ();
		canvas = GetComponentInParent<Canvas> ();
	}

	public override SerializableData GetSerializableData () {
		FindNeededData ();
		SerializableData rawData = base.GetSerializableData ();

		GUIItemData item = new GUIItemData ();
		item.actionsType = actionsType;
		item.additionalDataText = additionalDataText.text;
		item.nameText = nameText.text;
		item.size = size;
		item.tag = itemTag;
		item.sprite = icon.sprite.name;

		if (bindedSlot != null && bindedInventorySlots != null) {
			item.placedInSlots = true;
			item.inventorySlots = bindedInventorySlots.saveName;
			item.slotPosition = bindedSlot.position;
		} else if (bindedContainer != null) {
			item.placedInContainer = true;
			item.container = bindedContainer.saveName;
		}

		item.inventory = inventory.saveName;

		rawData.target = item;
		rawData.type = typeof(GUIItem);
		return rawData;
	}

	public void Resize() {
		if (rectTransform == null) {
			rectTransform = GetComponent<RectTransform> ();
		}
		Vector2 size = rectTransform.sizeDelta;
		size.x = GUISlot.SLOT_SIZE * this.size.x;
		size.y = GUISlot.SLOT_SIZE * this.size.y;
		rectTransform.sizeDelta = size;
	}

	public void OnBeginDrag (PointerEventData eventData) {
		if (bindedSlot != null && bindedInventorySlots != null) {
			List<GUISlot> slots = bindedInventorySlots.GetSlots ((int) bindedSlot.position.x, (int) bindedSlot.position.y, (int) size.x - 1, (int) size.y - 1);
			foreach (GUISlot slot in slots) {
				slot.item = null;
			}
		}

		if (bindedContainer != null) {
			rectTransform.SetParent (inventory.transform, true);
			rectTransform.anchorMax = Vector2.zero;
			bindedContainer.SetItem (null);
			Resize ();
		}

		foreach (GUIContainer container in inventory.containers) {
			if (container.CheckWhitelist (this) && container.contains == null) {
				container.image.color = container.puttableInContainerColor;
			} else {
				container.image.color = container.nonPuttableInContainerColor;
			}
		}
	}

	public void FindNeededData() {
		if (rectTransform == null) {
			rectTransform = GetComponent<RectTransform> ();
		}

		if (canvas == null) {
			canvas = GetComponentInParent<Canvas> ();
		}

		if (inventory == null) {
			inventory = GetComponentInParent<GUIInventory> ();
		}

		if (serializableTransform == null) {
			serializableTransform = GetComponent<SerializableTransform> ();
		}
	}

	public void OnDrag (PointerEventData eventData) {
		FindNeededData ();

		rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
		List<RaycastResult> raycastResults = GUICustomUtility.RaycastMouse ();
		bool findedSlots = false;
		foreach (RaycastResult result in raycastResults) {
			GameObject obj = result.gameObject;
			GUISlot slot = obj.GetComponent<GUISlot> ();
			if (slot != null) {
				GUIInventorySlots slots = obj.GetComponentInParent<GUIInventorySlots> ();
				if (slots != null) {
					slots.ReactToItem (this, slot);
					if (slots != previousSlots && previousSlots != null) {
						previousSlots.ColorAllSlots (previousSlots.idleSlotColor);
					}
					previousSlots = slots;
					findedSlots = true;
				}
			} 
		}

		if (previousSlots != null && !findedSlots) {
			previousSlots.ColorAllSlots (previousSlots.idleSlotColor);
			previousSlots = null;
		}
	}

	public void BindToSlot(GUISlot slot) {
		FindNeededData ();
		rectTransform.anchorMin = Vector2.zero;
		rectTransform.anchorMax = Vector2.zero;
		Transform previousParent = rectTransform.parent;
		rectTransform.SetParent (slot.transform, false);
		rectTransform.anchoredPosition = Vector2.zero;
		rectTransform.SetParent (previousParent, true);
		List<GUISlot> slots = slot.inventory.GetSlots ((int) slot.position.x, (int) slot.position.y, (int) size.x - 1, (int) size.y - 1);
		foreach (GUISlot checkSlot in slots) {
			checkSlot.item = this;
		}
		bindedSlot = slot;
		bindedInventorySlots = slot.inventory;
		bindedContainer = null;
		Resize ();
		placedInSlotsEvent.Invoke (bindedInventorySlots, bindedSlot);
	}

	public void BindToContainer(GUIContainer container) {
		FindNeededData ();
		rectTransform.SetParent (container.transform);
		rectTransform.anchorMax = Vector2.one;
		rectTransform.sizeDelta = Vector2.zero;
		rectTransform.anchoredPosition = Vector2.zero;
		container.SetItem (this);
		bindedSlot = null;
		bindedInventorySlots = null;
		bindedContainer = container;
		placedInContainerEvent.Invoke (container);
	}

	public void OnEndDrag (PointerEventData eventData) {
		List<RaycastResult> raycastResults = GUICustomUtility.RaycastMouse ();
		GUISlot slot = null;
		GUIContainer container = null;
		foreach (RaycastResult result in raycastResults) {
			GUISlot findedSlot = result.gameObject.GetComponent<GUISlot> ();
			if (findedSlot != null) {
				slot = findedSlot;
			}
			GUIContainer findedContainer = result.gameObject.GetComponent<GUIContainer> ();
			if (findedContainer != null) {
				container = findedContainer;
			}
		}

		bool placed = false;
		if (slot != null) {
			if (slot.inventory.IsPlaceable (this, slot)) {
				BindToSlot (slot);
				placed = true;
			}
			slot.inventory.ColorAllSlots (slot.inventory.idleSlotColor);
		} else if (container != null) {
			if (container.CheckWhitelist (this) && container.contains == null) {
				BindToContainer (container);
				placed = true;
			}
		} 

		if(!placed) {
			if (bindedContainer != null) {
				BindToContainer (bindedContainer);
			} else if (bindedSlot != null && bindedInventorySlots != null) {
				BindToSlot (bindedSlot);
			} //else {
				//rectTransform.anchoredPosition = onBeginPosition;
			//}
		}

		foreach (GUIContainer containerToColor in inventory.containers) {
			containerToColor.image.color = containerToColor.idleInContainerColor;
		}
	}
}
