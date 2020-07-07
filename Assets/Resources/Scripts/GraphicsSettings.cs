using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GraphicsSettingsData {
	public bool isFastMode = false;
	public bool bloomEnabled = true;
	public bool vignetteEnabled = true;
	public bool grainEnabled = true;
	public bool chromaticAberrationEnabled = true;
	public int textureQuality = 0;
	public bool enableRealtimeShadows = false;
	public bool enableAmbientOcclusion = false;
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
			} else {
				AmbientOcclusion ambientOcclusion = settings.GetSetting<AmbientOcclusion> ();
				ambientOcclusion.active = false;
			}

			QualitySettings.masterTextureLimit = data.textureQuality;

			//Light[] lights = FindObjectsOfType<Light> ();
			//foreach (Light light in lights) {
			//	if (light.type == LightType.Point) {
			//		light.enabled = data.enableRealtimeShadows;
			//	}
			//}

			if (!data.isFastMode) {
				DynamicLightSwitcher[] switchers = FindObjectsOfType<DynamicLightSwitcher> ();
				foreach (DynamicLightSwitcher switcher in switchers) {
					if (data.enableRealtimeShadows) {
						switcher.EnableDynamicLighting ();
					} else {
						switcher.EnableStaticLighting ();
					}
				}

				if (data.enableRealtimeShadows) {
					QualitySettings.shadows = ShadowQuality.All;
				} else {
					QualitySettings.shadows = ShadowQuality.Disable;
				}

				MaterialManager.instance.ChangeQuality (data.shadersQuality);
			}
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
	}

}
