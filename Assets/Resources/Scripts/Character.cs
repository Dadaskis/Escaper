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

	//private bool isPlayer = false;
	private Image damageScreen;

	void Start() {
		CharacterManager.Characters.Add (this);
		/*if (GetComponent<Player> () != null) {
			isPlayer = true;
			damageScreen = Player.instance.damagePanel;
		}*/
	}

	//private float damageScreenResetSpeed = 5.0f;
	void Update() {
		/*if (isPlayer) {
			Color color = damageScreen.color;
			color.a = Mathf.Lerp (color.a, 0.0f, Time.deltaTime * damageScreenResetSpeed);
			damageScreen.color = color;
		}*/
	}

	public void Damage(int count, Character character) {
		if (armor < 0) {
			health -= count;
			if (health < 0) {
				Kill ();
				onDeath.Invoke ();
				return;
			}
			/*if (isPlayer) {
				Color color = Color.red;
				color.a = 0.3f * (100.0f / ((float)health));
				damageScreen.color = color;
			}*/
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
		///if (isPlayer) {
		//	Color color = Color.red;
		//	color.a = 0.3f * (100.0f / ((float)health));
		//	damageScreen.color = color;
		//}
		onHealthChange.Invoke (health);
	}

	public void Kill() {
		Debug.Log ("Character killed. Faction: " + faction);
		if (!godMode) {
			killed = true;
			CharacterManager.Characters.Remove (this);
			if (ragdoll != null) {
				ragdoll.EnableRagdoll ();
				//Timer.Create (ragdoll.FreezeRagdoll, "RagdollFreeze" + this.GetHashCode (), 3.0f, 1);
				//Destroy (gameObject, 10.0f);
				/*
				RaycastHit hit;
				if (Physics.Raycast (transform.position, -transform.up, out hit)) {
					Instantiate (boxAfterDeath, hit.point, Quaternion.identity);
				}
				*/
			} else {
				//Destroy (gameObject, 0.3f);
			}
		}
	}

	public RaycastHit Raycast() {
		RaycastHit[] hits = Physics.RaycastAll(raycaster.position, raycaster.forward, 1000000.0f);
		RaycastHit resultHit = new RaycastHit ();
		resultHit.distance = 1000000.0f;
		foreach (RaycastHit hit in hits) {
			if (hit.transform.root.name != transform.root.name) {
				if (hit.distance < resultHit.distance) {
					resultHit = hit;
				}
			}
		}
		return resultHit;
	}

}
