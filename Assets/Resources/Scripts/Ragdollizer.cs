using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdollizer : MonoBehaviour {

	public Animator animator;

	void Start () {
		foreach (Rigidbody body in GetComponentsInChildren<Rigidbody>()) {
			body.isKinematic = true;
		}
	}

	public void EnableRagdoll() {
		foreach (Rigidbody body in GetComponentsInChildren<Rigidbody>()) {
			body.isKinematic = false;
		}
		animator.enabled = false;
	}

	public void FreezeRagdoll() {
		foreach (Rigidbody body in GetComponentsInChildren<Rigidbody>()) {
			body.isKinematic = true;
		}
	}
}
