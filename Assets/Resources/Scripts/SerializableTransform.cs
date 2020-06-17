using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class SerializableTransformData {
	public Vector3 position;
	public int counter;
	public Quaternion rotation;
	public Vector3 localScale;
	public string parent;
}	

public class SerializableTransform : SerializableMonoBehaviour {

	public static int counter = 0;

	void Awake() {
		saveName += counter;
		counter++;
	}

	public override SerializableData GetSerializableData () {
		SerializableData rawData = base.GetSerializableData ();
		SerializableTransformData data = new SerializableTransformData ();
		data.position = transform.localPosition;
		data.counter = counter;
		data.rotation = transform.localRotation;
		data.localScale = transform.localScale;
		Transform parentTransform = transform.parent;
		if (parentTransform != null) {
			SerializableTransform parentSerializable = parentTransform.GetComponent<SerializableTransform> ();
			if (parentSerializable != null) {
				data.parent = parentSerializable.saveName;
			}
		}
		rawData.target = data;
		rawData.type = typeof(SerializableTransform);
		return rawData;
	}

	public override void SetSerializableData (SerializableData rawData) {
		SerializableTransformData data = ConvertTargetObject <SerializableTransformData> (rawData.target);
		if (data.parent != null) {
			SerializableTransform parentTransform = Serializer.GetComponent (data.parent) as SerializableTransform;
			//if (parentTransform != null) {
				transform.SetParent (parentTransform.transform, false);
			//}
		}
		transform.localPosition = data.position;
		counter = data.counter;
		transform.localRotation = data.rotation;
		transform.localScale = data.localScale;
	}

}
