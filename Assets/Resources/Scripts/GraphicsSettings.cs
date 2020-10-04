using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Newtonsoft.Json;

[System.Serializable]
public class GraphicsSettingsData {
	public bool isFastMode = false;
	public bool bloomEnabled = true;
	public bool vignetteEnabled = true;
	public bool grainEnabled = true;
	public bool chromaticAberrationEnabled = true;
	public int textureQuality = 0;
	public bool enableRealtimeShadows = false;
	public bool enableAmbientOcclusion = false;
	public bool enableVolumetricLighting = false;
	public ShaderQuality shadersQuality = ShaderQuality.LOW;
	public PostProcessLayer.Antialiasing antialiasing = PostProcessLayer.Antialiasing.None;
	public bool isHDREnabled = false;
}

public class GraphicsSettings : MonoBehaviour {	

	public static GraphicsSettings instance;

	public PostProcessProfile settings;
	public PostProcessLayer layer;

	private GraphicsSettingsData data = new GraphicsSettingsData();

	public GraphicsSettingsData Data {
		set {
			if (layer == null) {
				layer = FindObjectOfType<PostProcessLayer> ();
			}

			data = value;

			layer.antialiasingMode = data.antialiasing;

			Camera.main.allowHDR = data.isHDREnabled;

			Bloom bloom = settings.GetSetting<Bloom> ();
			bloom.active = data.bloomEnabled;

			Vignette vignette = settings.GetSetting<Vignette> ();
			vignette.active = data.vignetteEnabled;

			Grain grain = settings.GetSetting<Grain> ();
			grain.active = data.grainEnabled;

			ChromaticAberration chromaticAberration = settings.GetSetting<ChromaticAberration> ();
			chromaticAberration.active = data.chromaticAberrationEnabled;

			if (!data.isFastMode) {
				AmbientOcclusion ambientOcclusion = settings.GetSetting<AmbientOcclusion> ();
				ambientOcclusion.active = data.enableAmbientOcclusion;

				if (data.enableRealtimeShadows) {
					VolumetricLightRenderer volumetricLight = Camera.main.GetComponent<VolumetricLightRenderer> ();
					volumetricLight.enabled = data.enableVolumetricLighting;
				}
			} else {
				AmbientOcclusion ambientOcclusion = settings.GetSetting<AmbientOcclusion> ();
				ambientOcclusion.active = false;

				VolumetricLightRenderer volumetricLight = Camera.main.GetComponent<VolumetricLightRenderer> ();
				volumetricLight.enabled = false;
			}

			if (data.isFastMode && 
					(!data.bloomEnabled 
					&& !data.chromaticAberrationEnabled
					&& !data.grainEnabled 
					&& !data.vignetteEnabled 
					&& data.antialiasing == PostProcessLayer.Antialiasing.None)
			) {
				PostProcessLayer layer = FindObjectOfType<PostProcessLayer> ();
				if (layer != null) {
					layer.enabled = false;
				}
			} else {
				PostProcessLayer layer = FindObjectOfType<PostProcessLayer> ();
				if (layer != null) {
					layer.enabled = true;
				}
			}

			QualitySettings.masterTextureLimit = data.textureQuality;

			//Light[] lights = FindObjectsOfType<Light> ();
			//foreach (Light light in lights) {
			//	if (light.type == LightType.Point) {
			//		light.enabled = data.enableRealtimeShadows;
			//	}
			//}

			/*DynamicLightSwitcher[] switchers = FindObjectsOfType<DynamicLightSwitcher> ();
			foreach (DynamicLightSwitcher switcher in switchers) {
				if (data.enableRealtimeShadows) {
					switcher.EnableDynamicLighting ();
				} else {
					switcher.EnableStaticLighting ();
				}
			}*/

			Light[] lights = FindObjectsOfType<Light> ();

			if (data.enableRealtimeShadows && !data.isFastMode) {
				QualitySettings.shadows = ShadowQuality.All;
				foreach (Light light in lights) {
					if (light.isBaked) {
						light.enabled = true;
					}
				}
				LightmapManager.instance.DisableLightmaps ();
			} else {
				QualitySettings.shadows = ShadowQuality.Disable;
				foreach (Light light in lights) {
					if (light.isBaked) {
						light.enabled = false;
					}
				}
				LightmapManager.instance.EnableLightmaps ();
			}

			MaterialManager.instance.ChangeQuality (data.shadersQuality);

			Save ();
		}

		get {
			return data;
		}
	}

	/*public static void CheckLights() {
		Light[] lights = FindObjectsOfType<Light> ();
		foreach (Light light in lights) {
			if (light.type == LightType.Point) {
				light.enabled = instance.data.enableRealtimeShadows;
			}
		}
	}*/

	void Awake() {
		instance = this;
		Load ();
	}

	public void Save() {
		try {
			string json = JsonConvert.SerializeObject(data);
			System.IO.File.WriteAllText("Saves/Graphics.settings", json);
		} catch(System.Exception ex) { 
			// Do you know that we can kill 90% of humans using exceptions?
		}
	}

	public void Load() {
		try {
			string json = System.IO.File.ReadAllText("Saves/Graphics.settings");
			Data = JsonConvert.DeserializeObject<GraphicsSettingsData>(json);
		} catch(System.Exception ex) { 
			// Or maybe 100%! 
		}
	}

}
