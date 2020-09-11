using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoint : MonoBehaviour {
	
	void Start () {
		PatrolPointsManager.AddPoint (transform.position);
	}

	private Color gizmosColor = new Color(255, 100, 255, 10);

	void OnDrawGizmos() {
		Gizmos.color = gizmosColor;
		Gizmos.DrawWireSphere (transform.position, 0.05f);
	}
}
