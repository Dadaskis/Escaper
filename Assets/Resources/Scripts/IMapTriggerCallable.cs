using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMapTriggerCallable : MonoBehaviour {

	public string triggerName = "";

	void Start() {
		MapTriggerManager.AddCallable (this);

		CustomStart ();
	}

	public virtual void CustomStart() { }

	public virtual void Call() { }

	void OnDestroy() {
		MapTriggerManager.RemoveCallable (this);
	}

}
