using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResourceUtility : MonoBehaviour {

	public static List<T> FindAllObjectsOfType<T>() {
		List<T> results = new List<T> ();
		for (int index = 0; index < SceneManager.sceneCount; index++) {
			Scene scene = SceneManager.GetSceneAt (index);
			if (scene.isLoaded) {
				GameObject[] allGameObjects = scene.GetRootGameObjects ();
				for(int rootIndex = 0; rootIndex < allGameObjects.Length; rootIndex++) {
					GameObject obj = allGameObjects [rootIndex];
					Debug.Log ("[Serializer] Checking root game object in scene. " + rootIndex + " " + obj.name + " " + obj.activeInHierarchy);
					T[] components = obj.GetComponentsInChildren<T> (true);
					results.AddRange(components);
				}
			}
		}
		foreach (GameObject obj in MarkAsNotDeletableOnLoad.objects) {
			if (obj != null) {
				T[] components = obj.GetComponentsInChildren<T> (true);
				results.AddRange (components);
			}
		}
		return results;
	}

}
