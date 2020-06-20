using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObjectData {
	public bool opened = false;
}

[RequireComponent(typeof(SerializableTransform))]
public class DoorObject : IUsableObject {

	public bool opened = false;
	public Vector3 fromRotation;
	public Vector3 toRotation;
	public float openSpeed = 0.1f;

	void Start() {
		SerializableTransform transform = GetComponent<SerializableTransform> ();
		saveName += transform.saveName + "Door";
	}

	string ConvertInputKey(string input) {
		return System.Enum.GetName (typeof(KeyCode), InputManager.instance.keys [input]);
	}

	public override string ShowText () {
		return "[" + ConvertInputKey ("Use") + "] Open the door";
	}

	public override void Use () {
		opened = !opened;
	}

	public override SerializableData GetSerializableData () {
		SerializableData rawData = base.GetSerializableData ();
		DoorObjectData data = new DoorObjectData ();
		data.opened = opened;
		rawData.target = data;
		rawData.type = typeof(DoorObject);
		return rawData;
	}

	public override void SetSerializableData (SerializableData rawData) {
		DoorObjectData data = ConvertTargetObject<DoorObjectData> (rawData.target);
		opened = data.opened;
	}

	Dictionary<int, Collider> touchingColliders = new Dictionary<int, Collider>();

	void OnCollisionEnter(Collision collision) {
		if (!collision.gameObject.isStatic) {
			Collider collider;
			if (!touchingColliders.TryGetValue (collision.gameObject.GetHashCode (), out collider)) {
				touchingColliders [collision.gameObject.GetHashCode ()] = collision.collider;
			}
		}
	}

	void OnCollisionStay(Collision collision) {
		OnCollisionEnter (collision);
	}

	void FixedUpdate() {
		if (touchingColliders.Count == 0) {
			Quaternion rotation = transform.localRotation;
			if (!opened) {
				rotation = Quaternion.Lerp (rotation, Quaternion.Euler(fromRotation), openSpeed);
			} else {
				rotation = Quaternion.Lerp (rotation, Quaternion.Euler(toRotation), openSpeed);
			}
			transform.localRotation = rotation;
		}
		touchingColliders.Clear ();
	}

}
