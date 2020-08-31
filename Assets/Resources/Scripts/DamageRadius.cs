using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRadius : MonoBehaviour {

	public float damageDistance = 5.0f;
	public float pushDistance = 20.0f;
	public float damage = 200.0f;
	public float explosionPhysicsForce = 20.0f;

	void Start() {
		IDamagable[] damagableObjects = FindObjectsOfType<IDamagable> ();
		foreach (IDamagable damagableObject in damagableObjects) {
			float distance = Vector3.Distance (transform.position, damagableObject.transform.position);
			if (distance < damageDistance) {
				float procent = 1.0f - (distance / damageDistance);
				damagableObject.Damage ((int) (damage * procent), null);
			}
		}

		Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody> ();
		foreach (Rigidbody body in rigidbodies) {
			body.AddExplosionForce (explosionPhysicsForce, transform.position, pushDistance);
		}
	}

}
