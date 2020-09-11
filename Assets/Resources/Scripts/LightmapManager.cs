using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightmapManager : MonoBehaviour {

	public static LightmapManager instance;

	public List<LightmapData> rememberedData = new List<LightmapData> ();
	public List<Light> bakedLights = new List<Light> ();
	public List<Light> realtimeLightsFromBaked = new List<Light>();

	public void Initialize() {
		rememberedData = new List<LightmapData> ();
		bakedLights = new List<Light> ();
		realtimeLightsFromBaked = new List<Light>();
		rememberedData.AddRange (LightmapSettings.lightmaps);
		Light[] lights = FindObjectsOfType<Light> ();
		foreach (Light light in lights) {
			if (light.isBaked) {
				bakedLights.Add (light);
				GameObject obj = Instantiate (light.gameObject);
				Light realtimeLight = obj.GetComponent<Light> ();
				//realtimeLight.lightmapBakeType = LightmapBakeType.Realtime;
				//realtimeLight.
				realtimeLight.bounceIntensity = light.bounceIntensity;
				realtimeLight.color = light.color;
				realtimeLight.intensity = light.intensity;
				realtimeLight.range = light.range;
				realtimeLight.shadows = light.shadows;
				realtimeLight.spotAngle = light.spotAngle;
				realtimeLight.type = light.type;
				realtimeLight.enabled = false;
				realtimeLightsFromBaked.Add (realtimeLight);
			}
		}
	}

	EventData InitializeDataOnNewLocation(EventData data) {
		Initialize ();
		return new EventData ();
	}

	void Awake () {
		instance = this;
		Initialize ();
		EventManager.AddEventListener<Events.GameLogic.NewLocationLoaded> (InitializeDataOnNewLocation);
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
