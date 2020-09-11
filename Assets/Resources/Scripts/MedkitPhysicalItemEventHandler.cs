using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitPhysicalItemEventHandler : MonoBehaviour {

	public int currentUse = -1;
	public PhysicalItem item;

	void SetData(GUIItem item, ItemData itemData) {
		MedkitItemData medkitItemData = itemData as MedkitItemData;
		MedkitGUIItemEventHandler medkitUIData = item.GetComponent<MedkitGUIItemEventHandler> ();
		if (currentUse == -1) {
			medkitUIData.currentUse = medkitItemData.maxUses;
		} else {
			medkitUIData.currentUse = currentUse;
		}
		medkitUIData.UpdateInfoAfterUse ();
	}

	void Start() {
		item.customItemSetDataEvent.AddListener (SetData);
	}

}
