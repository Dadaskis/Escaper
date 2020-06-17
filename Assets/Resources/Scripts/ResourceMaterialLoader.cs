using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

[LuaApi(
	luaName = "MaterialLoader",
	description = "C# side of material registering")]
public class ResourceMaterialLoader : LuaAPIBase {

	public static ResourceMaterialLoader instance;
	public Shader standartShader;
	public Shader noiseBlendShader;

	public Dictionary<string, Material> materials = new Dictionary<string, Material>();

	public static Material GetMaterial(string name) {
		Material material;
		if (instance.materials.TryGetValue (name, out material)) {
			return material;
		}
		return null;
	}

	public ResourceMaterialLoader() : base("MaterialLoader") {
		standartShader = Shader.Find("Standard (Specular setup)");
		Debug.Log (standartShader);
		noiseBlendShader = Shader.Find ("Custom/NoiseBlend");
		Debug.Log (noiseBlendShader);
	}

	protected override void InitialiseAPITable(){
		instance = this;
		m_ApiTable ["RegisterStandardMaterial"] = (System.Func<DynValue, bool>) RegisterStandardMaterial;
		m_ApiTable ["RegisterNoiseBlendMaterial"] = (System.Func<DynValue, bool>) RegisterNoiseBlendMaterial;
	}

	string GetStringFromTable(Table table, string key) {
		DynValue value = table.Get (key);
		if (!value.IsNil ()) {
			return value.CastToString ();
		}
		return "";
	}

	float GetFloatFromTable(Table table, string key) {
		DynValue value = table.Get (key);
		if (!value.IsNil ()) {
			return (float)value.CastToNumber ();
		}
		return 0.0f;
	}

	Vector3 GetVector3FromTable(Table table, string key){
		Vector3 vector = new Vector3 ();
		DynValue vectorValue = (DynValue) table.Get(key);
		if (!vectorValue.IsNil () && vectorValue.Type == DataType.Table) {
			Table vectorTable = vectorValue.Table;
			vector.x = GetFloatFromTable (vectorTable, "X");
			vector.y = GetFloatFromTable (vectorTable, "Y");
			vector.z = GetFloatFromTable (vectorTable, "Z");
		}
		return vector;
	}

	Color GetColorFromTable(Table table, string key){
		Color resultColor = new Color ();
		DynValue color = (DynValue) table.Get(key);
		if (!color.IsNil () && color.Type == DataType.Table) {
			Table colorTable = color.Table;
			resultColor.r = GetFloatFromTable (colorTable, "R");
			resultColor.g = GetFloatFromTable (colorTable, "G");
			resultColor.b = GetFloatFromTable (colorTable, "B");
			resultColor.a = GetFloatFromTable (colorTable, "A");
		}
		return resultColor;
	}

	[LuaApiFunction(
		name = "RegisterStandardMaterial", 
		description = "Register new standard material"
	)]
	public bool RegisterStandardMaterial(DynValue value) {
		if (value.Type == DataType.Table) {
			Table table = value.Table;
			Color materialColor = new Color (1.0f, 1.0f, 1.0f, 1.0f);
			if (table ["Color"] != null) {
				materialColor = GetColorFromTable (table, "Color");
			}
			return RegisterStandardMaterial (
				GetStringFromTable(table, "MaterialName"),
				GetStringFromTable(table, "DiffuseName"),
				GetStringFromTable(table, "NormalName"),
				GetStringFromTable(table, "SpecularName"),
				GetStringFromTable(table, "ParallaxName"),
				GetStringFromTable(table, "DetailName"),
				GetStringFromTable(table, "AOName"),
				materialColor,
				GetFloatFromTable(table, "NormalPower"),
				GetFloatFromTable(table, "SpecularPower")
			);
		}
		return false;
	}

	public bool RegisterStandardMaterial(
		string materialName,
		string diffuseName,
		string normalName,
		string specularName,
		string parallaxName,
		string detailName,
		string AOName,
		Color color,
		float normalPower,
		float specularPower
	) {
		Debug.Log ("Loading material " + materialName);
		Material material = new Material (standartShader);
		materials [materialName] = material;
		material.SetColor ("_Color", color);
		MaterialSettingsData data = new MaterialSettingsData();
		data.material = material;
		data.diffuseMap = ResourceTextureLoader.GetTexture (diffuseName);
		data.normalMap = ResourceTextureLoader.GetTexture (normalName);
		data.specularMap = ResourceTextureLoader.GetTexture (specularName);
		data.parallaxMap = ResourceTextureLoader.GetTexture (parallaxName);
		data.detailMap = ResourceTextureLoader.GetTexture (detailName);
		data.ambientOcclusionMap = ResourceTextureLoader.GetTexture (AOName);
		MaterialSettingsManager.instance.materials.Add (data);
		MaterialSettingsManager.instance.ChangeQuality (GraphicsSettings.instance.Data.shadersQuality);
		return true;
	}

	[LuaApiFunction(
		name = "RegisterNoiseBlendMaterial", 
		description = "Register new NoiseBlend material"
	)]
	public bool RegisterNoiseBlendMaterial(DynValue value) {
		if (value.Table != null) {
			Table table = value.Table;
			return RegisterNoiseBlendMaterial (
				GetStringFromTable(table, "MaterialName"),
				GetStringFromTable(table, "Diffuse1Name"),
				GetStringFromTable(table, "Normal1Name"),
				GetStringFromTable(table, "Specular1Name"),
				GetStringFromTable(table, "Parallax1Name"),
				GetStringFromTable(table, "Diffuse2Name"),
				GetStringFromTable(table, "Normal2Name"),
				GetStringFromTable(table, "Specular2Name"),
				GetStringFromTable(table, "Parallax2Name"),
				GetStringFromTable(table, "MaskName"),
				GetFloatFromTable(table, "MaskPower"),
				GetVector3FromTable(table, "NoiseOffset"),
				GetFloatFromTable(table, "NoiseScale"),
				GetFloatFromTable(table, "NoisePower"),
				GetFloatFromTable(table, "UpCheckValue"),
				GetFloatFromTable(table, "UpPower"),
				GetColorFromTable(table, "Color1"),
				GetColorFromTable(table, "Color2"),
				GetFloatFromTable(table, "Specular1Power"),
				GetFloatFromTable(table, "Specular2Power"),
				GetFloatFromTable(table, "Parallax1Power"),
				GetFloatFromTable(table, "Parallax2Power"),
				GetFloatFromTable(table, "Normal1Power"),
				GetFloatFromTable(table, "Normal2Power")
			);
		}
		return false;
	}

	public bool RegisterNoiseBlendMaterial(
		string materialName,
		string diffuse1Name,
		string normal1Name,
		string specular1Name,
		string parallax1Name,
		string diffuse2Name,
		string normal2Name,
		string specular2Name,
		string parallax2Name,
		string maskName,
		float maskPower,
		Vector3 noiseOffset,		// What the hell am i doing
		float noiseScale,
		float noisePower,
		float upCheckValue,
		float upPower,
		Color color1,
		Color color2,
		float smoothness1,
		float smoothness2,
		float displacementPower1,
		float displacementPower2,
		float normalPower1,
		float normalPower2
	) {
		Material material = new Material (noiseBlendShader);
		materials [materialName] = material;

		material.SetFloat ("_Glossiness1", smoothness1);
		material.SetFloat ("_Glossiness2", smoothness2);
		material.SetFloat ("_NormalPower1", normalPower1);
		material.SetFloat ("_NormalPower2", normalPower2);
		material.SetFloat ("_DisplacementPower1", displacementPower1);
		material.SetFloat ("_DisplacementPower2", displacementPower2);
		material.SetColor ("_ColorMultiplier1", color1);
		material.SetColor ("_ColorMultiplier2", color2);
		material.SetTexture("_Mask", ResourceTextureLoader.GetTexture(maskName));
		material.SetFloat ("_MaskPower", maskPower);
		material.SetVector ("_Offset", noiseOffset);
		material.SetFloat ("_Scale", noiseScale);
		material.SetFloat ("_Power", noisePower);
		material.SetFloat ("_UpCheckValue", upCheckValue);
		material.SetFloat ("_UpPower", upPower);

		NoiseBlendMaterialSettingsData data = new NoiseBlendMaterialSettingsData();
		data.material = material;
		data.diffuseMap1 = ResourceTextureLoader.GetTexture (diffuse1Name);
		data.diffuseMap2 = ResourceTextureLoader.GetTexture (diffuse2Name);
		data.normalMap1 = ResourceTextureLoader.GetTexture (normal1Name);
		data.normalMap2 = ResourceTextureLoader.GetTexture (normal2Name);
		data.specularMap1 = ResourceTextureLoader.GetTexture (specular1Name);
		data.specularMap2 = ResourceTextureLoader.GetTexture (specular2Name);
		data.parallaxMap1 = ResourceTextureLoader.GetTexture (parallax1Name);
		data.parallaxMap2 = ResourceTextureLoader.GetTexture (parallax2Name);
		MaterialSettingsManager.instance.noiseBlendMaterials.Add (data);
		MaterialSettingsManager.instance.ChangeQuality (GraphicsSettings.instance.Data.shadersQuality);
		return true;
	}


}
