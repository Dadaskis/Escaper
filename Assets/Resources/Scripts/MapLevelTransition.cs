using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLevelTransition : MonoBehaviour {

	public string target;

	void OnTriggerEnter(Collider collider) {
		Debug.Log (collider.gameObject.name);
		if (collider.gameObject.tag == "Player") { 
			ResourceMapLoader.instance.ChangeMap (target);
		}
	}

}
