using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public static class MaterialSettingsDataStorage {
	public static List<MaterialSettingsData> data = new List<MaterialSettingsData>();
}

[CreateAssetMenu(fileName = "Material settings", menuName = "New material settings")]
public class MaterialSettingsData : ScriptableObject {

	public Material material;

	public Texture diffuseMap;
	public Texture normalMap;
	public Texture specularMap;
	public Texture ambientOcclusionMap;
	public Texture detailMap;
	public Texture parallaxMap;

	void OnEnable() {
		//List<MaterialSettingsData> materials = MaterialSettingsDataStorage.data;
		//Debug.Log (materials.Contains (this) + " " + material.name);
		//if (materials.Contains(this) == false) {
		//	materials.Add (this);
		//}
	}

	public void ChangeQuality(MaterialShaderQuality quality) {
		/*

		VERY_LOW, 			// only diffuse
		LOW, 				// diffuse, detail
		LOW_PLUS, 			// diffuse, detail, ambient occlusion
		MEDIUM, 			// diffuse, normal, detail, ambient occlusion
		HIGH, 				// diffuse, normal, detail, ambient occlusion, specular
		ULTRA 				// diffuse, normal, detail, ambient occlusion, specular, parallax

		*//*
		switch (quality) {
		case MaterialShaderQuality.VERY_LOW:
			material.SetTexture ("_MainTex", diffuseMap);
			material.SetTexture ("_BumpMap", null);
			material.SetTexture ("_SpecGlossMap", null);
			material.SetTexture ("_ParallaxMap", null);
			material.SetTexture ("_OcclusionMap", null);
			material.SetTexture ("_DetailMask", null);
			material.SetFloat ("_SpecularHighlights", 0.0f);
			material.SetFloat ("_Glossiness", 0.0f);
			material.DisableKeyword ("_SPECULARHIGHLIGHTS_OFF");
			material.DisableKeyword ("_NORMALMAP");
			material.DisableKeyword ("_PARALLAXMAP");
			material.DisableKeyword ("_DETAIL_MULX2");
			material.DisableKeyword ("_SPECGLOSSMAP");
			break;
		case MaterialShaderQuality.LOW:
			material.EnableKeyword ("_DETAIL_MULX2");
			material.SetTexture ("_MainTex", diffuseMap);
			material.SetTexture ("_BumpMap", null);
			material.SetTexture ("_SpecGlossMap", null);
			material.SetTexture ("_ParallaxMap", null);
			material.SetTexture ("_OcclusionMap", null);
			material.SetTexture ("_DetailMask", detailMap);
			material.SetFloat ("_SpecularHighlights", 0.0f);
			material.SetFloat ("_Glossiness", 0.0f);
			material.DisableKeyword ("_SPECULARHIGHLIGHTS_OFF");
			material.DisableKeyword ("_NORMALMAP");
			material.DisableKeyword ("_PARALLAXMAP");
			material.DisableKeyword ("_SPECGLOSSMAP");
			break;
		case MaterialShaderQuality.LOW_PLUS:
			material.EnableKeyword ("_DETAIL_MULX2");
			material.SetTexture ("_MainTex", diffuseMap);
			material.SetTexture ("_BumpMap", null);
			material.SetTexture ("_SpecGlossMap", null);
			material.SetTexture ("_ParallaxMap", null);
			material.SetTexture ("_OcclusionMap", ambientOcclusionMap);
			material.SetTexture ("_DetailMask", detailMap);
			material.SetFloat ("_SpecularHighlights", 0.0f);
			material.SetFloat ("_Glossiness", 0.0f);
			material.DisableKeyword ("_SPECULARHIGHLIGHTS_OFF");
			material.DisableKeyword ("_NORMALMAP");
			material.DisableKeyword ("_PARALLAXMAP");
			material.DisableKeyword ("_SPECGLOSSMAP");
			break;
		case MaterialShaderQuality.MEDIUM:
			material.EnableKeyword ("_NORMALMAP");
			material.EnableKeyword ("_DETAIL_MULX2");
			material.SetTexture ("_MainTex", diffuseMap);
			material.SetTexture ("_BumpMap", normalMap);
			material.SetTexture ("_SpecGlossMap", null);
			material.SetTexture ("_ParallaxMap", null);
			material.SetTexture ("_OcclusionMap", ambientOcclusionMap);
			material.SetTexture ("_DetailMask", detailMap);
			material.SetFloat ("_SpecularHighlights", 0.0f);
			material.SetFloat ("_Glossiness", 0.0f);
			material.DisableKeyword ("_SPECULARHIGHLIGHTS_OFF");
			material.DisableKeyword ("_PARALLAXMAP");
			material.DisableKeyword ("_SPECGLOSSMAP");
			break;
		case MaterialShaderQuality.HIGH:
			material.EnableKeyword ("_NORMALMAP");
			material.EnableKeyword ("_DETAIL_MULX2");
			material.EnableKeyword ("_SPECGLOSSMAP");
			material.EnableKeyword ("_SPECULARHIGHLIGHTS_OFF");
			material.SetTexture ("_MainTex", diffuseMap);
			material.SetTexture ("_BumpMap", normalMap);
			material.SetTexture ("_SpecGlossMap", specularMap);
			material.SetTexture ("_ParallaxMap", null);
			material.SetTexture ("_OcclusionMap", ambientOcclusionMap);
			material.SetTexture ("_DetailMask", detailMap);
			material.SetFloat ("_SpecularHighlights", 1.0f);
			material.SetFloat ("_Glossiness", 0.5f);
			material.DisableKeyword ("_PARALLAXMAP");
			break;
		case MaterialShaderQuality.ULTRA:
			material.EnableKeyword ("_NORMALMAP");
			material.EnableKeyword ("_PARALLAXMAP");
			material.EnableKeyword ("_DETAIL_MULX2");
			material.EnableKeyword ("_SPECGLOSSMAP");
			material.EnableKeyword ("_SPECULARHIGHLIGHTS_OFF");
			material.SetTexture ("_MainTex", diffuseMap);
			material.SetTexture ("_BumpMap", normalMap);
			material.SetTexture ("_SpecGlossMap", specularMap);
			material.SetTexture ("_ParallaxMap", parallaxMap);
			material.SetTexture ("_OcclusionMap", ambientOcclusionMap);
			material.SetTexture ("_DetailMask", detailMap);
			material.SetFloat ("_SpecularHighlights", 1.0f);
			material.SetFloat ("_Glossiness", 0.5f);
			break;
		}
	}

}
*/