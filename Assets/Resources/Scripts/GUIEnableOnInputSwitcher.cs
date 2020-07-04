using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIEnableOnInputSwitcher : MonoBehaviour {

	public GUIDisableMouseLookOnInput input;
	public GameObject obj;

	public void Switch() {
		input.Switch ();
		obj.SetActive (false);
	}

}
