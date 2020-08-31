using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events.GUIItemActionSimple {
	class Drop {}
}

public class GUIItemActionSimple : GUIItemActionBase {
	public override void Apply (GUIItemDropOutWindow window, GUIItem item) {
		base.Apply (window, item);

		window.CreateNewButton ("Drop", delegate {
			GameObject itemObject = Instantiate(item.physicalData.physicalPrefab);
			Transform head = Player.instance.character.raycaster;
			itemObject.transform.position = head.position + head.forward;

			EventManager.RunEventListeners<Events.GUIItemActionSimple.Drop>(item, itemObject);

			Destroy(item.gameObject);
		});
	}
}
