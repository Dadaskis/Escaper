using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanoidCharacterVisionData {
	public List<Character> aliveEnemies = new List<Character> ();
	public List<Character> aliveFriends = new List<Character> ();
	public List<Character> deadEnemies = new List<Character> ();
	public List<Character> deadFriends = new List<Character> ();
	public List<Character> invisible = new List<Character> ();
	public List<Character> far = new List<Character> ();
	public List<Character> notInFOV = new List<Character> ();
}

public enum HumanoidCharacterShootStatus {
	HIT,
	MISS,
	RELOAD_NEEDED,
	JAMMED
}

public class HumanoidCharacter : INonPlayerCharacter {

	public NavMeshAgent agent;
	public Animator animator;
	public WeaponShooterTP shooter;
	public Character target;
	public Animator stateMachineAI;
	public int eachShootHit = 3;
	public int currentShoot = 0;

	public virtual EventData GoOnHit(EventData args) {
		//Vector3 position = args.Get<Vector3> (0);
		//Move (position);
		return null;
	}

	public void OnDeath() {
		Rigidbody body = shooter.weapon.GetComponent<Rigidbody> ();
		if (body != null) {
			shooter.weapon.transform.SetParent (null, true);
			body.isKinematic = false;
			body.useGravity = true;
		}
		stateMachineAI.enabled = false;
	}

	public virtual void Start() {
		EventManager.AddEventListener<Events.RaycastFirearmBulletHit> (GoOnHit);
		shooter = GetComponentInChildren<WeaponShooterTP> ();
		data.onDeath.AddListener (OnDeath);
	}		

	public override void Move(Vector3 position) {
		agent.destination = position;
	}

	public void MoveToNearestEdge() {
		NavMeshHit hit;
		agent.FindClosestEdge (out hit);
		if (hit.hit) {
			agent.destination = hit.position + MathHelper.RandomVector3(0.1f, 0.3f);
		}
	}

	public void StopMoving() {
		agent.destination = transform.position;
	}

	public void Chase(Character target, float minOffset = 0.3f, float maxOffset = 1.0f) {
		Vector3 position = transform.position;
		if (target != null) {
			position = target.transform.position;
		}
		Vector3 randomPlace = MathHelper.RandomVector3 (minOffset, maxOffset);
		randomPlace.y = target.transform.position.y;
		Move (position + randomPlace);
	}

	public HumanoidCharacterShootStatus Shoot(Character target) {
		currentShoot++;

		HumanoidCharacterShootStatus status = HumanoidCharacterShootStatus.MISS;

		if (currentShoot >= eachShootHit) {
			shooter.Shoot (target.gameObject, target.raycaster.position);
			status = HumanoidCharacterShootStatus.HIT;
			currentShoot = 0;
		} else {
			float X = Random.Range (0.2f, 0.5f);
			float Y = Random.Range (0.2f, 0.5f);
			float Z = Random.Range (0.2f, 0.5f);
			if (Random.Range (0, 100) > 50) {
				X = -X;
				Y = -Y;
				Z = -Z;
			}
			Vector3 offsetPosition = new Vector3 (X, Y, Z);
			shooter.Shoot (null, target.raycaster.position + offsetPosition);
		}

		if (shooter.weapon.currentAmmo <= 0) {
			status = HumanoidCharacterShootStatus.RELOAD_NEEDED;
		} else if (shooter.IsJammed ()) {
			status = HumanoidCharacterShootStatus.JAMMED;
		}

		return status;
	}

	public void Reload() {
		shooter.Reload ();
	}

	public void RotateToTheTarget(Vector3 position) {
		Vector3 direction = position - transform.position;
		direction.y = 0.0f;
		direction = direction.normalized;
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation (direction), Time.deltaTime * 5.0f);
	}

	public void ControlRotation() {
		agent.updateRotation = true;
	}

	public void DontControlRotation() {
		agent.updateRotation = false;
	}

	public bool IsOnDestination (float offset = 0.2f) {
		if (agent.remainingDistance < offset) {
			return true;
		}
		return false;
	}

	public HumanoidCharacterVisionData GetCharacterVision (float FOV = 90.0f, float visionDistance = 40.0f, float hearDistance = 10.0f) {
		HumanoidCharacterVisionData visionData = new HumanoidCharacterVisionData ();

		foreach (Character anotherCharacter in CharacterManager.Characters) {
			if (anotherCharacter == null) {
				continue;
			}

			if (Vector3.Distance (transform.position, anotherCharacter.transform.position) > visionDistance) {
				visionData.far.Add (anotherCharacter);
				continue;
			}

			if (Vector3.Angle (anotherCharacter.raycaster.position - data.raycaster.position, data.raycaster.forward) <= FOV 
					|| Vector3.Distance(transform.position, anotherCharacter.transform.position) < hearDistance) {
				Vector3 start = data.raycaster.position;
				Vector3 end = anotherCharacter.raycaster.position;
				RaycastHit[] rawHits = Physics.RaycastAll (start, end - start, visionDistance);
				if (rawHits.Length > 0) { 
					SortedDictionary<float, RaycastHit> hits = new SortedDictionary<float, RaycastHit> ();
					foreach (RaycastHit hit in rawHits) {
						hits [hit.distance] = hit;
					}
					foreach (KeyValuePair<float, RaycastHit> hitPair in hits) {
						RaycastHit hit = hitPair.Value;
						if (hit.transform.root == transform.root) {
							continue;
						} 
						if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Map")) {
							visionData.invisible.Add (anotherCharacter);
							break;
						}
						if (hit.collider.transform.root == anotherCharacter.transform.root) {
							bool friend = anotherCharacter.faction == data.faction;
							if (anotherCharacter.killed) {
								if (friend) {
									visionData.deadFriends.Add (anotherCharacter);
									break;
								} else {
									visionData.deadEnemies.Add (anotherCharacter);
									break;
								}
							} else {
								if (friend) {
									visionData.aliveFriends.Add (anotherCharacter);
									break;
								} else {
									visionData.aliveEnemies.Add (anotherCharacter);
									break;
								}
							}
						}
					}
				} 
			} else {
				visionData.notInFOV.Add (anotherCharacter);
			}
		}

		return visionData;
	}

}
