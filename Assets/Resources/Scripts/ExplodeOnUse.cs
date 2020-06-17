using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnUse : MonoBehaviour {

	public GameObject explosion;
	public DestroyableObject destroyObject;
	public bool exploded = false;

	public void Use() {
		if (!exploded) {
			Instantiate (explosion, destroyObject.transform.position, Quaternion.identity);
			destroyObject.Damage (destroyObject.health);
			exploded = true;
		}
	}

}
