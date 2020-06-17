using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveEvent : MonoBehaviour {

	public GameObject removeObject;

	public void Work() {
		Destroy (removeObject);
		Destroy (this);
	}
}
