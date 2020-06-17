using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceRemoveObject : MonoBehaviour {

	public float chance = 50.0f;

	void Start () {
		if (Random.Range (0, 100) > chance) {
			Destroy (this.gameObject);
		}
	}
}
