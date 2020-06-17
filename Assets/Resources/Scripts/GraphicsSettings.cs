using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GraphicsSettingsData {
	public bool bloomEnabled = true;
	public bool vignetteEnabled = true;
	public bool grainEnabled = true;
	public bool chromaticAberrationEnabled = true;
	public bool colorGradingEnabled = true;
	public int textureQuality = 0;
	public bool enableRealtimeShadows = false;
	public bool enableAmbientOcclusion = false;
	public MaterialShaderQuality shadersQuality = MaterialShaderQuality.VERY_LOW;
}

public class GraphicsSettings : MonoBehaviour {	

	public static GraphicsSettings instance;

	public PostProcessProfile settings;

	private GraphicsSettingsData data = new GraphicsSettingsData();

	public GraphicsSettingsData Data {
		set {
			data = value;

			Bloom bloom = settings.GetSetting<Bloom> ();
			bloom.active = data.bloomEnabled;

			Vignette vignette = settings.GetSetting<Vignette> ();
			vignette.active = data.vignetteEnabled;

			Grain grain = settings.GetSetting<Grain> ();
			grain.active = data.grainEnabled;

			ChromaticAberration chromaticAberration = settings.GetSetting<ChromaticAberration> ();
			chromaticAberration.active = data.chromaticAberrationEnabled;

			ColorGrading colorGrading = settings.GetSetting<ColorGrading> ();
			colorGrading.active = data.colorGradingEnabled;

			AmbientOcclusion ambientOcclusion = settings.GetSetting<AmbientOcclusion> ();
			ambientOcclusion.active = data.enableAmbientOcclusion;

			QualitySettings.masterTextureLimit = data.textureQuality;

			//Light[] lights = FindObjectsOfType<Light> ();
			//foreach (Light light in lights) {
			//	if (light.type == LightType.Point) {
			//		light.enabled = data.enableRealtimeShadows;
			//	}
			//}
			DynamicLightSwitcher[] switchers = FindObjectsOfType<DynamicLightSwitcher>();
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

			MaterialSettingsManager.instance.ChangeQuality (data.shadersQuality);
		}

		get {
			return data;
		}
	}

	public static void CheckLights() {
		Light[] lights = FindObjectsOfType<Light> ();
		foreach (Light light in lights) {
			if (light.type == LightType.Point) {
				light.enabled = instance.data.enableRealtimeShadows;
			}
		}
	}

	void Awake() {
		instance = this;
	}

}
