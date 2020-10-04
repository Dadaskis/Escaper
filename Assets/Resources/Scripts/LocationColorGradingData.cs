using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LocationColorGradingSceneData { 
	public string sceneName;
	public Material colorGrading;
}

public class LocationColorGradingData : MonoBehaviour {

	public static LocationColorGradingData instance;

	public List<LocationColorGradingSceneData> colorGradingList = new List<LocationColorGradingSceneData>();
	public Dictionary<string, Material> colorGrading = new Dictionary<string, Material>();

	void Awake() {
		instance = this;
		foreach (LocationColorGradingSceneData data in colorGradingList) {
			colorGrading [data.sceneName] = data.colorGrading;
		}
	}

	void Start() {
		ChangeColorGradingToCurrent ();
	}

	public static void ChangeColorGradingToCurrent() {
		LocationColorGrading colorGrading = Camera.main.GetComponent<LocationColorGrading> ();
		if (colorGrading != null) {
			string sceneName = SceneManager.GetActiveScene ().name;
			Material colorGradingMaterial;
			if (instance.colorGrading.TryGetValue (sceneName, out colorGradingMaterial)) {
				colorGrading.caller.material = colorGradingMaterial;
			}
		}
	}

}
