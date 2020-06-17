using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public TrailRenderer trail;

	void Start() {
		trail.Clear ();	
		Destroy (gameObject, 10.0f);
	}

	void FixedUpdate () {
		transform.position += transform.forward * 2.0f;
	}
}
