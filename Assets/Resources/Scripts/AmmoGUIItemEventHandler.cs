using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoGUIItemEventHandler : MonoBehaviour {
	public GUIItem item;
	public int currentAmmo = 0;

	public void UpdateText() {
		AmmoItemData data = item.physicalData as AmmoItemData;
		item.additionalDataText.text = currentAmmo + "/" + data.maxAmmo;
	}

	EventData OnItemDrop (EventData data) {
		GUIItem dataItem = data.Get<GUIItem> (0);
		if (dataItem == item) {
			GameObject itemObject = data.Get<GameObject> (1);
			AmmoPhysicalItemEventHandler physicalAmmoHandler = itemObject.GetComponent<AmmoPhysicalItemEventHandler> ();
			if (physicalAmmoHandler != null) {
				physicalAmmoHandler.currentAmmo = currentAmmo;
			}
		}
		return new EventData ();
	}

	void Start() {
		AmmoItemData data = item.physicalData as AmmoItemData;
		currentAmmo = data.maxAmmo;

		EventManager.AddEventListener<Events.GUIItemActionSimple.Drop> (OnItemDrop);
	}
}
