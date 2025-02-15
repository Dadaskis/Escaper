﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class GUIAdvancedRendererApply : MonoBehaviour {

	public Toggle bloomActive;
	public Toggle vignetteActive;
	public Toggle chromaticAberrationActive;
	public Toggle grainActive;
	public Toggle ambientOcclusionActive;
	public Toggle isHDRActive;
	public Toggle dynamicShadowsActive;
	public Toggle volumetricLightActive;
	public GUIQualityController texturesQuality;
	public GUIQualityController antialiasingApplied;
	public GUIQualityController materialsQuality;

	public void Apply() {
		GraphicsSettingsData data = new GraphicsSettingsData ();
		data.isFastMode = false;
		data.bloomEnabled = bloomActive.isOn;
		data.vignetteEnabled = vignetteActive.isOn;
		data.chromaticAberrationEnabled = chromaticAberrationActive.isOn;
		data.grainEnabled = grainActive.isOn;
		data.isHDREnabled = isHDRActive.isOn;
		data.textureQuality = texturesQuality.quality;
		data.enableRealtimeShadows = dynamicShadowsActive.isOn;
		data.enableAmbientOcclusion = ambientOcclusionActive.isOn;
		data.enableVolumetricLighting = volumetricLightActive.isOn;
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
		data.shadersQuality = (ShaderQuality) materialsQuality.quality;
		GraphicsSettings.instance.Data = data;
	}

}
