using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceSpawnObject : MonoBehaviour {

	public int chance = 50;
	public GameObject spawnObject;

	private float timer = 0.0f;
	private float targetTime = 1.0f;
	private bool canCreate = false;

	void Start () {
		if (Random.Range (0, 100) <= chance) {
			//Instantiate (spawnObject, this.transform);
			targetTime = Random.Range (0.5f, 2.0f);
		} else {
			Destroy (this.gameObject);
		}
	}

	void Update() {
		timer += Time.deltaTime;
		if (timer > targetTime) {
			GameObject spawned = Instantiate (spawnObject, transform);
			spawned.transform.SetParent (null);
			Destroy (this.gameObject);
		}
	}

	/*void Start() {
		if (Random.Range (0, 100) <= chance) {
			GameObject spawned = Instantiate (spawnObject, transform);
			spawned.transform.SetParent (null);
			Destroy (this.gameObject);
		}
	}*/
}
