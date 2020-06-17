using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectInOneFrame : MonoBehaviour {

	public GameObject enableObject;

	IEnumerator EnableInOneFrame() {
		enableObject.SetActive (true);
		yield return new WaitForEndOfFrame ();
		enableObject.SetActive (false);
	}

	void Awake() { 
		StartCoroutine (EnableInOneFrame ());
	}

}
