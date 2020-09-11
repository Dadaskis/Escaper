using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPointsManager : MonoBehaviour {

	public static PatrolPointsManager instance;
	public List<Vector3> patrolPoints = new List<Vector3>();

	void Awake() {
		instance = this;
	}

	public static void AddPoint(Vector3 point) {
		instance.patrolPoints.Add (point);
	}

	public static Vector3 GetPoint(Vector3 position = default(Vector3)) {
		Vector3 point = instance.patrolPoints[Random.Range(0, instance.patrolPoints.Count - 1)];
		return point;
	}

}
