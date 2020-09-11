using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInPostProcessing : MonoBehaviour {

	public PostProcessingCaller caller;
	float timer = 0.0f;
	bool active = true;

	void Start () {
		caller.enabled = true;
	}

	void Update() {
		timer += Time.deltaTime * 0.5f;
		caller.material.SetFloat ("_CurrentTime", timer);
		if (timer > 3.0f) {
			caller.enabled = false;
			Destroy (this);
		}
	}
}
