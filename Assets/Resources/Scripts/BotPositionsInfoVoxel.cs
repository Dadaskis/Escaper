using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPositionsInfoVoxel : MonoBehaviour {

	public Color gizmosColor = new Color(1.0f, 0.2f, 1.0f, 1.0f);
	public bool isCover = false;
	public Vector3 position;

	public void Identify(float size = 1.0f, float coverMultiplier = 0.5f, float characterCoverHeight = 0.5f, float minCoverPower = 0.8f, float coverDistance = 3.0f) {
		gizmosColor = new Color (0.1f, 1.0f, 0.2f, 1.0f);
		isCover = false;

		List<GameObject> voxels = BotPositionsInfo.GetVoxels ();
		foreach (GameObject voxel in voxels) {
			BotPositionsInfoVoxel info = voxel.GetComponent<BotPositionsInfoVoxel> ();
			if (info != null) {
				if (info.isCover) {
					if(Vector3.Distance(transform.position, voxel.transform.position) < coverDistance) {
						return;
					}
				}
			}
		}

		RaycastHit hit;

		float distance = 999999.0f;

		if (Physics.Raycast (transform.position + new Vector3(0.0f, characterCoverHeight, 0.0f), Vector3.forward, out hit)) {
			distance = Mathf.Min (hit.distance, distance);
		}

		if (Physics.Raycast (transform.position + new Vector3(0.0f, characterCoverHeight, 0.0f), -Vector3.forward, out hit)) {
			distance = Mathf.Min (hit.distance, distance);
		}

		if (Physics.Raycast (transform.position + new Vector3(0.0f, characterCoverHeight, 0.0f), Vector3.right, out hit)) {
			distance = Mathf.Min (hit.distance, distance);
		}

		if (Physics.Raycast (transform.position + new Vector3(0.0f, characterCoverHeight, 0.0f), -Vector3.right, out hit)) {
			distance = Mathf.Min (hit.distance, distance);
		}

		if (distance < size * coverMultiplier) {
			float coverPower = (size * coverMultiplier) / distance;
			if (coverPower > minCoverPower) {
				gizmosColor = new Color (coverPower, 0.0f, 0.0f, 1.0f);
				isCover = true;
			}
		}

		if (Physics.Raycast (transform.position + new Vector3(0.0f, characterCoverHeight, 0.0f), -Vector3.up, out hit)) {
			position = hit.point;
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = gizmosColor;
		//Gizmos.DrawWireSphere (transform.position, 0.05f);
		//Gizmos.DrawRay(transform.position, transform.forward);
		Gizmos.DrawWireCube (transform.position, transform.localScale);
	}

}
