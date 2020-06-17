using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicLightSwitcher : MonoBehaviour {
	public Light staticLight;
	public Light dynamicLight;

	public void EnableStaticLighting() {
		staticLight.enabled = true;
		dynamicLight.enabled = false;
	}

	public void EnableDynamicLighting() {
		staticLight.enabled = false;
		dynamicLight.enabled = true;
	}
}
