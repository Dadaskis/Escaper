using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering.PostProcessing;
using Newtonsoft.Json;

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
	public bool blacklisted = false;
}

[System.Serializable]
public class MaterialManagerSaveData {
	public ShaderQuality currentQuality;
	public MaterialMode currentMode;
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

	public void Save() {
		try {
			MaterialManagerSaveData data = new MaterialManagerSaveData ();
			data.currentMode = currentMode;
			data.currentQuality = currentQuality;
			string json = JsonConvert.SerializeObject(data);
			System.IO.File.WriteAllText("Saves/MaterialManager.settings", json);
		} catch(System.Exception ex) {
			Debug.LogError (ex);
		}
	}

	public void Load() {
		try {
			string json = System.IO.File.ReadAllText("Saves/MaterialManager.settings");
			MaterialManagerSaveData data = JsonConvert.DeserializeObject<MaterialManagerSaveData>(json);
			currentMode = data.currentMode;
			currentQuality = data.currentQuality;
			ChangeQuality(currentQuality);
			ChangeMode (currentMode);
		} catch(System.Exception ex) {
			Debug.LogError (ex);
		}
	}

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
			if (data.blacklisted) {
				continue;
			}

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
		Save ();
	}

	IEnumerator ChangeCamerasRenderingPath() {
		int counter = 0;
		while (true) {
			bool isRightPath = true;

			foreach (Camera camera in Camera.allCameras) {
				if (currentMode == MaterialMode.ADVANCED) {
					camera.renderingPath = RenderingPath.DeferredShading;
				} else if (currentMode == MaterialMode.FAST) {
					camera.renderingPath = RenderingPath.Forward;
				}
			}

			foreach (Camera camera in Camera.allCameras) {
				if (currentMode == MaterialMode.ADVANCED) {
					if (camera.actualRenderingPath != RenderingPath.DeferredShading) {
						isRightPath = false;
					}
				} else if (currentMode == MaterialMode.FAST) {
					if (camera.actualRenderingPath != RenderingPath.Forward) {
						isRightPath = false;
					}
				}
				Debug.LogError (camera.actualRenderingPath);
			}

			if (isRightPath == false) {
				yield return new WaitForSeconds (0.1f);
				counter = 0;
			} else {
				counter++;
				if (counter > 10) {
					break;
				}
			}
		}
	}

	public void ChangeMode(MaterialMode mode = MaterialMode.UNDEFINED) {
		if (mode == MaterialMode.UNDEFINED) {
			mode = currentMode;
		}

		foreach (MaterialVariablesData data in materialsSettings) {
			if (data.blacklisted) {
				continue;
			}

			if (mode == MaterialMode.ADVANCED) {
				data.material.shader = Shader.Find (data.advancedShaderName);
			} else if (mode == MaterialMode.FAST) {
				data.material.shader = Shader.Find (data.fastShaderName);
			}
		}

		if (mode == MaterialMode.FAST) {
			GraphicsSettingsData data = GraphicsSettings.instance.Data;
			data.enableRealtimeShadows = false;
			data.enableAmbientOcclusion = false;
			ChangeQuality (ShaderQuality.LOW);

			GraphicsSettings.instance.Data = data;
		}

		currentMode = mode;

		StartCoroutine (ChangeCamerasRenderingPath ());

		Save ();
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

	public Material GetMaterialFromRaycast(RaycastHit hit) { 
		if (hit.triangleIndex == -1) {
			return null;
		}

		//MeshFilter filter = hit.transform.GetComponent<MeshFilter> ();
		Renderer renderer = hit.transform.GetComponent<Renderer> ();
		MeshCollider collider = hit.collider as MeshCollider;
		Mesh mesh = collider.sharedMesh;

		int[] triangleIndexes = new int[] {
			mesh.triangles[(hit.triangleIndex * 3)], 
			mesh.triangles[(hit.triangleIndex * 3) + 1],
			mesh.triangles[(hit.triangleIndex * 3) + 2]
		};

		int materialIndex = -1;
		for(int subMeshIndex = 0; subMeshIndex < mesh.subMeshCount; subMeshIndex++) {
			int[] triangles = mesh.GetTriangles (subMeshIndex);
			for (int index = 0; index < triangles.Length; index += 3) {
				if (triangles [index] == triangleIndexes[0] 
					&& triangles[index + 1] == triangleIndexes[1] 
					&& triangles[index + 2] == triangleIndexes[2]) {
					materialIndex = subMeshIndex;
					break;
				}
			}
		}

		if (materialIndex == -1) {
			return null;
		}

		Material material = null;
		//try {
			material = renderer.sharedMaterials [materialIndex];
		//} catch(System.Exception ex) {
		//	material = renderer.materials [materialIndex];
		//}

		return material;
	}

}
