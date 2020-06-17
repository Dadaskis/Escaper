using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootBoxItem {
	public GameObject item;
	public int chance;
}

public class LootBox : MonoBehaviour {

	public Vector3 offset = Vector3.zero + new Vector3(0.0f, 0.5f, 0.0f);
	public int count = 1;
	public List<LootBoxItem> items;

	private bool used = false;

	public void Use() {
		if (!used) {
			Vector3 spawnPosition = transform.position + offset;
			Debug.Log ("Items: " + items.Count);
			for (int counter = 0; counter < count; counter++) {
				LootBoxItem item = items [Random.Range (0, items.Count)];
				Debug.Log ("Item: " + item.item.name);
				while(Random.Range(0, 100) > item.chance){
					item = items [Random.Range (0, items.Count)];
					Debug.Log ("Item: " + item.item.name);
				}
				GameObject spawned = Instantiate (item.item);
				spawned.transform.position = spawnPosition + (Random.insideUnitSphere / 5.0f);
			}
			used = true;
		}
	}
}
