using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullifyTransform : MonoBehaviour {

	void LateUpdate() {
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
	}

}
