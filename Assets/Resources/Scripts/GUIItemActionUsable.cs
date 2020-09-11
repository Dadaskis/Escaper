using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events.GUIItemActionUsable {
	class Use {}
}

public class GUIItemActionUsable : GUIItemActionSimple {

	public override void Apply (GUIItemDropOutWindow window, GUIItem item)
	{
		base.Apply (window, item);

		window.CreateNewButton ("Use", delegate {
			EventManager.RunEventListeners<Events.GUIItemActionUsable.Use>(item);
		});
	}

}
