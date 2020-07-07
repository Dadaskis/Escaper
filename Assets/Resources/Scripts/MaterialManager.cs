using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ShaderQuality {
	LOW,
	MEDIUM,
	HIGH,
	ULTRA,
	UNDEFINED
}

public enum MaterialMode {
	FAST,
	ADVANCED,
	UNDEFINED
}

[System.Serializable]
public class ShaderTextureData {
	public string textureName;
	public ShaderQuality bindedAtQuality;
}

[System.Serializable]
public class ShaderKeywordData {
	public string keyWord;
	public ShaderQuality bindedAtQuality;
}

[System.Serializable]
public class FastShaderOverrideData {
	public string keyWord;
	public string fastShaderName;
}

[System.Serializable]
public class ShaderSettingsData {
	public string advancedName;
	public string fastName;
	public List<FastShaderOverrideData> fastShaderOverride = new List<FastShaderOverrideData> ();
	public List<ShaderTextureData> textures = new List<ShaderTextureData>();
	public List<ShaderKeywordData> keywords = new List<ShaderKeywordData>();
}

[System.Serializable]
public class TextureSettingsData {
	public Texture texture;
	public ShaderTextureData shaderData;
}

[System.Serializable]
public class MaterialVariablesData {
	public string name;
	public Material material;
	public int shader;
	public List<TextureSettingsData> textures = new List<TextureSettingsData> ();
	public string advancedShaderName;
	public string fastShaderName;
}

[ExecuteInEditMode]
public class MaterialManager : MonoBehaviour {

	public List<ShaderSettingsData> shaderSettings = new List<ShaderSettingsData>();
	public Dictionary<string, int> advancedShaderSettingsIndex = new Dictionary<string, int>();
	public Dictionary<string, string> fastShaderSettingsIndex = new Dictionary<string, string>();
	public List<MaterialVariablesData> materialsSettings = new List<MaterialVariablesData>();
	public Dictionary<string, int> materialsSettingsIndex = new Dictionary<string, int> ();
	public ShaderQuality currentQuality;
	public MaterialMode currentMode;

	public static MaterialManager instance;

	void Awake() {
		Initialize ();
		instance = this;
	}

	public void Initialize() {
		int counter = 0;
		foreach (ShaderSettingsData data in shaderSettings) {
			advancedShaderSettingsIndex [data.advancedName] = counter;
			fastShaderSettingsIndex [data.fastName] = data.advancedName;
			foreach (FastShaderOverrideData fastShaderData in data.fastShaderOverride) {
				fastShaderSettingsIndex [fastShaderData.fastShaderName] = data.advancedName;
			}
			counter++;
		}

		counter = 0;
		foreach (MaterialVariablesData data in materialsSettings) {
			materialsSettingsIndex [data.material.name] = counter;
			counter++;
		}
	}

	public void ResetMaterials() {
		materialsSettings.Clear ();
		materialsSettingsIndex.Clear ();
	}

	public void ChangeQuality(ShaderQuality quality = ShaderQuality.UNDEFINED) {
		if (quality == ShaderQuality.UNDEFINED) {
			quality = currentQuality;
		}
		foreach (MaterialVariablesData data in materialsSettings) {
			ShaderSettingsData shaderSettingsData = shaderSettings [data.shader];

			foreach (TextureSettingsData textureData in data.textures) {
				if (quality >= textureData.shaderData.bindedAtQuality) {
					data.material.SetTexture (textureData.shaderData.textureName, textureData.texture);
				} else {
					data.material.SetTexture (textureData.shaderData.textureName, null);
				}
			}

			foreach (ShaderKeywordData keyWordData in shaderSettingsData.keywords) {
				if (quality >= keyWordData.bindedAtQuality) {
					data.material.EnableKeyword (keyWordData.keyWord);
				} else {
					data.material.DisableKeyword (keyWordData.keyWord);
				}
			}
		}
		currentQuality = quality;
	}

	public void ChangeMode(MaterialMode mode = MaterialMode.UNDEFINED) {
		if (mode == MaterialMode.UNDEFINED) {
			mode = currentMode;
		}

		if (mode == MaterialMode.ADVANCED) {
			Camera.main.renderingPath = RenderingPath.DeferredShading;
		} else if (mode == MaterialMode.FAST) {
			Camera.main.renderingPath = RenderingPath.Forward;
			ChangeQuality (ShaderQuality.LOW);
		}

		foreach (MaterialVariablesData data in materialsSettings) {
			if (mode == MaterialMode.ADVANCED) {
				data.material.shader = Shader.Find (data.advancedShaderName);
			} else if (mode == MaterialMode.FAST) {
				data.material.shader = Shader.Find (data.fastShaderName);
			}
		}

		if (mode == MaterialMode.FAST) {
			DynamicLightSwitcher[] switchers = FindObjectsOfType<DynamicLightSwitcher> ();
			foreach (DynamicLightSwitcher switcher in switchers) {	
				switcher.EnableStaticLighting ();
			}
		} 
	}

	public void RegisterMaterialsAndTextures() { 
		Initialize ();
		Material[] objects = Resources.LoadAll ("", typeof(Material)).Cast<Material>().ToArray();
		foreach (Material material in objects) {
			MaterialVariablesData data = new MaterialVariablesData();
			data.name = material.name;
			data.material = material;

			Shader shader = material.shader;
			int index;
			bool registered = false;
			string advancedShaderName;

			if (advancedShaderSettingsIndex.TryGetValue (shader.name, out index)) {
				advancedShaderName = shader.name;
				registered = true;
			} else {
				if (fastShaderSettingsIndex.TryGetValue (shader.name, out advancedShaderName)) {
					if (advancedShaderSettingsIndex.TryGetValue (advancedShaderName, out index)) {
						registered = true;
					} else {
						Debug.LogError (material.name + " cant be processed somewhat");
					}
				} 
			}

			if (registered) {
				ShaderSettingsData shaderData = shaderSettings [index];

				data.shader = index;

				data.advancedShaderName = advancedShaderName;

				bool fastShaderBinded = false;
				foreach (FastShaderOverrideData fastShaderData in shaderData.fastShaderOverride) {
					if (data.material.name.Contains (fastShaderData.keyWord)) {
						data.fastShaderName = fastShaderData.fastShaderName;
						fastShaderBinded = true;
					}
				}
				if (!fastShaderBinded) {
					data.fastShaderName = shaderData.fastName;
				}

				foreach (ShaderTextureData textureData in shaderData.textures) {
					Texture texture = material.GetTexture (textureData.textureName);
					if (texture != null) {
						TextureSettingsData textureSettings = new TextureSettingsData ();
						textureSettings.texture = texture;
						textureSettings.shaderData = textureData;
						data.textures.Add (textureSettings);
					}
				}
				int materialIndex;
				if (materialsSettingsIndex.TryGetValue (material.name, out materialIndex)) {
					MaterialVariablesData existData = materialsSettings[materialIndex];
					int textureIndex = 0;
					foreach (TextureSettingsData textureData in data.textures) {
						bool exist = false;
						foreach (TextureSettingsData existTextureData in existData.textures) {
							if (textureData.shaderData.textureName == existTextureData.shaderData.textureName) {
								exist = true;
								break;
							}
						}
						if (!exist) {
							existData.textures.Add (textureData);
						}
					}
					if (existData.advancedShaderName.Length == 0) {
						existData.advancedShaderName = data.advancedShaderName;
					}
					if (existData.fastShaderName.Length == 0) {
						existData.fastShaderName = data.fastShaderName;
					}
				} else {
					materialsSettings.Add (data);
					materialsSettingsIndex [material.name] = data.textures.Count - 1;
				}
			}
		}
	}

}
