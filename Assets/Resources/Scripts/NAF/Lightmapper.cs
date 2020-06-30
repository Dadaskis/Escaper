using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightmapper : MonoBehaviour {

	public float voxelSize = 1.0f;
	public GameObject voxelPrefab;

	public Bounds GetSceneBounds() {
		Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
		foreach (Renderer renderer in FindObjectsOfType<Renderer>()) {
			bounds.Encapsulate(renderer.bounds);
		}
		return bounds;
	}

	private List<GameObject> voxels = new List<GameObject>();
	void PlaceVoxels() {
		Bounds sceneBounds = GetSceneBounds ();
		Vector3 minimum = sceneBounds.min;
		Vector3 maximum = sceneBounds.max;
		Vector3 current = Vector3.zero;
		Vector3 vectorVoxelSize = new Vector3 (voxelSize, voxelSize, voxelSize);
		List<Vector3> positions = new List<Vector3> ();
		for (current.x = minimum.x; current.x <= maximum.x; current.x += voxelSize) {
			for (current.y = minimum.y; current.y <= maximum.y; current.y += voxelSize) {
				for (current.z = minimum.z; current.z <= maximum.z; current.z += voxelSize) {
					if (Physics.CheckBox (current, vectorVoxelSize / 2.0f)) {
						//GameObject voxel = Instantiate (voxelPrefab);
						//voxel.transform.localScale = vectorVoxelSize;
						//voxel.transform.position = current;
						positions.Add(current);
					}
				}
			}
		}

		foreach (Vector3 position in positions) {
			GameObject voxel = Instantiate (voxelPrefab);
			voxel.transform.localScale = vectorVoxelSize;
			voxel.transform.position = position;
		}
	}

	public void Bake() {
		PlaceVoxels ();
	}

	void Start () {
		Bake ();
	}

}
