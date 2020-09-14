using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeLightmapManager : MonoBehaviour {

	IEnumerator InitializeNeededThings() {
		yield return new WaitForEndOfFrame ();
		yield return new WaitForEndOfFrame ();
		yield return new WaitForEndOfFrame ();
		LightmapManager.instance.Initialize ();
		MaterialManager.instance.Load ();
		GraphicsSettings.instance.Load ();
	}

	void Start () {
		StartCoroutine (InitializeNeededThings ());
	}

}
