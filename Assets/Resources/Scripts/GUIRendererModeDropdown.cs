﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIRendererModeDropdown : MonoBehaviour {

	public Dropdown dropdown;
	public GameObject fastUI;
	public GameObject advancedUI;

	public void Change() {
		if (dropdown.value == 0) { // 0 == FAST
			MaterialManager.instance.ChangeMode(MaterialMode.FAST);
			GraphicsSettings.instance.Data = GraphicsSettings.instance.Data;
			fastUI.SetActive (true);
			advancedUI.SetActive (false);
		} else { // >1 == ADVANCED
			MaterialManager.instance.ChangeMode(MaterialMode.ADVANCED);
			GraphicsSettings.instance.Data = GraphicsSettings.instance.Data;
			fastUI.SetActive (false);
			advancedUI.SetActive (true);
		}
	}

}
