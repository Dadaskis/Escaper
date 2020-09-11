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

			Rigidbody body = itemObject.GetComponent<Rigidbody>();
			if(body != null) {
				body.AddForce(head.forward, ForceMode.Impulse);
			}

			EventManager.RunEventListeners<Events.GUIItemActionSimple.Drop>(item, itemObject);

			item.ClearEverythingAfterItem();

			Destroy(item.gameObject);
		});
	}
}
