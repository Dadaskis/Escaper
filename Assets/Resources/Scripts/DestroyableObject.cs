using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyableObject : IDamagable {

	public int health = 10;
	public GameObject appendOnDestroy;

	private bool destroyed = false;

	public override void Damage (int damage, Character damager) {
		if (destroyed) {
			return;
		}
		health -= damage;
		if (health < 0) {
			GameObject obj = Instantiate (appendOnDestroy);
			obj.transform.position = transform.position;
			destroyed = true;
			Destroy (gameObject);
		}
	}

}
