using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class CursorReturner : MonoBehaviour {

	public void Work() {
		FirstPersonController controller = Player.instance.GetComponent<FirstPersonController> ();
		if (controller != null) {
			controller.enableMouseLook = true;
			controller.mouseLook.SetCursorLock (true);
		} 
	}

}
