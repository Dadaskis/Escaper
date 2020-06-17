using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class InGameMenu : MonoBehaviour {

	public enum CurrentMenu {
		MAIN_MENU,
		SETTINGS,
		LOAD,
		SAVE
	}

	public CurrentMenu currentMenu = CurrentMenu.MAIN_MENU;

	public static InGameMenu instance;

	public FirstPersonController controller;
	public bool isOpened = false;

	void Awake() {
		instance = this;
	}

	void OnKeyPressed(KeyCode code) {
		//Debug.Log (code);
		if (isListeningSettingsButton) {
			InputManager.instance.keys [listeningButton] = code;
			isListeningSettingsButton = false;
			listeningButton = "";
		}
	}

	void Start() {
		InputManager.instance.onKeyPressed.AddListener (OnKeyPressed);
	}

	void Update() {
		if (InputManager.GetButtonDown ("OpenMenu")) {
			if (controller == null) {
				controller = FindObjectOfType<FirstPersonController> ();
				//return;
			}
			if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name == "Main menu") {
				isOpened = false;
				return;
			}

			isOpened = !isOpened;
			if (controller != null) {
				controller.enableMouseLook = !isOpened;
				controller.mouseLook.SetCursorLock (!isOpened);
			} else {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
			if (isOpened) {
				//Time.timeScale = 0.0f;
				currentMenu = CurrentMenu.MAIN_MENU;
			} else {
				//Time.timeScale = 1.0f;
			}
		}
	}

	void RenderMainMenu() {
		int width = Screen.width;
		int height = Screen.height;

		/*if (GUI.Button (new Rect (width * 0.3f, height * 0.2f, width * 0.45f, height * 0.08f), "Save game")) {
			currentMenu = CurrentMenu.SAVE;
		}

		if (GUI.Button (new Rect (width * 0.3f, height * 0.3f, width * 0.45f, height * 0.08f), "Load game")) {
			currentMenu = CurrentMenu.LOAD;
		}*/

		if (GUI.Button (new Rect (width * 0.3f, height * 0.5f, width * 0.45f, height * 0.08f), "Settings")) {
			currentMenu = CurrentMenu.SETTINGS;
		}

		if (GUI.Button (new Rect (width * 0.3f, height * 0.8f, width * 0.45f, height * 0.08f), "Exit to main menu")) {
			UnityEngine.SceneManagement.SceneManager.LoadScene ("Main menu");
		}

		if (GUI.Button (new Rect (width * 0.3f, height * 0.9f, width * 0.45f, height * 0.08f), "Exit to desktop")) {
			Application.Quit ();
		}
	}

	public string TextureQualityTextChange(int quality) {
		switch (quality) {
		case 0:
			return "Ultra";
			break;
		case 1:
			return "High";
			break;
		case 2:
			return "Medium";
			break;
		case 3:
			return "Low";
			break;
		}
		return "WTF are you doing";
	}

	public string ShaderQualityTextChange(MaterialShaderQuality quality) {
		switch (quality) {
		case MaterialShaderQuality.VERY_LOW:
			return "Very Low";
			break;
		case MaterialShaderQuality.LOW:
			return "Low";
			break;
		case MaterialShaderQuality.LOW_PLUS:
			return "Low+";
			break;
		case MaterialShaderQuality.MEDIUM:
			return "Medium";
			break;
		case MaterialShaderQuality.HIGH:
			return "High";
			break;
		case MaterialShaderQuality.ULTRA:
			return "Ultra";
			break;
		}
		return "WTF are you doing";
	}

	public Vector2 settingsScrollPosition = new Vector2();
	public bool isListeningSettingsButton = false;
	public string listeningButton = "";

	void RenderSettings() {
		int width = Screen.width;
		int height = Screen.height;

		float groupXBegin = width * 0.2f;
		float groupYBegin = height * 0.1f;
		float groupWidth = width * 0.7f;
		float groupHeight = height * 0.8f;

		bool isChanged = false;

		settingsScrollPosition = GUI.BeginScrollView (
			new Rect (groupXBegin, groupYBegin, groupWidth, groupHeight), 
			settingsScrollPosition, 
			new Rect (0.0f, 0.0f, groupWidth * 0.8f, groupHeight * 1.8f)
		);

		GraphicsSettingsData data = GraphicsSettings.instance.Data;

		float heightProcent = 0.0f;

		Debug.Log (data);

		data.bloomEnabled = GUI.Toggle (
			new Rect (0.0f, groupHeight * heightProcent, groupWidth, groupHeight * 0.05f), 
			data.bloomEnabled, 
			"Bloom"
		);
		heightProcent += 0.06f;

		data.chromaticAberrationEnabled = GUI.Toggle (
			new Rect (0.0f, groupHeight * heightProcent, groupWidth, groupHeight * 0.05f), 
			data.chromaticAberrationEnabled, 
			"Chromatic Aberration"
		);
		heightProcent += 0.06f;

		data.colorGradingEnabled = GUI.Toggle (
			new Rect (0.0f, groupHeight * heightProcent, groupWidth, groupHeight * 0.05f), 
			data.colorGradingEnabled, 
			"Color Grading"
		);
		heightProcent += 0.06f;

		data.grainEnabled = GUI.Toggle (
			new Rect (0.0f, groupHeight * heightProcent, groupWidth, groupHeight * 0.05f), 
			data.grainEnabled, 
			"Grain"
		);
		heightProcent += 0.06f;

		data.vignetteEnabled = GUI.Toggle (
			new Rect (0.0f, groupHeight * heightProcent, groupWidth, groupHeight * 0.05f), 
			data.vignetteEnabled, 
			"Vignette"
		);
		heightProcent += 0.06f;

		data.enableRealtimeShadows = GUI.Toggle (
			new Rect (0.0f, groupHeight * heightProcent, groupWidth, groupHeight * 0.05f), 
			data.enableRealtimeShadows, 
			"Realtime shadows"
		);
		heightProcent += 0.06f;

		data.enableAmbientOcclusion = GUI.Toggle (
			new Rect (0.0f, groupHeight * heightProcent, groupWidth, groupHeight * 0.05f), 
			data.enableAmbientOcclusion, 
			"Ambient Occlusion"
		);
		heightProcent += 0.06f;

		GUI.Label (new Rect (0.0f, groupHeight * heightProcent, groupWidth, groupHeight * 0.05f), "Texture quality");
		GUI.Label (new Rect (groupWidth * 0.4f, groupHeight * heightProcent, groupWidth * 0.2f, groupHeight * 0.05f), TextureQualityTextChange(data.textureQuality));
		if (GUI.Button (new Rect (groupWidth * 0.3f, groupHeight * heightProcent, groupWidth * 0.06f, groupHeight * 0.06f), "<")) {
			if (data.textureQuality < 3) {
				++data.textureQuality;
			}
		}
		if (GUI.Button (new Rect (groupWidth * 0.5f, groupHeight * heightProcent, groupWidth * 0.06f, groupHeight * 0.06f), ">")) {
			if (data.textureQuality > 0) {
				--data.textureQuality;
			}
		}
		heightProcent += 0.06f;


		GUI.Label (new Rect (0.0f, groupHeight * heightProcent, groupWidth, groupHeight * 0.05f), "Shaders quality");
		GUI.Label (new Rect (groupWidth * 0.4f, groupHeight * heightProcent, groupWidth * 0.2f, groupHeight * 0.05f), ShaderQualityTextChange(data.shadersQuality));
		if (GUI.Button (new Rect (groupWidth * 0.3f, groupHeight * heightProcent, groupWidth * 0.06f, groupHeight * 0.06f), "<")) {
			if (data.shadersQuality > MaterialShaderQuality.VERY_LOW) {
				--data.shadersQuality;
			}
		}
		if (GUI.Button (new Rect (groupWidth * 0.55f, groupHeight * heightProcent, groupWidth * 0.06f, groupHeight * 0.06f), ">")) {
			if (data.shadersQuality < MaterialShaderQuality.ULTRA) {
				++data.shadersQuality;
			}
		}
		heightProcent += 0.06f;


		GraphicsSettings.instance.Data = data;

		float offsetValue = groupHeight * 0.1f;
		float counter = 10.0f * heightProcent;
		foreach (KeyValuePair<string, KeyCode> pair in InputManager.instance.keys) {
			if (InputManager.instance.normalNames [pair.Key].Length == 0) {
				continue;
			}

			GUI.Label (
				new Rect (groupWidth * 0.4f, groupHeight * 0.3f + (offsetValue * counter), groupWidth * 0.6f, groupHeight * 0.05f), 
				InputManager.instance.normalNames[pair.Key]
			);

			if (GUI.Button (
				   new Rect (groupWidth * 0.1f, groupHeight * 0.3f + (offsetValue * counter), groupWidth * 0.26f, groupHeight * 0.06f), 
					listeningButton == pair.Key ? "---" : System.Enum.GetName (typeof(KeyCode), pair.Value)
			   )) {
				isListeningSettingsButton = true;
				listeningButton = pair.Key;
			}

			counter++;
		}

		GUI.EndScrollView ();
	}

	void RenderSave() {
		
	}

	void RenderLoad() {

	}

	void OnGUI() {
		if (isOpened) {
			int width = Screen.width;
			int height = Screen.height;

			//InventorySystem.instance.isEnabled = false;
			GUI.Box (new Rect (0, 0, width, height), "");

			switch (currentMenu) {
			case CurrentMenu.MAIN_MENU:
				RenderMainMenu ();
				break;
			case CurrentMenu.SETTINGS:
				RenderSettings ();
				break;
			case CurrentMenu.SAVE:
				RenderSave ();
				break;
			case CurrentMenu.LOAD:
				RenderLoad ();
				break;
			}
		}
	}

}
