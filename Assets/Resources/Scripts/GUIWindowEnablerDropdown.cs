using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIWindowEnablerDropdown : MonoBehaviour {

	public GUIWindowEnablerMemory memory;
	public Dropdown dropdown;
	public List<GameObject> gameObjects = new List<GameObject>();

	public void Enable() {
		memory.Enable (gameObjects [dropdown.value]);
	}
}
