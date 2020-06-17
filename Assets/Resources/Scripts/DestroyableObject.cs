using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DestroyableObjectSaveData {
	public int health = 100;
	public bool destroyed = false;
}

public class DestroyableObject : MonoBehaviour {

	public int health = 100;
	public bool destroyed = false;

	public class DestroyEvent : UnityEvent {}
	public DestroyEvent onDestroyEvent = new DestroyEvent();

	public void Damage(int damage) {
		if (destroyed) {
			return;
		}
		health -= damage;
		if (health <= 0) {
			destroyed = true;
			onDestroyEvent.Invoke ();
			this.gameObject.SetActive (false);
		}
	}

	public DestroyableObjectSaveData Save() {
		DestroyableObjectSaveData save = new DestroyableObjectSaveData ();
		save.destroyed = destroyed;
		save.health = health;
		return save;
	}

	public void Load(DestroyableObjectSaveData save){
		destroyed = save.destroyed;
		health = save.health;

		if (destroyed) {
			this.gameObject.SetActive (false);
		}
	}

}
