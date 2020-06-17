using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {

	public List<Character> characters;
	public static CharacterManager instance;

	public static List<Character> Characters {
		get {
			return instance.characters;
		}
		set {
			instance.characters = value;
		}
	}

	void Awake () {
		instance = this;
	}
}
