using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIEnableObjectOnPress : MonoBehaviour {

	public GameObject obj;

	public void OnPress() {
		obj.SetActive (true);
	}

}
