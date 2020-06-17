using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializableMonoBehaviour : MonoBehaviour {

	public string saveName = "";
	public string prefabName = "";
	public bool idFromRoot = true;

	public T ConvertTargetObject<T>(object target) {
		var obj = target as Newtonsoft.Json.Linq.JObject;
		if (obj == null) {
			return (T) target;
		}
		return obj.ToObject<T> ();
	}

	public virtual SerializableData GetSerializableData() { 
		SerializableData data = new SerializableData ();
		data.saveName = saveName;
		data.prefabName = prefabName;
		if (idFromRoot) {
			data.id = transform.root.gameObject.GetInstanceID ();
		} else {
			data.id = gameObject.GetInstanceID ();
		}
		return data;
	}

	public virtual void SetSerializableData(SerializableData data) {}

}
