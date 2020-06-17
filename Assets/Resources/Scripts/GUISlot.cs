using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GUISlotData {
	public GUIItemData item;
	public Vector2 position;
	public GUIInventorySlotsData inventory;
}

public class GUISlot : MonoBehaviour {

	public const int SLOT_SIZE = 30;

	public Vector2 position = Vector2.zero;
	public Image image;
	public GUIInventorySlots inventory;
	public GUIItem item = null;

}
