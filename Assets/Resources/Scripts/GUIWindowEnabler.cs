using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIWindowEnabler : MonoBehaviour {
	public GUIWindowEnablerMemory memory;
	public GameObject enableObject;

	public void Enable() {
		memory.Enable (enableObject);
	}
}
