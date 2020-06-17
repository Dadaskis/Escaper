using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ResourceModelLoader : MonoBehaviour {

	public static ResourceModelLoader instance;

	public Dictionary<string, GameObject> models = new Dictionary<string, GameObject> ();

	void LoadAllModels(string path) {
		DirectoryInfo dirInfo = new DirectoryInfo(path);
		FileInfo[] filesInfo = dirInfo.GetFiles ();
		foreach (FileInfo fileInfo in filesInfo) {
			Debug.Log(fileInfo.FullName);
			GameObject obj = OBJLoader.LoadOBJFile(fileInfo.FullName);
			models [fileInfo.Name] = obj;
			obj.transform.parent = transform;
			obj.SetActive (false);
			obj.name = fileInfo.Name;
			MeshRenderer[] renderers = obj.GetComponentsInChildren<MeshRenderer> ();
			foreach (MeshRenderer renderer in renderers) {
				Material[] materials = renderer.sharedMaterials;
				int index = 0;
				foreach (Material material in materials) {
					Material validMaterial = ResourceMaterialLoader.GetMaterial (material.name);
					if (validMaterial != null) {
						materials [index] = validMaterial;
					}
					index++;
				}
				renderer.sharedMaterials = materials;
			}
		}
		DirectoryInfo[] directoriesInfo = dirInfo.GetDirectories ();
		if (directoriesInfo != null && directoriesInfo.GetLength (0) > 0) {
			foreach (DirectoryInfo directoryInfo in directoriesInfo) {
				LoadAllModels (directoryInfo.FullName);
			}
		}
	}

	IEnumerator LoadAllAfterMaterials() {
		while (!GlobalLuaExecutor.GetInstance().materialsLoaded) {
			yield return null;
		}
		LoadAllModels ("GameData/Models");
		instance = this;
	}

	void Start () {
		StartCoroutine (LoadAllAfterMaterials ());
	}

}
