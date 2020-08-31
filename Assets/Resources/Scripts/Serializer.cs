using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData : Dictionary<string, SerializableData> {}

[System.Serializable]
public class SerializableSaveData {
	public SaveData data = new SaveData();
}

public class Serializer : MonoBehaviour {

	public static Serializer instance;

	public string[] prefabFoldersToCheck;
	public Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject> ();
	public Dictionary<string, ScriptableObject> scriptableObjects = new Dictionary<string, ScriptableObject>();
	public Dictionary<string, SaveData> saves = new Dictionary<string, SaveData>();
	public Dictionary<string, SerializableMonoBehaviour> componentBySaveName = new Dictionary<string, SerializableMonoBehaviour>();

	public static SerializableMonoBehaviour GetComponent(string saveName) {
		SerializableMonoBehaviour component;
		if (instance.componentBySaveName.TryGetValue (saveName, out component)) { 
			return component;
		}
		Debug.LogError ("[Serializer] Get component returns null: " + saveName);
		return null;
	}

	public static ScriptableObject GetScriptableObject(string name) {
		ScriptableObject obj;
		if (instance.scriptableObjects.TryGetValue (name, out obj)) { 
			return obj;
		}
		Debug.LogError ("[Serializer] Get scriptable object returns null: " + name);
		return null;
	}

	public static GameObject GetPrefab(string name) {
		GameObject obj;
		if (instance.prefabs.TryGetValue (name, out obj)) { 
			return obj;
		}
		Debug.LogError ("[Serializer] Get game object returns null: " + name);
		return null;
	}

	void Start() {
		foreach (string folderToCheck in prefabFoldersToCheck) {
			GameObject[] objects = Resources.LoadAll (folderToCheck, typeof(GameObject)).Cast<GameObject> ().ToArray ();
			foreach (GameObject obj in objects) {
				prefabs [obj.name] = obj;
				Debug.Log ("[Serializer] Loaded prefab: " + obj.name);
			}
		}

		ScriptableObject[] scriptableObjectsArray = Resources.LoadAll ("", typeof(ScriptableObject)).Cast<ScriptableObject>().ToArray();
		foreach (ScriptableObject obj in scriptableObjectsArray) {
			scriptableObjects [obj.name] = obj;
			Debug.Log ("[Serializer] Loaded scriptable object: " + obj.name);
		}

		Deserialize ();
		instance = this;
	}

	void Update() {
		//if (Input.GetKeyDown (KeyCode.LeftBracket)) {
		//	Save ("Test");
		//} else if (Input.GetKeyDown (KeyCode.RightBracket)) {
		//	StartCoroutine(Load ("Test"));
		//}
	}

	public void Serialize() {
		foreach (string key in saves.Keys) {
			Debug.Log ("[Serializer] Serializating save slot: " + key);
			SaveData data = saves [key];
			SerializableSaveData save = new SerializableSaveData ();
			save.data = data;
			string json = JsonConvert.SerializeObject(save, Formatting.Indented);

			File.WriteAllText ("Saves/" + key + ".save", json);
		}
	}

	public void Deserialize() {
		Debug.Log ("[Serializer] Deserializing");
		DirectoryInfo dirInfo = new DirectoryInfo ("Saves");
		FileInfo[] filesInfo = dirInfo.GetFiles ();
		foreach (FileInfo fileInfo in filesInfo) {
			if (fileInfo.Extension == ".save") {
				Debug.Log ("[Serializer] Deserializing, processing file: " + fileInfo.Name);
				string json = File.ReadAllText ("Saves/" + fileInfo.Name);
				SerializableSaveData save = JsonConvert.DeserializeObject<SerializableSaveData> (json);
				string name = "";
				string[] splits = fileInfo.Name.Split ('.');
				for(int index = 0; index < splits.GetLength(0) - 1; index++) {
					name += splits [index];
				}
				
				saves [name] = save.data;
			}
		}
	}

	public void Save(string name) {
		Debug.Log ("[Serializer] Creating save: " + name);
		SaveData allData = new SaveData ();

		List<SerializableMonoBehaviour> objects 
			= ResourceUtility.FindAllObjectsOfType<SerializableMonoBehaviour> ();

		foreach (SerializableMonoBehaviour obj in objects) {
			SerializableData data = obj.GetSerializableData ();
			allData[data.saveName] = data;
		}
		saves [name] = allData;

		Serialize ();
	}

	public IEnumerator Load(string name) {
		SaveData allData;
		if (saves.TryGetValue (name, out allData)) {
			Debug.Log ("[Serializer] Loading save: " + name);

			List<SerializableMonoBehaviour> rawComponents = ResourceUtility.FindAllObjectsOfType<SerializableMonoBehaviour> ();
			List<SerializableMonoBehaviour> components = new List<SerializableMonoBehaviour> ();
			components.AddRange (rawComponents);

			Debug.Log ("[Serializer] Prefab creating needed for " + (allData.Count - components.Count) + " MonoBehaviours");
		
			Dictionary<int, List<SerializableData>> gameObjectDataList = new Dictionary<int, List<SerializableData>> ();
			Dictionary<int, bool> IDChecked = new Dictionary<int, bool> ();
			Dictionary<string, List<int>> prefabCounts = new Dictionary<string, List<int>> ();

			foreach (KeyValuePair<string, SerializableData> dataPair in allData) {
				SerializableData rawData = dataPair.Value;
				List<SerializableData> dataList;
				if (gameObjectDataList.TryGetValue (rawData.id, out dataList)) {
					dataList.Add (rawData);
				} else {
					dataList = new List<SerializableData> ();
					dataList.Add (rawData);
					gameObjectDataList [rawData.id] = dataList;
				}
			}

			foreach (KeyValuePair<string, SerializableData> dataPair in allData) {
				SerializableData rawData = dataPair.Value;
				bool isChecked;
				if(!IDChecked.TryGetValue (rawData.id, out isChecked)) {
					List<int> prefabCount;
					if (prefabCounts.TryGetValue (rawData.prefabName, out prefabCount)) {
						prefabCount.Add (rawData.id);
					} else {
						prefabCount = new List<int> ();
						prefabCount.Add(rawData.id);
						prefabCounts [rawData.prefabName] = prefabCount;
					}
					IDChecked [rawData.id] = true;
				}
			}

			IDChecked.Clear ();
			foreach (SerializableMonoBehaviour component in components) {
				SerializableData rawData;
				if (allData.TryGetValue (component.saveName, out rawData)) {
					bool isChecked;
					if(!IDChecked.TryGetValue (rawData.id, out isChecked)) {
						List<int> prefabCount;
						if (prefabCounts.TryGetValue (rawData.prefabName, out prefabCount)) {
							prefabCount.Remove(rawData.id);
						} else {
							prefabCounts [rawData.prefabName] = new List<int> ();
						}
						IDChecked [rawData.id] = true;
					}
				}
			}

			componentBySaveName.Clear ();

			foreach (KeyValuePair<string, List<int>> prefabCountPair in prefabCounts) {
				for (int counter = 0; counter < prefabCountPair.Value.Count; counter++) {
					GameObject prefab;
					if (prefabs.TryGetValue (prefabCountPair.Key, out prefab)) {
						GameObject obj = Instantiate (prefab);
						Debug.Log ("[Serializer] Instantiating prefab: " + prefabCountPair.Key);
						int ID = prefabCountPair.Value [counter];
						List<SerializableData> dataList = gameObjectDataList [ID];
						foreach (SerializableData rawData in dataList) {
							try {
								SerializableMonoBehaviour component = obj.GetComponent (rawData.type) as SerializableMonoBehaviour;
								component.saveName = rawData.saveName;
							} catch (System.Exception ex) {
								Debug.LogError (ex);
							}
						}
					} else {
						Debug.LogError ("[Serializer] Cant instantiate prefab: " + prefabCountPair.Key);
					}
				}
			}

			rawComponents = ResourceUtility.FindAllObjectsOfType<SerializableMonoBehaviour> ();
			components = new List<SerializableMonoBehaviour> ();
			components.AddRange (rawComponents);

			foreach (SerializableMonoBehaviour component in components) {
				componentBySaveName [component.saveName] = component;
			}

			yield return new WaitForEndOfFrame ();

			foreach (KeyValuePair<string, SerializableMonoBehaviour> dataPair in componentBySaveName) {
				dataPair.Value.saveName = dataPair.Key;
			}

			foreach (SerializableMonoBehaviour component in components) {
				SerializableData rawData;
				Debug.Log ("[Serializer] Checking component: " + component + ". SaveName: " + component.saveName);
				if (allData.TryGetValue (component.saveName, out rawData)) {
					Debug.Log ("[Serializer] Assigning component data: " + component);
					Debug.Log ("[Serializer] Component game object: " + component.gameObject.name);
					try {
						component.SetSerializableData (rawData);
					} catch (System.Exception ex) {
						Debug.LogError (ex);
					}
				} else {
					Debug.LogError ("[Serializer] Destroying game object: " + component.gameObject.name);
					Destroy (component.gameObject);
				}
			}
		} else {
			Debug.LogError ("[Serializer] Failed when loading save: " + name);
		}
	}

}
