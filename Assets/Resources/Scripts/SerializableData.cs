using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableData {
	public string saveName = "";
	public string prefabName = "";
	public int id = -1;
	public object target;
	public System.Type type;
}
