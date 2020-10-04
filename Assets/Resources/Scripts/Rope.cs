using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Rope : MonoBehaviour {

	public LineRenderer lineRenderer;
	public Transform start;
	public Transform end;
	public float gravityPower = 1.0f;
	public int linesAmount = 3;
	public float smoothing = 2.0f;
	public float powerValue = 2.0f;
	public float gravityApplyingValue = 0.5f;
	public float windPower = 0.01f;

	float LinePower(int line) {
		return ((float)line) / ((float)linesAmount);
	}

	void UpdateRope() {
		List<Vector3> positions = new List<Vector3> ();

		Vector3 gravityTarget = new Vector3 (Mathf.Sin(Time.time) * windPower, gravityPower, Mathf.Cos(Time.time) * windPower);


		positions.Add (start.position);

		for (int line = 0; line < linesAmount; line++) {
			float power = LinePower (line);
			Vector3 point = Vector3.Lerp (start.position, end.position, power);
			power *= 2.0f;
			if (power > 1.0f) {
				power = 2.0f - power;
			}
			power = Mathf.Pow (power, powerValue) * smoothing;
			Vector3 newPoint = Vector3.Lerp(point, new Vector3(point.x, point.y - gravityTarget.y, point.z), power);
			point.y = newPoint.y;
			point.x += Mathf.Lerp(0.0f, gravityTarget.x, power);
			point.z += Mathf.Lerp(0.0f, gravityTarget.z, power);
			positions.Add (point);
		}

		//for (int line = 0; line < linesAmount; line++) {
		//	float power = LinePower (line);
		//	Vector3 point = Vector3.Lerp (halfway, end.position, power);
		//	point = Vector3.Lerp(gravityTarget, point, (1.0f - power));
		//	positions.Add (point);
		//}

		positions.Add (end.position);

		lineRenderer.positionCount = positions.Count;
		lineRenderer.SetPositions (positions.ToArray());
	}

	#if UNITY_EDITOR 
	void OnRenderObject() {
	#else 
	void Update () {
	#endif
		UpdateRope();
	}
}
