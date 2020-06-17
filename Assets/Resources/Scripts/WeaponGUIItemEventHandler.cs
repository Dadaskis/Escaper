using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGUIItemEventHandlerData { 
	public string weaponClass;
}

[RequireComponent(typeof(SerializableTransform))]
public class WeaponGUIItemEventHandler : SerializableMonoBehaviour {

	public GUIItem item;
	public string weaponClass;

	void PlacedInContainer(GUIContainer container) {
		Player.instance.character.weapon.SetWeapon (weaponClass);
	}

	void PlacedInSlots(GUIInventorySlots slots, GUISlot slot) {
		Player.instance.character.weapon.SetWeapon ("");
	}

	void Start () {
		item.placedInContainerEvent.AddListener (PlacedInContainer);
		item.placedInSlotsEvent.AddListener (PlacedInSlots);
		SerializableTransform transform = GetComponent<SerializableTransform> ();
		saveName += transform.saveName + "WeaponGUIItemEventHandler";
	}

	public override SerializableData GetSerializableData () {
		SerializableData rawData = base.GetSerializableData ();
		WeaponGUIItemEventHandlerData data = new WeaponGUIItemEventHandlerData ();
		data.weaponClass = weaponClass;
		rawData.type = typeof(WeaponGUIItemEventHandler);
		rawData.target = data;
		return rawData;
	}

	public override void SetSerializableData (SerializableData rawData) {
		WeaponGUIItemEventHandlerData data = ConvertTargetObject<WeaponGUIItemEventHandlerData>(rawData.target);
		weaponClass = data.weaponClass;
	}

}
