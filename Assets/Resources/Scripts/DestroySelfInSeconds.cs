using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfInSeconds : MonoBehaviour {

	public float seconds = 0.3f;

	void Start () {
		Destroy (gameObject, seconds);
	}
}
