using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponData { 
	public string name;
	public GameObject firstPerson;
	public GameObject thirdPerson;
}

public class WeaponManager : MonoBehaviour {

	public static WeaponManager instance;
	public string firstPersonWeaponPrefabsFolder;
	public string thirdPersonWeaponPrefabsFolder;
	public Dictionary<string, WeaponData> weapons = new Dictionary<string, WeaponData>();

	void Awake () {
		Dictionary<string, GameObject> firstPersonPrefabs = new Dictionary<string, GameObject> ();
		Dictionary<string, GameObject> thirdPersonPrefabs = new Dictionary<string, GameObject> ();

		GameObject[] objects = Resources.LoadAll (firstPersonWeaponPrefabsFolder, typeof(GameObject)).Cast<GameObject>().ToArray();
		foreach (GameObject obj in objects) {
			firstPersonPrefabs [obj.name] = obj;
			Debug.Log ("[WeaponManager] Loaded first person weapon prefab: " + obj.name);
		}

		objects = Resources.LoadAll (thirdPersonWeaponPrefabsFolder, typeof(GameObject)).Cast<GameObject>().ToArray();
		foreach (GameObject obj in objects) {
			thirdPersonPrefabs [obj.name] = obj;
			Debug.Log ("[WeaponManager] Loaded third person weapon prefab: " + obj.name);
		}

		foreach (KeyValuePair<string, GameObject> firstPersonPrefabPair in firstPersonPrefabs) {
			GameObject thirdPersonPrefab;
			if (thirdPersonPrefabs.TryGetValue (firstPersonPrefabPair.Key, out thirdPersonPrefab)) {
				WeaponData data = new WeaponData ();
				data.name = firstPersonPrefabPair.Key;
				data.firstPerson = firstPersonPrefabPair.Value;
				data.thirdPerson = thirdPersonPrefab;
				Debug.Log ("[WeaponManager] Registered weapon: " + data.name);
				weapons [data.name] = data;
			} else {
				Debug.LogError ("[WeaponManager] Cant create weapon data, third person version is not exist: " + firstPersonPrefabPair.Key);
			}
		}

		instance = this;
	}

	public static WeaponData GetWeaponData(string name) {
		WeaponData data;
		if(instance.weapons.TryGetValue(name, out data)) {
			return data;
		}
		Debug.LogError ("[WeaponManager] Cant find weapon data: " + name);
		return null;
	}


}
