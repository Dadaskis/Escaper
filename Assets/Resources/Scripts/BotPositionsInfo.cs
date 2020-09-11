using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class BotCoverPosition {
	public Vector3 position = new Vector3();
	public bool isUsedNow = false;
}

public class BotPositionsInfo : MonoBehaviour {

	public float voxelSize = 1.0f;
	public float upDistanceCheckValue = 2.0f;
	public float coverMultiplier = 0.5f;
	public float characterCoverHeight = 0.5f;
	public float minCoverPower = 0.7f;
	public float coverDistanceToOptimize = 3.0f;
	public int coverSearchDistance = 10;
	public GameObject voxelPrefab;
	public List<BotCoverPosition> coversList;
	public Dictionary<int, List<BotCoverPosition>> covers = new Dictionary<int, List<BotCoverPosition>>();
	private List<GameObject> voxels = new List<GameObject>();

	public static BotPositionsInfo instance;

	public Bounds GetSceneBounds() {
		Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
		foreach (Renderer renderer in FindObjectsOfType<Renderer>()) {
			bounds.Encapsulate(renderer.bounds);
		}
		return bounds;
	}

	public void ClearVoxels() {
		for (int counter = 0; counter < 10; counter++) {
			for (int index = 0; index < transform.childCount; index++) {
				Transform voxelTransform = transform.GetChild (index);
				DestroyImmediate (voxelTransform.gameObject);
			}
		}
			
		voxels.Clear ();
	}

	public void PlaceVoxels() {
		ClearVoxels ();

		Bounds sceneBounds = GetSceneBounds ();
		Vector3 minimum = sceneBounds.min;
		Vector3 maximum = sceneBounds.max;
		Vector3 current = Vector3.zero;
		Vector3 vectorVoxelSize = new Vector3 (voxelSize, voxelSize, voxelSize);
		List<Vector3> positions = new List<Vector3> ();
		for (current.x = minimum.x; current.x <= maximum.x; current.x += voxelSize) {
			for (current.y = minimum.y; current.y <= maximum.y; current.y += voxelSize) {
				for (current.z = minimum.z; current.z <= maximum.z; current.z += voxelSize) {
					RaycastHit hit;
					if (Physics.Raycast (current, Vector3.down, out hit)) {
						if (hit.distance <= voxelSize) {
							RaycastHit secondHit;
							if (Physics.Raycast (current, Vector3.up, out secondHit)) {
								if (secondHit.distance > upDistanceCheckValue) {
									positions.Add (current);
								}
							}
						}
					}
				}
			}
		}

		foreach (Vector3 position in positions) {
			GameObject voxel = Instantiate (voxelPrefab, transform);
			voxel.transform.localScale = vectorVoxelSize;
			voxel.transform.position = position;
			voxels.Add (voxel);
		}

		IdentifyVoxels ();
	}

	public void IdentifyVoxels() {
		int counter = 0;
		foreach (GameObject voxel in voxels) {
			BotPositionsInfoVoxel info = voxel.GetComponent<BotPositionsInfoVoxel> ();
			if (info != null) {
				info.Identify (voxelSize, coverMultiplier, characterCoverHeight, minCoverPower, coverDistanceToOptimize);
				if (info.isCover) {
					counter++;

					BotCoverPosition cover = new BotCoverPosition ();
					cover.isUsedNow = false;
					cover.position = info.position;
					coversList.Add (cover);

					int distance = (int)(Vector3.Distance (Vector3.zero, cover.position));
					List<BotCoverPosition> positions;
					if (covers.TryGetValue (distance, out positions)) {
						positions.Add (cover);
					} else {
						positions = new List<BotCoverPosition> ();
						positions.Add (cover);
						covers [distance] = positions;
					}
				}
			}
		}
		Debug.LogError ("Covers count: " + counter);
	}

	public static BotCoverPosition GetNearestCover(Vector3 position) {
		int zeroDistance = (int)(Vector3.Distance (Vector3.zero, position));
		for (int counter = 0; counter < instance.coverSearchDistance; counter++) {			
			List<BotCoverPosition> coversList;
			if (instance.covers.TryGetValue (zeroDistance, out coversList)) {
				foreach (BotCoverPosition cover in coversList) {
					if (!cover.isUsedNow) {
						return cover;
					}
				}
			} else {
				zeroDistance++;
			}
		}
		zeroDistance = (int)(Vector3.Distance (Vector3.zero, position));
		for (int counter = 0; counter < instance.coverSearchDistance; counter++) {			
			List<BotCoverPosition> coversList;
			if (instance.covers.TryGetValue (zeroDistance, out coversList)) {
				foreach (BotCoverPosition cover in coversList) {
					if (!cover.isUsedNow) {
						return cover;
					}
				}
			} else {
				zeroDistance--;
			}
		}
		return null;
		/*BotCoverPosition nearestCover;
		float minDistance = 999999.0f;
		foreach (BotCoverPosition cover in instance.coversList) {
			if (!cover.isUsedNow) {
				float distance = Vector3.Distance (cover.position, position);
				if (minDistance < distance) {
					minDistance = distance;
					nearestCover = cover;
				}
			}
		}
		return nearestCover;*/
	}

	public void EnableVoxelsGizmos() {
		foreach (GameObject voxel in voxels) {
			voxel.SetActive (true);
		}
	}

	public void DisableVoxelsGizmos() {
		foreach (GameObject voxel in voxels) {
			voxel.SetActive (false);
		}
	}

	public void InitializeSingleton() {
		instance = this;
	}

	public static List<GameObject> GetVoxels() {
		return instance.voxels;
	}

	void Awake () {
		foreach (BotCoverPosition cover in coversList) {
			int distance = (int)(Vector3.Distance (Vector3.zero, cover.position));
			List<BotCoverPosition> positions;
			if (covers.TryGetValue (distance, out positions)) {
				positions.Add (cover);
			} else {
				positions = new List<BotCoverPosition> ();
				positions.Add (cover);
				covers [distance] = positions;
			}
		}
		InitializeSingleton ();
	}

}
