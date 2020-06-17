using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializableRectTransformData {
	public Vector2 position;
	public Vector2 anchorMax;
	public Vector2 anchorMin;
	public Vector2 offsetMax;
	public Vector2 offsetMin;
	public Vector2 pivot;
	public Vector2 sizeDelta;
}

[RequireComponent(typeof(SerializableTransform))]
public class SerializableRectTransform : SerializableMonoBehaviour {

	RectTransform rectTransform;
	SerializableTransform transform;

	void Start () {
		transform = GetComponent<SerializableTransform> ();
		rectTransform = GetComponent<RectTransform> ();
		saveName += transform.saveName + "RectTransform";
	}

	public override SerializableData GetSerializableData () {
		SerializableData rawData = base.GetSerializableData ();
		SerializableRectTransformData data = new SerializableRectTransformData ();
		data.position = rectTransform.anchoredPosition;
		data.anchorMax = rectTransform.anchorMax;
		data.anchorMin = rectTransform.anchorMin;
		data.offsetMax = rectTransform.offsetMax;
		data.offsetMin = rectTransform.offsetMin;
		data.pivot = rectTransform.pivot;
		data.sizeDelta = rectTransform.sizeDelta;
		rawData.target = data;
		rawData.type = typeof(SerializableRectTransform);
		return rawData;
	}

	public override void SetSerializableData (SerializableData rawData) {
		SerializableRectTransformData data = ConvertTargetObject<SerializableRectTransformData> (rawData.target);
		rectTransform.anchoredPosition = data.position;
		rectTransform.anchorMax = data.anchorMax;
		rectTransform.anchorMin = data.anchorMin;
		rectTransform.offsetMax = data.offsetMax;
		rectTransform.offsetMin = data.offsetMin;
		rectTransform.pivot = data.pivot;
		rectTransform.sizeDelta = data.sizeDelta;
	}
}
