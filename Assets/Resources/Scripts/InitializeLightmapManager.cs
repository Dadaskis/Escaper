using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeLightmapManager : MonoBehaviour {
	
	void Start () {
		LightmapManager.instance.Initialize ();
	}

}
