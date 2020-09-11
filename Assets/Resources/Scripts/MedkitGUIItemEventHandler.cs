using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MedkitGUIItemEventHandler : MonoBehaviour {

	public GUIItem item;
	public int currentUse = -1;
	public string currentTrigger = "";
	public string currentModel = "";

	public void UpdateText() {
		MedkitItemData data = item.physicalData as MedkitItemData;
		item.additionalDataText.text = currentUse + "/" + data.maxUses;
	}

	EventData OnItemDrop (EventData data) {
		GUIItem dataItem = data.Get<GUIItem> (0);
		if (dataItem == item) {
			GameObject itemObject = data.Get<GameObject> (1);
			MedkitPhysicalItemEventHandler physicalMedkitHandler = itemObject.GetComponent<MedkitPhysicalItemEventHandler> ();
			if (physicalMedkitHandler != null) {
				physicalMedkitHandler.currentUse = currentUse;
			}
		}
		return new EventData ();
	}

	IEnumerator AddHealth() {
		MedkitItemData data = item.physicalData as MedkitItemData;
		Character character = Player.instance.character;
		for(int counter = 0; counter < data.addHealthSeconds * 2; counter++) {
			if (character != null) {
				yield return new WaitForSeconds (0.5f);
				int health = character.Health;
				health += data.addHealthPerHalfSecond;
				if (health > character.MaxHealth) {
					health = character.MaxHealth;
				}
				character.Health = health;
			}
		}
	}

	public void UpdateInfoAfterUse() {
		MedkitItemData data = item.physicalData as MedkitItemData;
		int distance = 999999;
		foreach (MedkitUIIconData iconData in data.icons) {
			int distanceNow = currentUse - iconData.applyOnUse;
			if (distanceNow < 0) {
				continue;
			}
			if (distanceNow < distance) {
				distance = distanceNow;
				item.icon.sprite = SpriteManager.GetSprite (iconData.spriteName);
			}
		}
		distance = 999999;
		foreach (MedkitUIAnimationData animationData in data.animations) {
			int distanceNow = currentUse - animationData.applyOnUse;
			if (distanceNow < 0) {
				continue;
			}
			if (distanceNow < distance) {
				distance = distanceNow;
				currentTrigger = animationData.triggerName;
				currentModel = animationData.modelName;
			}
		}
		UpdateText ();
	}

	EventData OnItemUse(EventData data) {
		if (currentUse <= 0) {
			return new EventData();
		}
		GUIItem dataItem = data.Get<GUIItem> (0);
		if (dataItem == item) {
			currentUse--;
			AnimatedItemPlayer.PlayAnimation (currentModel, currentTrigger);
			UpdateInfoAfterUse ();
			Player.instance.StartCoroutine (AddHealth ());
		}
		return new EventData ();
	}

	void Start() {
		if (currentUse == -1) {
			MedkitItemData data = item.physicalData as MedkitItemData;
			currentUse = data.maxUses;
		}

		UpdateInfoAfterUse ();

		EventManager.AddEventListener<Events.GUIItemActionSimple.Drop> (OnItemDrop);
		EventManager.AddEventListener<Events.GUIItemActionUsable.Use> (OnItemUse);
	}

}
