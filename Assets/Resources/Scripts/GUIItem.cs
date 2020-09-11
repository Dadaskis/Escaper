using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Events.GUIItem {
	class Selected {}
}

[System.Serializable]
public class GUIItemData {

	public Vector2 size = Vector2.one;
	public string nameText;
	public string additionalDataText;
	public string actionsType = "";
	public string tag = "";
	public string sprite = "";
	public bool rotated = false;
	public string physicalData = "";

	public string inventory = "";

	public string rectTransform = "";

	public bool placedInSlots = false;
	public string inventorySlots = "";
	public Vector2 slotPosition = Vector2.zero;

	public bool placedInContainer = false;
	public string container = "";

	public bool selectable = false;
	public bool selected = false;

}

[ExecuteInEditMode]
[RequireComponent(typeof(SerializableTransform))]
public class GUIItem : SerializableMonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler {
	public Vector2 size = Vector2.one;
	public Image icon;
	public Text nameText;
	public Text additionalDataText;
	public string actionsType = "";
	public string itemTag = "";
	public string fullName;
	public string description;
	public GUIInventory inventory;
	public bool rotated = false;
	public bool draging = false;
	public bool rotatedOnBeginDrag = false;
	public ItemData physicalData;
	public bool selectable = false;
	public bool selected = false;
	public Image background;
	public Color selectedColor;
	public Color notSelectedColor;
	public GUISlot bindedSlot = null;
	public GUIInventorySlots bindedInventorySlots = null;
	public GUIContainer bindedContainer = null;

	private RectTransform rectTransform;
	private Canvas canvas;
	private GUIInventorySlots previousSlots;
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
		background = GetComponent<Image> ();

		if (!inventory.items.Contains (this)) {
			inventory.items.Add (this);
		}
	}

	void Update() {
		if (!draging) { 
			return;
		}

		if (InputManager.GetButtonDown ("RotateItem")) {
			Rotate ();
		}
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (selectable) {
			selected = !selected;
			if (background == null) {
				return;
			}
			if (selected) {
				background.color = selectedColor;
			} else {
				background.color = notSelectedColor;
			}
			EventManager.RunEventListeners<Events.GUIItem.Selected> (this);
		}
	}

	public override void SetSerializableData (SerializableData rawData) {
		GUIItemData data = ConvertTargetObject<GUIItemData>(rawData.target);

		actionsType = data.actionsType;
		additionalDataText.text = data.additionalDataText;
		nameText.text = data.nameText;
		size = data.size;
		itemTag = data.tag;
		icon.sprite = SpriteManager.GetSprite (data.sprite);
		physicalData = Serializer.GetScriptableObject (data.physicalData) as ItemData;

		rotated = data.rotated;

		selectable = data.selectable;
		selected = data.selected;

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
		item.rotated = rotated;
		item.physicalData = physicalData.name;
		item.selected = selected;
		item.selectable = selectable;

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
		if (!rotated) {
			size.x = GUISlot.SLOT_SIZE * this.size.x;
			size.y = GUISlot.SLOT_SIZE * this.size.y;
		} else {
			size.y = GUISlot.SLOT_SIZE * this.size.x;
			size.x = GUISlot.SLOT_SIZE * this.size.y;
		}
		rectTransform.sizeDelta = size;
	}

	public void Rotate() {
		rotated = !rotated;
		float sizeX = size.x;
		float sizeY = size.y;
		size = new Vector2 (sizeY, sizeX);
		if (rotated == true) {
			rectTransform.pivot = new Vector2 (0.5f, 0.5f);
			rectTransform.rotation = Quaternion.Euler (new Vector3 (0.0f, 0.0f, 90.0f));
		} else {
			rectTransform.rotation = Quaternion.Euler (new Vector3 (0.0f, 0.0f, 0.0f));
			rectTransform.pivot = new Vector2 (0.0f, 0.0f);
		}
	}

	public void ClearSlotsAfterItem() {
		if (bindedSlot != null && bindedInventorySlots != null) {
			List<GUISlot> slots = bindedInventorySlots.GetSlots ((int) bindedSlot.position.x, (int) bindedSlot.position.y, (int) size.x - 1, (int) size.y - 1);
			foreach (GUISlot slot in slots) {
				slot.item = null;
			}
		}
	}

	public void ClearEverythingAfterItem() {
		ClearSlotsAfterItem ();
		ClearContainerAfterItem ();
	}

	public void ClearContainerAfterItem() {
		if (bindedContainer != null) {
			rectTransform.SetParent (inventory.transform, true);
			rectTransform.anchorMax = Vector2.zero;
			bindedContainer.SetItem (null);
			bindedContainer.image.enabled = true;
			Resize ();
		}
	}

	public void OnBeginDrag (PointerEventData eventData) {
		if (selectable) {
			return;
		}

		ClearEverythingAfterItem ();

		foreach (GUIContainer container in inventory.containers) {
			if (container.CheckWhitelist (this) && container.contains == null) {
				container.image.color = container.puttableInContainerColor;
			} else {
				container.image.color = container.nonPuttableInContainerColor;
			}
		}

		draging = true;
		rotatedOnBeginDrag = rotated;
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
		if (selectable) {
			return;
		}
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
		if (rotated) {
			rectTransform.pivot = new Vector2 (0.0f, 1.0f);
		}
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
		GUIInventory previousInventory = inventory;
		inventory = slot.inventory.inventory;
		if (inventory != previousInventory) {
			rectTransform.SetParent (inventory.transform, true);
			Debug.LogError (previousInventory);
			if (previousInventory != null) {
				previousInventory.items.Remove (this);
			}
			inventory.items.Add (this);
		}
		placedInSlotsEvent.Invoke (bindedInventorySlots, bindedSlot);
	}

	public void BindToContainer(GUIContainer container) {
		FindNeededData ();

		if (rotated) {
			Rotate ();
		}

		rectTransform.SetParent (container.transform);
		rectTransform.anchorMax = Vector2.one;
		rectTransform.sizeDelta = Vector2.zero;
		rectTransform.anchoredPosition = Vector2.zero;
		container.SetItem (this);
		bindedSlot = null;
		bindedInventorySlots = null;
		bindedContainer = container;
		inventory = container.inventory;
		placedInContainerEvent.Invoke (container);
		container.image.enabled = false;
	}

	public void OnEndDrag (PointerEventData eventData) {
		if (selectable) {
			return;
		}
		draging = false;

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
			if (rotatedOnBeginDrag != rotated) {
				Rotate ();
			} 
			if (bindedContainer != null) {
				BindToContainer (bindedContainer);
			} else if (bindedSlot != null && bindedInventorySlots != null) {
				BindToSlot (bindedSlot);
			} 
		}

		foreach (GUIContainer containerToColor in inventory.containers) {
			containerToColor.image.color = containerToColor.idleInContainerColor;
		}
	}
}
