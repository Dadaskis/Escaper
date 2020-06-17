using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkAsNotDeletableOnLoad : MonoBehaviour {
	public GameObject obj;
	public static List<GameObject> objects = new List<GameObject>();
	void Awake () {
		DontDestroyOnLoad (obj);
		objects.Add (obj);
	}
}
