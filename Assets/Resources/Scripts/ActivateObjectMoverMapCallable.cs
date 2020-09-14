using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObjectMoverMapCallable : IMapTriggerCallable {

	public ObjectMover mover;

	public override void Call () {
		mover.StartMove ();
	}

	public override void CustomStart () { }

}
