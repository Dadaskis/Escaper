using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIDestroyPlayer : MonoBehaviour {

	public void OnPress() {
		Destroy (Player.instance.gameObject);
	}

}
