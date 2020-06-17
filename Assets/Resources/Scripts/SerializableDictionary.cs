using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableKeyValues<TKey, TValue> {
	public List<TKey> keys = new List<TKey>();
	public List<TValue> values = new List<TValue>();
}

public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue> {
	public SerializableKeyValues<TKey, TValue> GetSerializableKeyValues() {
		SerializableKeyValues<TKey, TValue> keyValues = new SerializableKeyValues<TKey, TValue>();
		foreach (KeyValuePair<TKey, TValue> pair in this) {
			keyValues.keys.Add (pair.Key);
			keyValues.values.Add (pair.Value);
		}
		return keyValues;
	}

	public void SetFromSerializableKeyValues(SerializableKeyValues<TKey, TValue> data) {
		this.Clear ();
		if (data.keys.Count != data.values.Count) {
			Debug.LogError ("WTF man, values count is not equal to keys count. Maybe some types are fucked up for serialization");
			return;
		}

		for (int index = 0; index < data.keys.Count; index++) {
			this.Add (data.keys [index], data.values [index]);
		}
	}
}
