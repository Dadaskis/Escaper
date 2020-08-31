using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SyncAnimatorData {
	public string stateName;
	public Animator animator;
}

public class PlaySyncAnimators : MonoBehaviour {

	public List<SyncAnimatorData> animators;

	void Start() {
		foreach (SyncAnimatorData data in animators) {
			Animator animator = data.animator;
			animator.Play (data.stateName, -1, 0.0f);
		}
	}

}
