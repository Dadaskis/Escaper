using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTriggerCaller : MonoBehaviour {

	public bool active = true;
	public string triggerName;

	public void Call() {
		if (active) {
			MapTriggerManager.Call (triggerName);
		}
	}

}
