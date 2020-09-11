using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdollizer : MonoBehaviour {

	public Animator animator;

	public float minVelocityToDisable = 0.01f;
	public float timer = 0.0f;
	public float delay = 1.0f;
	public bool enabled = false;
	public bool freezed = false;
	public Rigidbody[] bodies;

	void Start () {
		bodies = GetComponentsInChildren<Rigidbody> ();
		foreach (Rigidbody body in bodies) {
			body.isKinematic = true;
		}
	}

	void Update() {
		if (!enabled) {
			return;
		}

		if (freezed) {
			return;
		}

		timer += Time.deltaTime;
		if (timer > delay) {
			timer = 0.0f;
			float maxVelocity = 0.0f;
			foreach (Rigidbody body in bodies) {
				maxVelocity = Mathf.Max (maxVelocity, body.velocity.magnitude);
			}
			if (maxVelocity <= minVelocityToDisable) {
				FreezeRagdoll ();
			}
		}
	}

	public void EnableRagdoll() {
		if (enabled) {
			return;
		}

		foreach (Rigidbody body in bodies) {
			body.isKinematic = false;
		}
		animator.enabled = false;
		enabled = true;
	}

	public void FreezeRagdoll() {
		if (freezed) {
			return;
		}

		foreach (Rigidbody body in bodies) {
			body.isKinematic = true;
			body.Sleep ();
		}
		freezed = true;
	}
}
