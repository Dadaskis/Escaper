using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Character : MonoBehaviour {

	public string faction;
	public bool godMode;
	public bool killed = false;
	public Transform raycaster;
	public IWeaponChanger weapon;

	[SerializeField]
	private int health = 100;
	public int Health {
		get {
			return health;
		}
		set {
			health = value;
			onHealthChange.Invoke (health);
		}
	}

	[SerializeField]
	private int armor = 0;
	public int Armor {
		get {
			return armor;
		}
		set {
			armor = value;
			onArmorChange.Invoke (armor);
		}
	}

	public class HealthChangeEvent : UnityEvent<int> {}
	public HealthChangeEvent onHealthChange = new HealthChangeEvent ();

	public class ArmorChangeEvent : UnityEvent<int> {}
	public ArmorChangeEvent onArmorChange = new ArmorChangeEvent ();

	public class DeathEvent : UnityEvent {}
	public DeathEvent onDeath = new DeathEvent();

	public Ragdollizer ragdoll;

	private Image damageScreen;

	private bool rawRaycast = true;
	private RaycastHit[] raycastHits;

	void Start() {
		CharacterManager.Characters.Add (this);
	}

	void Update() {
		rawRaycast = true;
	}

	public void Damage(int count, Character character) {
		if (armor < 0) {
			health -= count;
			if (health < 0) {
				Kill ();
				onDeath.Invoke ();
				return;
			}
			onHealthChange.Invoke (health);
		} else {
			armor--;
			onArmorChange.Invoke (armor);
		}
	}

	public void FallDamage(float speed) {
		Debug.Log (speed);
		if (speed < 20.0f) {
			return;
		}
		int count = (int)((speed / 50.0f) * 100);
		health -= count;
		if (health < 0) {
			Kill ();
			onDeath.Invoke ();
			return;
		}
		onHealthChange.Invoke (health);
	}

	public void Kill() {
		Debug.Log ("Character killed. Faction: " + faction);
		if (!godMode) {
			killed = true;
			CharacterManager.Characters.Remove (this);
			if (ragdoll != null) {
				ragdoll.EnableRagdoll ();
			}
		}
	}

	public RaycastHit Raycast() {
		if (rawRaycast) {
			raycastHits = Physics.RaycastAll (raycaster.position, raycaster.forward, 1000000.0f);
			rawRaycast = false;
		}
		RaycastHit resultHit = new RaycastHit ();
		resultHit.distance = 1000000.0f;
		foreach (RaycastHit hit in raycastHits) {
			if (hit.transform.root.name != transform.root.name) {
				if (hit.distance < resultHit.distance) {
					resultHit = hit;
				}
			}
		}
		return resultHit;
	}

	public RaycastHit[] RaycastAll() {
		if (rawRaycast) {
			raycastHits = Physics.RaycastAll (raycaster.position, raycaster.forward, 1000000.0f);
			rawRaycast = false;
		}
		return raycastHits;
	}

}
