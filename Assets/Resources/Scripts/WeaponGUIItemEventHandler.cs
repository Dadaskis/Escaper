using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGUIItemEventHandlerData { 
	public int currentAmmo = 0;
	public int currentDurability = 0;
}

[RequireComponent(typeof(SerializableTransform))]
public class WeaponGUIItemEventHandler : SerializableMonoBehaviour {

	public GUIItem item;
	public int currentAmmo = 0;
	public int currentDurability = 0;
	public static bool initializedFirst = false;

	void PlacedInContainer(GUIContainer container) {
		//Player.instance.character.weapon.SetWeapon (weaponClass);
	}

	void PlacedInSlots(GUIInventorySlots slots, GUISlot slot) {
		if (Player.instance.character.weapon.GetWeapon ()) {
			if (Player.instance.weaponSlots.currentSlot == Player.instance.character.weapon.GetWeapon ().slot) {
				Player.instance.character.weapon.SetWeapon ("");
			}
		}
	}

	EventData OnItemDrop (EventData data) {
		GUIItem dataItem = data.Get<GUIItem> (0);
		if (dataItem == item) {
			GameObject itemObject = data.Get<GameObject> (1);
			WeaponPhysicalItemEventHandler physicalWeaponHandler = itemObject.GetComponent<WeaponPhysicalItemEventHandler> ();
			if (physicalWeaponHandler != null) {
				physicalWeaponHandler.currentAmmo = currentAmmo;
				physicalWeaponHandler.currentDurability = currentDurability;
			}
		}
		return new EventData ();
	}

	private static EventData OnLocationStart(EventData data) {

		GUIItem item = data.Get<GUIItem> (0);
		WeaponItemData itemData = item.physicalData as WeaponItemData;
		if (itemData != null) {
			WeaponData weaponData = WeaponManager.GetWeaponData (itemData.weaponClass);
			IWeapon weaponObject = weaponData.firstPerson.GetComponent<IWeapon> ();
			WeaponGUIItemEventHandler handler = item.GetComponent<WeaponGUIItemEventHandler> ();
			handler.currentAmmo = weaponObject.maxAmmo;
			handler.currentDurability = weaponObject.maxDurability;
			handler.UpdateText ();

			List<GUIContainer> containers = Player.instance.inventory.containers;
			int placedInSlot = -1;
			foreach (GUIContainer container in containers) {
				GUIContainerWeaponEventHandler weaponHandler = container.GetComponent<GUIContainerWeaponEventHandler> ();
				if (weaponHandler != null) {
					if (container.contains == null) {
						item.ClearSlotsAfterItem ();
						item.BindToContainer (container);
						placedInSlot = weaponHandler.weaponSlot;
						break;
					}
				}
			}

			if (Player.instance.weaponSlots.currentSlot == -1 && placedInSlot != -1) {
				Player.instance.weaponSlots.TakeWeapon (placedInSlot);
			}
		}

		return new EventData ();
	}

	void Start () {
		if (initializedFirst == false) {
			initializedFirst = true;
			EventManager.AddEventListener<Events.GameLogic.AddedItemToThePlayer> (OnLocationStart);
		}
		//item.placedInContainerEvent.AddListener (PlacedInContainer);
		item.placedInSlotsEvent.AddListener (PlacedInSlots);
		SerializableTransform transform = GetComponent<SerializableTransform> ();
		saveName += transform.saveName + "WeaponGUIItemEventHandler";
		EventManager.AddEventListener<Events.GUIItemActionSimple.Drop> (OnItemDrop);
	}

	public void UpdateText() {
		WeaponItemData data = item.physicalData as WeaponItemData;
		GameObject weapon = WeaponManager.GetWeaponData (data.weaponClass).firstPerson;
		if (weapon == null) {
			weapon = WeaponManager.GetWeaponData (data.weaponClass).thirdPerson;
		}
		if(weapon != null) {
			IWeapon weaponData = weapon.GetComponent<IWeapon> ();
			//item.additionalDataText.text = currentAmmo + "/" + weaponData.maxAmmo;
			item.additionalDataText.text = ((int)(((float)currentDurability / (float)weaponData.maxDurability) * 100.0f)) + "%";
		}
	}

	public override SerializableData GetSerializableData () {
		SerializableData rawData = base.GetSerializableData ();
		WeaponGUIItemEventHandlerData data = new WeaponGUIItemEventHandlerData ();
		data.currentAmmo = currentAmmo;
		rawData.type = typeof(WeaponGUIItemEventHandler);
		rawData.target = data;
		return rawData;
	}

	public override void SetSerializableData (SerializableData rawData) {
		WeaponGUIItemEventHandlerData data = ConvertTargetObject<WeaponGUIItemEventHandlerData>(rawData.target);
		currentAmmo = data.currentAmmo;
	}

}
