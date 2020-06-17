using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class SerializableMaterialData {
	public List<string> materialsName = new List<string>();
}

[RequireComponent(typeof(SerializableTransform))]
public class SerializableMaterial : SerializableMonoBehaviour {

	private MeshRenderer renderer;
	private SerializableTransform transform;

	static bool initialized = false;
	static Dictionary<string, Material> materials = new Dictionary<string, Material>();
	static void InitializeMaterials() {
		if (!initialized) {
			initialized = true;
			Material[] objects = Resources.LoadAll ("", typeof(Material)).Cast<Material>().ToArray();
			foreach (Material material in objects) {
				materials [material.name] = material;
				Debug.Log ("[SerializableMaterial] Registered material " + material.name);
			}
		}
	}

	void Awake() {
		InitializeMaterials ();
	}

	void Start() {
		renderer = GetComponent<MeshRenderer> ();
		transform = GetComponent<SerializableTransform> ();
		saveName = transform.saveName + "Materials";
	}

	public override SerializableData GetSerializableData () {
		SerializableData rawData = base.GetSerializableData ();
		SerializableMaterialData data = new SerializableMaterialData();
		foreach(Material material in renderer.sharedMaterials) {
			data.materialsName.Add(material.name);
		}
		rawData.target = data;
		rawData.saveName = saveName;
		rawData.type = typeof(SerializableMaterial);
		return rawData;
	}

	public override void SetSerializableData (SerializableData rawData) {
		renderer = GetComponent<MeshRenderer> ();
		transform = GetComponent<SerializableTransform> ();
		SerializableMaterialData data = ConvertTargetObject<SerializableMaterialData>(rawData.target);
		int index = 0;
		Material[] sharedMaterials = renderer.sharedMaterials;
		foreach(string materialName in data.materialsName){
			sharedMaterials[index] = materials [materialName];
			index++;
		}
		renderer.sharedMaterials = sharedMaterials;
	}

}
