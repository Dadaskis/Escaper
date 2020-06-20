using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUICloseButton : MonoBehaviour {

	public GameObject objectToDisable;

	public void Close() {
		objectToDisable.SetActive (false);
	}

}
