using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevMetricsTimerTrigger : MonoBehaviour {

	public static bool initialized = false;
	public bool thisGeneral = false;
	public static DevMetricsTimerTrigger instance;
	public static float timer = 0.0f;
	public static bool enabled = false;

	static void Switch() {
		if (enabled) {
			Debug.LogError ("Metrics. Timer: " + timer);
		} else {
			timer = 0.0f;
			Debug.LogError ("Metrics. Started timer.");
		}
		enabled = !enabled;
	}

	static void UpdateTimer() {
		if (enabled) {
			timer += Time.deltaTime;
		}
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.transform.gameObject.tag == "Player") {
			Switch ();
		}
	}

	void Awake () {
		if (!initialized) {
			instance = this;
			thisGeneral = true;
			initialized = true;
		}
	}

	void Update () {
		if (thisGeneral) {
			UpdateTimer ();
		}
	}

}
