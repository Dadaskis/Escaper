using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Events.GUIInventories {
	class OpenNormalInventory {}
	class OpenCorpseInventory {}
	class Close {}
}

public class GUIInventories : MonoBehaviour {

	public static GUIInventories instance;
	public bool corpseInventoryOpened = false;
	public bool normalInventoryOpened = false;
	public bool restrictInventoryOpening = false;
	public FirstPersonController controller;

	public void DisableMouseLook() {
		if (controller == null) {
			controller = FindObjectOfType<FirstPersonController> ();
		}

		if (controller != null) {
			controller.enableMouseLook = false;
			controller.mouseLook.SetCursorLock (false);
			controller.mouseLook.UpdateCursorLock ();
		}
	}

	public void EnableMouseLook() {
		if (controller == null) {
			controller = FindObjectOfType<FirstPersonController> ();
		}

		if (controller != null) {
			controller.enableMouseLook = true;
			controller.mouseLook.SetCursorLock (true);
			controller.mouseLook.UpdateCursorLock ();
		}
	}

	void Awake() {
		instance = this;
	}

	public static void OpenNormalInventory(bool switchMouseLook = false) {
		if (instance.restrictInventoryOpening) {
			return;
		}
		EventManager.RunEventListeners<Events.GUIInventories.OpenNormalInventory> ();
		GUICorpseInventorySwitch.instance.EnableNormalInventory ();	
		instance.gameObject.SetActive (true);
		instance.DisableMouseLook ();
		IWeapon weapon = Player.instance.character.weapon.GetWeapon (); 
		if (weapon != null) {
			weapon.restrictUsing = true;
		}
		instance.corpseInventoryOpened = false;
		instance.normalInventoryOpened = true;
	}

	public static void OpenCorpseInventory(bool switchMouseLook = false) {
		if (instance.restrictInventoryOpening) {
			return;
		}
		EventManager.RunEventListeners<Events.GUIInventories.OpenCorpseInventory> ();
		GUICorpseInventorySwitch.instance.EnableCorpseInventory ();
		instance.gameObject.SetActive (true);
		instance.DisableMouseLook ();
		IWeapon weapon = Player.instance.character.weapon.GetWeapon (); 
		if (weapon != null) {
			weapon.restrictUsing = true;
		}
		instance.corpseInventoryOpened = true;
		instance.normalInventoryOpened = false;
	}

	public static void Close(bool switchMouseLook = true) {
		EventManager.RunEventListeners<Events.GUIInventories.Close> ();
		instance.gameObject.SetActive(false);
		//if (switchMouseLook) {
		//	instance.inventoryMouseLookDisabler.Switch ();
		//}
		if (switchMouseLook) {
			instance.EnableMouseLook ();
		}
		IWeapon weapon = Player.instance.character.weapon.GetWeapon (); 
		if (weapon != null) {
			weapon.restrictUsing = false;
		}
		instance.corpseInventoryOpened = false;
		instance.normalInventoryOpened = false;
		InSightDisabler.instance.image.enabled = true;
	}

}
