using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUILoadWindowButton : MonoBehaviour {

	public string saveName;

	public void Load() {
		if (saveName.Length > 0) {
			StartCoroutine(Serializer.instance.Load (saveName));
		}
	}

}
