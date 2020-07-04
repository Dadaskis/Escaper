using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUITab : MonoBehaviour {

	public GUITabMemory memory;
	public GameObject tabElement;

	public void Change() {
		memory.Change (tabElement);
	}

}
