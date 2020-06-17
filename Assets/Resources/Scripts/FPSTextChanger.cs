using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSTextChanger : MonoBehaviour {
	public Text text;
	float timer = 0.0f;

	void Update () {
		if (timer > 0.5f) {
			text.text = "FPS: " + Mathf.RoundToInt(1.0f / Time.deltaTime).ToString();
			timer = 0.0f;
		}
		timer += Time.deltaTime;
	}

}
