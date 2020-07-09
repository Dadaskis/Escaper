using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class GUIInitializeRenderSettings : MonoBehaviour {

	public Dropdown rendererModeUI;
	public GUIFastModeRendererApply fastModeApply;
	public GUIAdvancedRendererApply advancedModeApply;

	void Start() {
		GraphicsSettingsData data = GraphicsSettings.instance.Data;

		if (data.isFastMode) {
			rendererModeUI.value = 0;
		} else {
			rendererModeUI.value = 1;
		}
		rendererModeUI.RefreshShownValue ();
		rendererModeUI.onValueChanged.Invoke (0);

		switch (data.antialiasing) {
		case PostProcessLayer.Antialiasing.None:
			fastModeApply.antialiasingApplied.quality = 0;
			break;
		case PostProcessLayer.Antialiasing.FastApproximateAntialiasing:
			fastModeApply.antialiasingApplied.quality = 1;
			break;
		case PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing:
			fastModeApply.antialiasingApplied.quality = 2;
			break;
		}
		fastModeApply.bloomActive.isOn = data.bloomEnabled;
		fastModeApply.chromaticAberrationActive.isOn = data.chromaticAberrationEnabled;
		fastModeApply.grainActive.isOn = data.grainEnabled;
		fastModeApply.isHDRActive.isOn = data.isHDREnabled;
		fastModeApply.texturesQuality.quality = data.textureQuality - 1;
		fastModeApply.vignetteActive.isOn = data.vignetteEnabled;

		try {
			fastModeApply.texturesQuality.UpdateText ();
		} catch(System.Exception ex) {
			// In one time, it will fail and fuck everything up
		}

		switch (data.antialiasing) {
		case PostProcessLayer.Antialiasing.None:
			advancedModeApply.antialiasingApplied.quality = 0;
			break;
		case PostProcessLayer.Antialiasing.FastApproximateAntialiasing:
			advancedModeApply.antialiasingApplied.quality = 1;
			break;
		case PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing:
			advancedModeApply.antialiasingApplied.quality = 2;
			break;
		}
		advancedModeApply.bloomActive.isOn = data.bloomEnabled;
		advancedModeApply.chromaticAberrationActive.isOn = data.chromaticAberrationEnabled;
		advancedModeApply.grainActive.isOn = data.grainEnabled;
		advancedModeApply.isHDRActive.isOn = data.isHDREnabled;
		advancedModeApply.texturesQuality.quality = data.textureQuality - 1;
		advancedModeApply.vignetteActive.isOn = data.vignetteEnabled;
		advancedModeApply.ambientOcclusionActive.isOn = data.enableAmbientOcclusion;
		advancedModeApply.dynamicShadowsActive.isOn = data.enableRealtimeShadows;
		advancedModeApply.materialsQuality.quality = (int) data.shadersQuality;

		try {
			advancedModeApply.texturesQuality.UpdateText ();
		} catch(System.Exception ex) {
			// Its for textures... 
		}
		try { 
			advancedModeApply.materialsQuality.UpdateText ();
		} catch(System.Exception ex) {
			// ... and materials... maybe they will work, BUT I CANT CONTROL ANYTHING HERE
		}
	}

}
