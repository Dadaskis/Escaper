using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GUIInventorySlotsData {
	
}

[ExecuteInEditMode]
[RequireComponent(typeof(SerializableTransform))]
public class GUIInventorySlots : SerializableMonoBehaviour {

	public int width = 1;
	public int height = 1;
	public GameObject slot;
	public GUISlot[,] slots;
	public GUIInventory inventory;
	public Color idleSlotColor;
	public Color puttableSlotColor;
	public Color nonPuttableSlotColor;
	public bool putRestricted = false;

	void Awake() {
		GenerateSlots ();

		inventory = GetComponentInParent<GUIInventory> ();
		inventory.slots.Add (this);
	}

	void Start () {
		SerializableTransform transform = GetComponent<SerializableTransform> ();
		saveName += transform.saveName + "InventorySlots";
	}

	public override SerializableData GetSerializableData () {
		SerializableData rawData = base.GetSerializableData ();

		rawData.type = typeof(GUIInventorySlots);
		rawData.target = new GUIInventorySlotsData ();
		return rawData;
	}

	public override void SetSerializableData (SerializableData rawData) {
		GUIInventorySlotsData data = ConvertTargetObject<GUIInventorySlotsData> (rawData.target);
	}

	public void ColorAllSlots(Color color) {
		foreach (GUISlot slot in slots) {
			slot.image.color = color;
		}
	}

	public void ColorSlots(List<GUISlot> slots, Color color) {
		foreach (GUISlot slot in slots) {
			slot.image.color = color;
		}
	}

	public bool PositionInRange(int X, int Y) {
		if (X >= 0 && X <= slots.GetUpperBound (0)) {
			if (Y >= 0 && Y <= slots.GetUpperBound (1)) {
				return true;
			}
		}
		return false;
	}

	public List<GUISlot> GetSlots(int X, int Y, int width, int height, bool blacklistWithItem = false) {
		List<GUISlot> neededSlots = new List<GUISlot> ();
		int startX = X;
		int startY = Y;
		for (X = startX; X <= startX + width; X++) {
			for (Y = startY; Y <= startY + height; Y++) {
				if (PositionInRange (X, Y)) {
					GUISlot neededSlot = slots [X, Y];
					if (neededSlot != null) {
						if (blacklistWithItem) {
							if (neededSlot.item == null) {
								neededSlots.Add (neededSlot);
							}
						} else {
							neededSlots.Add (neededSlot);
						}
					}
				} 
			}
		}
		return neededSlots;
	}

	public bool IsPlaceable(GUIItem item, GUISlot slot) {
		if (putRestricted) {
			return false;
		}

		List<GUISlot> neededSlots = GetSlots ((int) slot.position.x, (int) slot.position.y, (int) item.size.x - 1, (int) item.size.y - 1, true);

		if (neededSlots.Count == item.size.x * item.size.y) {
			return true;
		} 
		return false;
	}

	public bool IsPlaceable(GUIItem item, GUISlot slot, out List<GUISlot> slots) {
		if (putRestricted) {
			slots = new List<GUISlot> ();
			return false;
		}

		List<GUISlot> neededSlots = GetSlots ((int) slot.position.x, (int) slot.position.y, (int) item.size.x - 1, (int) item.size.y - 1, true);

		slots = neededSlots;

		if (neededSlots.Count == item.size.x * item.size.y) {
			return true;
		} 
		return false;
	}

	public GUISlot GetFreePlace(GUIItem item) {
		if (putRestricted) {
			return null;
		}

		if (slots == null) {
			return null;
		}
		for (int X = 0; X < width; X++) {
			for (int Y = 0; Y < height; Y++) {
				if(IsPlaceable(item, slots[X, Y])) {
					return slots [X, Y];
				}
			}
		}
		return null;
	}

	public void ReactToItem(GUIItem item, GUISlot slot, bool colorAllSlotsIdle = true) {
		if (colorAllSlotsIdle) {
			ColorAllSlots (idleSlotColor);
		}

		List<GUISlot> neededSlots;
		if (IsPlaceable (item, slot, out neededSlots)) {
			ColorSlots (neededSlots, puttableSlotColor);
		} else {
			ColorSlots (neededSlots, nonPuttableSlotColor);
		}
	}

	public void ColorSlotsUnderItem(GUIItem item, GUISlot slot, Color color) {
		List<GUISlot> slots = GetSlots ((int) slot.position.x, (int) slot.position.y, (int) item.size.x - 1, (int) item.size.y - 1, true);
		ColorSlots (slots, color);
	}

	public void GenerateSlots() {
		while(transform.childCount > 0){
			for (int index = 0; index < transform.childCount; index++) {
				DestroyImmediate (transform.GetChild (index).gameObject);
			}
		}

		slots = new GUISlot[width, height];

		for (int Y = 0; Y < height; Y++) {
			for (int X = 0; X < width; X++) {
				GameObject newSlot = Instantiate (slot, transform);

				GUISlot slotData = newSlot.GetComponent<GUISlot> ();
				slotData.position.x = X;
				slotData.position.y = Y;
				slotData.inventory = this;

				slots [X, Y] = slotData;
			}
		}

		for (int Y = 0; Y < height; Y++) {
			for (int X = 0; X < width; X++) {
				GameObject newSlot = slots [X, Y].gameObject;
				RectTransform newSlotRect = newSlot.GetComponent<RectTransform> ();
				Vector2 position = newSlotRect.anchoredPosition;
				position.x = newSlotRect.rect.width * X;
				position.y = newSlotRect.rect.height * Y;
				newSlotRect.anchoredPosition = position;
			}
		}
	}
}
