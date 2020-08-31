using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class GUIFastModeRendererApply : MonoBehaviour {

	public Toggle bloomActive;
	public Toggle vignetteActive;
	public Toggle chromaticAberrationActive;
	public Toggle grainActive;
	public Toggle isHDRActive;
	public GUIQualityController texturesQuality;
	public GUIQualityController antialiasingApplied;

	public void Apply() {
		GraphicsSettingsData data = new GraphicsSettingsData ();
		data.isFastMode = true;
		data.bloomEnabled = bloomActive.isOn;
		data.vignetteEnabled = vignetteActive.isOn;
		data.chromaticAberrationEnabled = chromaticAberrationActive.isOn;
		data.grainEnabled = grainActive.isOn;
		data.isHDREnabled = isHDRActive.isOn;
		data.textureQuality = texturesQuality.quality;
		data.enableRealtimeShadows = false;
		switch (antialiasingApplied.quality) {
		case 0:
			data.antialiasing = PostProcessLayer.Antialiasing.None;
			break;
		case 1:
			data.antialiasing = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
			break;
		case 2:
			data.antialiasing = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
			break;
		}
		GraphicsSettings.instance.Data = data;
	}

}
