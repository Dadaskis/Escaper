using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class GUIInventoryData {

	public List<string> containers = new List<string>();
	public List<string> slots = new List<string>();

}

[RequireComponent(typeof(SerializableTransform))]
public class GUIInventory : SerializableMonoBehaviour {

	public List<GUIContainer> containers;
	public List<GUIInventorySlots> slots;
	public GUIInventoryData inventory;
	public RectTransform dropOutTransform;
	public GUIItemDropOutWindow dropOutWindow;

	private Canvas canvas;

	void Start() {
		canvas = GetComponentInParent<Canvas> ();
		SerializableTransform transform = GetComponent<SerializableTransform> ();
		saveName += transform.saveName + "Inventory";
	}

	void Update() {
		if (InputManager.GetButtonDown ("PlayerSight") == true) {
			List<RaycastResult> raycastResults = GUICustomUtility.RaycastMouse ();

			foreach (RaycastResult raycast in raycastResults) {
				GUIItem item = raycast.gameObject.GetComponent<GUIItem> ();
				if (item != null) {
					Transform previousParent = dropOutTransform.parent;
					dropOutTransform.SetParent (null, true);
					dropOutTransform.SetParent (previousParent, true);

					dropOutTransform.gameObject.SetActive (true);
					Vector2 mousePosition = Input.mousePosition;
					mousePosition.x -= Screen.width * 0.5f;
					mousePosition.y -= Screen.height * 0.5f;
					mousePosition /= canvas.scaleFactor;
					dropOutTransform.anchoredPosition = mousePosition;

					dropOutWindow.Work (GUIDropOutActionRegister.Get(item.actionsType), item);
				}
			}
		} else if(InputManager.GetButtonDown ("PlayerShoot") == true) {
			dropOutTransform.gameObject.SetActive (false);
		}
	}

	public override SerializableData GetSerializableData () {
		SerializableData rawData = base.GetSerializableData ();
		GUIInventoryData data = new GUIInventoryData ();

		foreach (GUIContainer container in containers) {
			data.containers.Add (container.saveName);
		}

		foreach (GUIInventorySlots slots in slots) {
			data.slots.Add (slots.saveName);
		}

		rawData.type = typeof(GUIInventory);
		rawData.target = data;
		return rawData;
	}

	public override void SetSerializableData (SerializableData rawData) {
		GUIInventoryData data = ConvertTargetObject<GUIInventoryData> (rawData.target);

		foreach (string containerSaveName in data.containers) {
			GUIContainer container = Serializer.GetComponent (containerSaveName) as GUIContainer;
			containers.Add (container);
			container.inventory = this;
		}
	}

}
