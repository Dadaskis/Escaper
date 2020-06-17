using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour {
	private Color gizmosColor = new Color(255, 100, 255, 10);

	void OnDrawGizmos() {
		Gizmos.color = gizmosColor;
		Gizmos.DrawWireSphere (transform.position, 0.05f);
		Gizmos.DrawRay(transform.position, transform.forward);
	}
}
