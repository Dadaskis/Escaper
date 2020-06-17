using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour {

	public static CoroutineManager instance;

	void Awake () {
		instance = this;
	}

	public static void ExecuteCoroutine(IEnumerator routine) {
		instance.StartCoroutine(routine);
	}
}
