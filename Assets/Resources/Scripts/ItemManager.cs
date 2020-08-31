using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemManager : MonoBehaviour {

	public static ItemManager instance;

	public Dictionary<string, ItemData> items = new Dictionary<string, ItemData> ();
	public List<string> itemsFoldersToCheck;

	void Awake() {
		foreach (string folderToCheck in itemsFoldersToCheck) {
			ItemData[] objects = Resources.LoadAll (folderToCheck, typeof(ItemData)).Cast<ItemData> ().ToArray ();
			foreach (ItemData obj in objects) {
				items [obj.name] = obj;
				Debug.Log ("[ItemManager] Loaded item: " + obj.name);
			}
		}
		instance = this;
	}

	public static ItemData GetItem(string name) {
		ItemData item;
		if(instance.items.TryGetValue(name, out item)) {
			return item;
		} else {
			Debug.LogError("[ItemManager] Cant get item: " + name);
		}
		return null;
	}
}
