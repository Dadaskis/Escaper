using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIItemActionBase : MonoBehaviour {
	public string typeName = "actionBase";

	public void Register () {
		GUIDropOutActionRegister.instance.actions.Add (typeName, this);
	}

	public virtual void Apply(GUIItemDropOutWindow window, GUIItem item) { 
		window.CreateNewButton ("Close", delegate {});

		window.CreateNewButton ("Info", delegate {
			item.inventory.OpenItemInfoWindow(item);
		});
	}

	public virtual void Start() {
		Register ();
	}
}
