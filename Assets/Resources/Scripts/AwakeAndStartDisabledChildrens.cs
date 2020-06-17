using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeAndStartDisabledChildrens : MonoBehaviour {
	void Awake () {
		gameObject.SendMessageToAll (true, "Awake");
	}

	void Start() {
		gameObject.SendMessageToAll (true, "Start");
	}
}
