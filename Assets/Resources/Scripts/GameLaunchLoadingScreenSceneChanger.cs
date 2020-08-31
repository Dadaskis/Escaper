using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLaunchLoadingScreenSceneChanger : MonoBehaviour {

	public float delay = 1.0f;
	public float timer = 0.0f;
	public bool loading = false;
	public string sceneName = "";

	IEnumerator LoadScene() {
		AsyncOperation operation = SceneManager.LoadSceneAsync (sceneName);
		while (true) {
			Debug.LogError (operation.progress);
			if (operation.isDone) {
				break;
			}
			yield return new WaitForSeconds (0.1f);
		}
	}

	void Update () {
		timer += Time.deltaTime;
		if (timer < delay || loading) {
			return;
		}
		loading = true;
		StartCoroutine (LoadScene ());
	}
}
