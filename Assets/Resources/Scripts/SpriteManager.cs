using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpriteManager : MonoBehaviour {

	public static SpriteManager instance;

	public Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite> ();
	public List<string> spritesFoldersToCheck;

	void Awake() {
		foreach (string folderToCheck in spritesFoldersToCheck) {
			Sprite[] objects = Resources.LoadAll (folderToCheck, typeof(Sprite)).Cast<Sprite> ().ToArray ();
			foreach (Sprite obj in objects) {
				sprites [obj.name] = obj;
				Debug.Log ("[SpriteManager] Loaded sprite: " + obj.name);
			}
		}
		instance = this;
	}

	public static Sprite GetSprite(string name) {
		Sprite sprite;
		if(instance.sprites.TryGetValue(name, out sprite)) {
			return sprite;
		} else {
			Debug.LogError("[SpriteManager] Cant get sprite: " + name);
		}
		return null;
	}

}
