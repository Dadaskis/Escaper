using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIChangeScene : MonoBehaviour {

	public string sceneName = "";
	public float waitSeconds = 0.0f;

	IEnumerator ChangeSceneAfterSeconds() {
		yield return new WaitForSeconds (waitSeconds);
		SceneManager.LoadScene (sceneName);
	}

	public void ChangeScene() {
		StartCoroutine (ChangeSceneAfterSeconds ());
	}

}
