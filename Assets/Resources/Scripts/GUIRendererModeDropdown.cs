using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIRendererModeDropdown : MonoBehaviour {

	public Dropdown dropdown;

	public void Change() {
		if (dropdown.value == 0) { // 0 == FAST
			MaterialManager.instance.ChangeMode(MaterialMode.FAST);
		} else { // >1 == ADVANCED
			MaterialManager.instance.ChangeMode(MaterialMode.ADVANCED);
		}
	}

}
