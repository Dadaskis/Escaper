using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCallerMapCallable : IMapTriggerCallable {

	public MapTriggerCaller caller;

	public override void Call () {
		caller.active = true;
	}

	public override void CustomStart () { }

}
