using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GuidePanelData {
	public string locationName;
	public GameObject panelPrefab;
}

public class GUIGuidePanel : MonoBehaviour {

	public List<GuidePanelData> guidePanelsList = new List<GuidePanelData>();
	public Dictionary<string, GuidePanelData> guidePanels = new Dictionary<string, GuidePanelData>();
	public GameObject lockedPrefab;

	void Start() {
		foreach (GuidePanelData data in guidePanelsList) {
			guidePanels [data.locationName] = data;
		}
		UpdateInside ();
	}

	void OnEnable() {
		UpdateInside ();
	}

	public void UpdateInside() {
		for (int index = 0; index < transform.childCount; index++) {
			Destroy (transform.GetChild (index).gameObject);
		}

		if (!GameLogic.GetCurrentLocationSettings ().guideUnlocked) {
			Instantiate (lockedPrefab, transform);
			return;
		}

		Instantiate (guidePanels [GameLogic.GetCurrentLocationSettings().locationName].panelPrefab, transform);
	}

}
