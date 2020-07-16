using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracerObject : MonoBehaviour {

	public LineRenderer line;
	public float checkDistance = 10.0f;
	public int resolution = 10;

	private float timer = 0.0f;
	public float killTime = 0.2f;
	private float alpha = 1.0f;
	public float alphaDisappearPower = 5.0f;
	private Material material;

	void Start() {
		material = line.material;
	}

	void Update() {
		timer += Time.deltaTime;
		if (timer > killTime) {
			alpha -= Time.deltaTime * alphaDisappearPower;
			material.SetColor ("_TintColor", new Color (1.0f, 1.0f, 1.0f, alpha));
			if (alpha <= 0) {
				Destroy (gameObject);
			}
		}
	}

	public void SetLineSettings(Vector3 start, Vector3 end) {
		if (Vector3.Distance (start, end) > checkDistance) { 
			line.positionCount = resolution - 1;
			List<Vector3> positions = new List<Vector3> ();
			for (int currentResolution = 1; currentResolution < resolution - 1; currentResolution++) {
				positions.Add (Vector3.Lerp (start, end, (float)currentResolution / (float)resolution));
			}
			positions.Add (end);
			line.SetPositions (positions.ToArray());
		}
	}
}
