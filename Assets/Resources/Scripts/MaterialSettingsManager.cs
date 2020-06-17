using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MaterialShaderQuality {
	VERY_LOW, 	// only diffuse
	LOW, 		// diffuse, detail
	LOW_PLUS, 	// diffuse, detail, ambient occlusion
	MEDIUM, 	// diffuse, normal, detail, ambient occlusion
	HIGH, 		// diffuse, normal, detail, ambient occlusion, specular
	ULTRA 		// diffuse, normal, detail, ambient occlusion, specular, parallax
}

[System.Serializable]
public class MaterialSettingsData { 
	public Material material;

	public Texture diffuseMap;
	public Texture normalMap;
	public Texture specularMap;
	public Texture ambientOcclusionMap;
	public Texture detailMap;
	public Texture parallaxMap;

	public void ChangeQuality(MaterialShaderQuality quality) {
		if (material == null) {
			Debug.LogWarning ("Havent material here!");
			return;
		}
		/*

		VERY_LOW, 			// only diffuse
		LOW, 				// diffuse, detail
		LOW_PLUS, 			// diffuse, detail, ambient occlusion
		MEDIUM, 			// diffuse, normal, detail, ambient occlusion
		HIGH, 				// diffuse, normal, detail, ambient occlusion, specular
		ULTRA 				// diffuse, normal, detail, ambient occlusion, specular, parallax

		*/
		switch (quality) {
		case MaterialShaderQuality.VERY_LOW:
			material.SetTexture ("_MainTex", diffuseMap);
			material.SetTexture ("_BumpMap", null);
			material.SetTexture ("_SpecGlossMap", null);
			material.SetTexture ("_ParallaxMap", null);
			material.SetTexture ("_OcclusionMap", null);
			material.SetTexture ("_DetailMask", null);
			material.SetFloat ("_SpecularHighlights", 0.0f);
			//material.SetFloat ("_Glossiness", 0.0f);
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
			//material.SetFloat ("_Glossiness", 0.0f);
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
			//material.SetFloat ("_Glossiness", 0.0f);
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
			//material.SetFloat ("_Glossiness", 0.0f);
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
			//material.SetFloat ("_Glossiness", 0.5f);
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
			//material.SetFloat ("_Glossiness", 0.5f);
			break;
		}
	}
} 

[System.Serializable]
public class MaterialMetalSettingsData { 
	public Material material;

	public Texture diffuseMap;
	public Texture normalMap;
	public Texture metallicMap;
	public Texture ambientOcclusionMap;
	public Texture detailMap;
	public Texture parallaxMap;

	public void ChangeQuality(MaterialShaderQuality quality) {
		if (material == null) {
			Debug.LogWarning ("Havent material here!");
			return;
		}
		/*

		VERY_LOW, 			// only diffuse
		LOW, 				// diffuse, detail
		LOW_PLUS, 			// diffuse, detail, ambient occlusion
		MEDIUM, 			// diffuse, normal, detail, ambient occlusion
		HIGH, 				// diffuse, normal, detail, ambient occlusion, specular
		ULTRA 				// diffuse, normal, detail, ambient occlusion, specular, parallax

		*/
		switch (quality) {
		case MaterialShaderQuality.VERY_LOW:
			material.SetTexture ("_MainTex", diffuseMap);
			material.SetTexture ("_BumpMap", null);
			material.SetTexture ("_SpecGlossMap", null);
			material.SetTexture ("_ParallaxMap", null);
			material.SetTexture ("_OcclusionMap", null);
			material.SetTexture ("_DetailMask", null);
			material.SetFloat ("_SpecularHighlights", 0.0f);
			//material.SetFloat ("_Glossiness", 0.0f);
			material.DisableKeyword ("_SPECULARHIGHLIGHTS_OFF");
			material.DisableKeyword ("_NORMALMAP");
			material.DisableKeyword ("_PARALLAXMAP");
			material.DisableKeyword ("_DETAIL_MULX2");
			material.DisableKeyword ("_METALLICGLOSSMAP");
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
			//material.SetFloat ("_Glossiness", 0.0f);
			material.DisableKeyword ("_SPECULARHIGHLIGHTS_OFF");
			material.DisableKeyword ("_NORMALMAP");
			material.DisableKeyword ("_PARALLAXMAP");
			material.DisableKeyword ("_METALLICGLOSSMAP");
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
			//material.SetFloat ("_Glossiness", 0.0f);
			material.DisableKeyword ("_SPECULARHIGHLIGHTS_OFF");
			material.DisableKeyword ("_NORMALMAP");
			material.DisableKeyword ("_PARALLAXMAP");
			material.DisableKeyword ("_METALLICGLOSSMAP");
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
			//material.SetFloat ("_Glossiness", 0.0f);
			material.DisableKeyword ("_SPECULARHIGHLIGHTS_OFF");
			material.DisableKeyword ("_PARALLAXMAP");
			material.DisableKeyword ("_METALLICGLOSSMAP");
			break;
		case MaterialShaderQuality.HIGH:
			material.EnableKeyword ("_NORMALMAP");
			material.EnableKeyword ("_DETAIL_MULX2");
			material.EnableKeyword ("_METALLICGLOSSMAP");
			material.EnableKeyword ("_SPECULARHIGHLIGHTS_OFF");
			material.SetTexture ("_MainTex", diffuseMap);
			material.SetTexture ("_BumpMap", normalMap);
			material.SetTexture ("_SpecGlossMap", metallicMap);
			material.SetTexture ("_ParallaxMap", null);
			material.SetTexture ("_OcclusionMap", ambientOcclusionMap);
			material.SetTexture ("_DetailMask", detailMap);
			material.SetFloat ("_SpecularHighlights", 1.0f);
			//material.SetFloat ("_Glossiness", 0.5f);
			material.DisableKeyword ("_PARALLAXMAP");
			break;
		case MaterialShaderQuality.ULTRA:
			material.EnableKeyword ("_NORMALMAP");
			material.EnableKeyword ("_PARALLAXMAP");
			material.EnableKeyword ("_DETAIL_MULX2");
			material.EnableKeyword ("_METALLICGLOSSMAP");
			material.EnableKeyword ("_SPECULARHIGHLIGHTS_OFF");
			material.SetTexture ("_MainTex", diffuseMap);
			material.SetTexture ("_BumpMap", normalMap);
			material.SetTexture ("_MetallicGlossMap", metallicMap);
			material.SetTexture ("_ParallaxMap", parallaxMap);
			material.SetTexture ("_OcclusionMap", ambientOcclusionMap);
			material.SetTexture ("_DetailMask", detailMap);
			material.SetFloat ("_SpecularHighlights", 1.0f);
			//material.SetFloat ("_Glossiness", 0.5f);
			break;
		}
	}
} 

[System.Serializable]
public class NoiseBlendMaterialSettingsData { 
	public Material material;

	public Texture diffuseMap1;
	public Texture normalMap1;
	public Texture specularMap1;
	public Texture parallaxMap1;

	public Texture diffuseMap2;
	public Texture normalMap2;
	public Texture specularMap2;
	public Texture parallaxMap2;

	public void ChangeQuality(MaterialShaderQuality quality) {
		if (material == null) {
			Debug.LogWarning ("Havent material here!");
			return;
		}
		/*

		VERY_LOW, 			// only diffuse
		LOW, 				// diffuse, detail
		LOW_PLUS, 			// diffuse, detail, ambient occlusion
		MEDIUM, 			// diffuse, normal, detail, ambient occlusion
		HIGH, 				// diffuse, normal, detail, ambient occlusion, specular
		ULTRA 				// diffuse, normal, detail, ambient occlusion, specular, parallax

		*/
		switch (quality) {
		case MaterialShaderQuality.VERY_LOW:
			material.SetTexture ("_Color1", diffuseMap1);
			material.SetTexture ("_Normal1", null);
			material.SetTexture ("_Specular1", null);
			material.SetTexture ("_Displacement1", null);
			material.SetTexture ("_Color2", diffuseMap2);
			material.SetTexture ("_Normal2", null);
			material.SetTexture ("_Specular2", null);
			material.SetTexture ("_Displacement2", null);

			material.DisableKeyword ("_SPECULARHIGHLIGHTS_OFF");
			material.DisableKeyword ("_NORMALMAP");
			material.DisableKeyword ("_PARALLAXMAP");
			material.DisableKeyword ("_SPECGLOSSMAP");
			break;
		case MaterialShaderQuality.LOW:
			material.SetTexture ("_Color1", diffuseMap1);
			material.SetTexture ("_Normal1", null);
			material.SetTexture ("_Specular1", null);
			material.SetTexture ("_Displacement1", null);
			material.SetTexture ("_Color2", diffuseMap2);
			material.SetTexture ("_Normal2", null);
			material.SetTexture ("_Specular2", null);
			material.SetTexture ("_Displacement2", null);

			material.DisableKeyword ("_SPECULARHIGHLIGHTS_OFF");
			material.DisableKeyword ("_NORMALMAP");
			material.DisableKeyword ("_PARALLAXMAP");
			material.DisableKeyword ("_SPECGLOSSMAP");
			break;
		case MaterialShaderQuality.LOW_PLUS:
			material.SetTexture ("_Color1", diffuseMap1);
			material.SetTexture ("_Normal1", null);
			material.SetTexture ("_Specular1", null);
			material.SetTexture ("_Displacement1", null);
			material.SetTexture ("_Color2", diffuseMap2);
			material.SetTexture ("_Normal2", null);
			material.SetTexture ("_Specular2", null);
			material.SetTexture ("_Displacement2", null);

			material.DisableKeyword ("_SPECULARHIGHLIGHTS_OFF");
			material.DisableKeyword ("_NORMALMAP");
			material.DisableKeyword ("_PARALLAXMAP");
			material.DisableKeyword ("_SPECGLOSSMAP");
			break;
		case MaterialShaderQuality.MEDIUM:
			material.EnableKeyword ("_NORMALMAP");

			material.SetTexture ("_Color1", diffuseMap1);
			material.SetTexture ("_Normal1", normalMap1);
			material.SetTexture ("_Specular1", null);
			material.SetTexture ("_Displacement1", null);
			material.SetTexture ("_Color2", diffuseMap2);
			material.SetTexture ("_Normal2", normalMap2);
			material.SetTexture ("_Specular2", null);
			material.SetTexture ("_Displacement2", null);

			material.DisableKeyword ("_SPECULARHIGHLIGHTS_OFF");
			material.DisableKeyword ("_PARALLAXMAP");
			material.DisableKeyword ("_SPECGLOSSMAP");
			break;
		case MaterialShaderQuality.HIGH:
			material.EnableKeyword ("_NORMALMAP");
			material.EnableKeyword ("_SPECGLOSSMAP");
			material.EnableKeyword ("_SPECULARHIGHLIGHTS_OFF");

			material.SetTexture ("_Color1", diffuseMap1);
			material.SetTexture ("_Normal1", normalMap1);
			material.SetTexture ("_Specular1", specularMap1);
			material.SetTexture ("_Displacement1", null);
			material.SetTexture ("_Color2", diffuseMap2);
			material.SetTexture ("_Normal2", normalMap2);
			material.SetTexture ("_Specular2", specularMap2);
			material.SetTexture ("_Displacement2", null);

			material.DisableKeyword ("_PARALLAXMAP");
			break;
		case MaterialShaderQuality.ULTRA:
			material.EnableKeyword ("_NORMALMAP");
			material.EnableKeyword ("_SPECGLOSSMAP");
			material.EnableKeyword ("_SPECULARHIGHLIGHTS_OFF");
			material.EnableKeyword ("_PARALLAXMAP");

			material.SetTexture ("_Color1", diffuseMap1);
			material.SetTexture ("_Normal1", normalMap1);
			material.SetTexture ("_Specular1", specularMap1);
			material.SetTexture ("_Displacement1", parallaxMap1);
			material.SetTexture ("_Color2", diffuseMap2);
			material.SetTexture ("_Normal2", normalMap2);
			material.SetTexture ("_Specular2", specularMap2);
			material.SetTexture ("_Displacement2", parallaxMap2);
			break;
		}
	}
} 

public class MaterialSettingsManager : MonoBehaviour {

	public static MaterialSettingsManager instance; 

	public List<MaterialSettingsData> materials;
	public List<MaterialMetalSettingsData> metalMaterials;
	public List<NoiseBlendMaterialSettingsData> noiseBlendMaterials;

	void Awake () {
		instance = this;
		ChangeQuality (MaterialShaderQuality.VERY_LOW);
	}
		
	public void ChangeQuality (MaterialShaderQuality quality) {
		foreach (MaterialSettingsData materialData in materials) {
			materialData.ChangeQuality (quality);
		}

		foreach (NoiseBlendMaterialSettingsData materialData in noiseBlendMaterials) {
			materialData.ChangeQuality (quality);
		}

		foreach (MaterialMetalSettingsData materialData in metalMaterials) {
			materialData.ChangeQuality (quality);
		}
	}
}
