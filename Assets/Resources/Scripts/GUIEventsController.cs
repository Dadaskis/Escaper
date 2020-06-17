using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIEventsController : MonoBehaviour {

	public enum GUIEvents {
		STAY,
		BACK_TO_MAIN_MENU,
		MOVE_TO_LOAD_MENU,
		MOVE_TO_SETTINGS
	};

	public GUIEvents status;
	public GameObject canvasCameraObject;
	public GameObject loadGameCameraObject;
	public GameObject settingsCameraObject;
	public BezierCurve loadGamePath;
	public BezierCurve settingsPath;
	public BezierCurve lastPath;
	public float pathTimer = 0.0f;
	public GameObject camera;
	public MainMenuCameraRotator rotator;
	public Text textureQualityText;

	public Toggle bloomEnabled;
	public Toggle vignetteEnabled;
	public Toggle grainEnabled;
	public Toggle chromaticAberrationEnabled;
	public Toggle colorGradingEnabled;

	public GameObject destroySingletons;
	public GameObject player;

	[System.Serializable]
	public class KeyButtonData {
		public Button button;
		public Text text;
		public string name;
	}

	public KeyButtonData[] keySettingButtons;
	public KeyButtonData listenButton;
	public bool isListeningButton = false;
	 
	void Start() {
		rotator = camera.GetComponentInChildren<MainMenuCameraRotator> ();
		GraphicsSettingsData data = GraphicsSettings.instance.Data;
		if (data != null) {
			bloomEnabled.isOn = data.bloomEnabled;
			vignetteEnabled.isOn = data.vignetteEnabled;
			grainEnabled.isOn = data.grainEnabled;
			chromaticAberrationEnabled.isOn = data.chromaticAberrationEnabled;
			colorGradingEnabled.isOn = data.colorGradingEnabled;
			TextureQualityTextChange (data.textureQuality);
		}

		InputManager.instance.onKeyPressed.AddListener (OnKeyPressed);

		foreach(KeyButtonData keyData in keySettingButtons) {
			keyData.button.onClick.AddListener(delegate {
				if(!isListeningButton) {
					listenButton = keyData;
					keyData.text.text = "---";
					isListeningButton = true;
				}
			});
			keyData.text.text = System.Enum.GetName (typeof(KeyCode), InputManager.instance.keys[keyData.name]);
		}
	}

	void OnKeyPressed(KeyCode code) {
		if (isListeningButton) {
			listenButton.text.text = System.Enum.GetName (typeof(KeyCode), code);
			InputManager.instance.keys [listenButton.name] = code;
			listenButton = null;
			isListeningButton = false;
		}
	}

	void Update() {
		switch (status) {
		case GUIEvents.STAY:
			break;
		case GUIEvents.MOVE_TO_LOAD_MENU:
			camera.transform.position = loadGamePath.GetPointAt (pathTimer);
			if (pathTimer > 0.99f) {
				status = GUIEvents.STAY;
			}
			break;
		case GUIEvents.MOVE_TO_SETTINGS:
			camera.transform.position = settingsPath.GetPointAt (pathTimer);
			if (pathTimer > 0.99f) {
				status = GUIEvents.STAY;
			}
			break;
		case GUIEvents.BACK_TO_MAIN_MENU:
			camera.transform.position = lastPath.GetPointAt (1.0f - pathTimer);
			if (1.0f - pathTimer < 0.01f) {
				rotator.canvas = canvasCameraObject;
				status = GUIEvents.STAY;
			}
			break;
		}
		pathTimer += Time.deltaTime;
	}

	public void OnNewGame() {
		//Destroy (destroySingletons);
		ResourceMapLoader.instance.ChangeMap(ResourceMapLoader.instance.onNewGameMap);
		Instantiate(player);
		player.transform.position = Vector3.zero;
	} 

	public void OnLoadGame() {
		lastPath = loadGamePath;
		status = GUIEvents.MOVE_TO_LOAD_MENU;
		pathTimer = 0.0f;
		rotator.canvas = loadGameCameraObject;
	}

	public void OnSettings() {
		lastPath = settingsPath;
		status = GUIEvents.MOVE_TO_SETTINGS;
		pathTimer = 0.0f;
		rotator.canvas = settingsCameraObject;
	}

	public void OnBack() {
		status = GUIEvents.BACK_TO_MAIN_MENU;
		pathTimer = 0.0f;
	}

	public void OnExit() {
		Application.Quit ();
	}

	public void OnBloomCheck(bool isChecked) {
		GraphicsSettings settings = GraphicsSettings.instance;
		GraphicsSettingsData data = settings.Data;
		data.bloomEnabled = isChecked;
		settings.Data = data;
	}

	public void OnVignetteCheck(bool isChecked) {
		GraphicsSettings settings = GraphicsSettings.instance;
		GraphicsSettingsData data = settings.Data;
		data.vignetteEnabled = isChecked;
		settings.Data = data;
	}

	public void OnGrainCheck(bool isChecked) {
		GraphicsSettings settings = GraphicsSettings.instance;
		GraphicsSettingsData data = settings.Data;
		data.grainEnabled = isChecked;
		settings.Data = data;
	}

	public void OnChromaticAberrationCheck(bool isChecked) {
		GraphicsSettings settings = GraphicsSettings.instance;
		GraphicsSettingsData data = settings.Data;
		data.chromaticAberrationEnabled = isChecked;
		settings.Data = data;
	}

	public void OnColorGradingCheck(bool isChecked) {
		GraphicsSettings settings = GraphicsSettings.instance;
		GraphicsSettingsData data = settings.Data;
		data.colorGradingEnabled = isChecked;
		settings.Data = data;
	}

	public void OnTextureQualityIncrease() {
		GraphicsSettings settings = GraphicsSettings.instance;
		GraphicsSettingsData data = settings.Data;
		if (data.textureQuality < 3) {
			data.textureQuality = ++data.textureQuality;
			settings.Data = data;
		}
		TextureQualityTextChange (data.textureQuality);
	}

	public void OnTextureQualityDecrease() {
		GraphicsSettings settings = GraphicsSettings.instance;
		GraphicsSettingsData data = settings.Data;
		if (data.textureQuality > 0) {
			data.textureQuality = --data.textureQuality;
			settings.Data = data;
		}
		TextureQualityTextChange (data.textureQuality);
	}

	public void TextureQualityTextChange(int quality) {
		switch (quality) {
		case 0:
			textureQualityText.text = "Ultra";
			break;
		case 1:
			textureQualityText.text = "High";
			break;
		case 2:
			textureQualityText.text = "Medium";
			break;
		case 3:
			textureQualityText.text = "Low";
			break;
		}
	}

	public void SelectSave(int index) {
		Destroy (destroySingletons);
		Instantiate (player);
		GraphicsSettings.CheckLights ();
	}

	public void OnSelectSave0(){
		SelectSave (0);
	}

	public void OnSelectSave1(){
		SelectSave (1);
	}

	public void OnSelectSave2(){
		SelectSave (2);
	}

	public void OnSelectSave3(){
		SelectSave (3);
	}

	public void OnSelectSave4(){
		SelectSave (4);
	}

	public void OnSelectSave5(){
		SelectSave (5);
	}

	public void OnSelectSave6(){
		SelectSave (6);
	}

	public void OnSelectSave7(){
		SelectSave (7);
	}

	public void OnSelectSave8(){
		SelectSave (8);
	}

	public void OnSelectSave9(){
		SelectSave (9);
	}

	public void OnSelectSave10(){
		SelectSave (10);
	}

	public void OnSelectSave11(){
		SelectSave (11);
	}

}
