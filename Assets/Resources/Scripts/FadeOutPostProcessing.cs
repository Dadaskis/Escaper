using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutPostProcessing : MonoBehaviour {

	public static FadeOutPostProcessing instance;

	public PostProcessingCaller caller;
	public float timer = 0.0f;
	public bool started = false;

	void Awake() {
		instance = this;
	}

	public void FadeOut() {
		started = true;
		caller.enabled = true;
	}

	void Update() {
		if (started) {
			caller.material.SetFloat ("_CurrentTime", timer);
			timer += Time.deltaTime * 0.5f;
		}
	}

}
