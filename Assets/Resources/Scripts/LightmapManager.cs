using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightmapManager : MonoBehaviour {

	public static LightmapManager instance;

	public List<LightmapData> rememberedData = new List<LightmapData> ();
	public List<Light> bakedLights = new List<Light> ();
	public List<Light> realtimeLightsFromBaked = new List<Light>();

	void Awake () {
		rememberedData.AddRange (LightmapSettings.lightmaps);
		instance = this;
		Light[] lights = FindObjectsOfType<Light> ();
		foreach (Light light in lights) {
			if (light.isBaked) {
				bakedLights.Add (light);
				GameObject obj = Instantiate (light.gameObject);
				Light realtimeLight = obj.GetComponent<Light> ();
				realtimeLight.lightmapBakeType = LightmapBakeType.Realtime;
				realtimeLight.enabled = false;
				realtimeLightsFromBaked.Add (realtimeLight);
			}
		}
	}

	public void DisableLightmaps() {
		LightmapSettings.lightmaps = new LightmapData[0];
		foreach (Light light in realtimeLightsFromBaked) {
			//light.lightmapBakeType = LightmapBakeType.Realtime;
			light.enabled = true;
		}
	}

	public void EnableLightmaps() {
		LightmapSettings.lightmaps = rememberedData.ToArray ();
		foreach (Light light in realtimeLightsFromBaked) {
			light.enabled = false;
		}
	}

}
