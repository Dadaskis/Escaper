using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Events.HumanoidCharacter {
	class Shoot {}
}

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
	public float walkSmoothing = 5.0f;
	public string moveBoolString;
	public string velocityXFloatString;
	public string velocityYFloatString;
	public Transform walkVelocityObjectHelper;
	public BotCoverPosition currentCover;
	public float stepVolume = 1.0f;
	public float stepSoundMinDistance = 3.0f;
	public float stepSoundMaxDistance = 9.0f;
	public float stepDelay = 0.05f;
	public List<GameObject> bloodDecals = new List<GameObject>();

	public string currentState = "Awaiting";
	public string startingState = "Awaiting";

	//private Vector2 smoothDeltaPosition = Vector2.zero;
	private Vector3 velocity = Vector2.zero;
	private float stepTimer = 0.0f;
	private bool shouldMove = false;
	//private Vector3 previousRootPosition = Vector3.zero;
	//private Vector3 rootVelocity = Vector3.zero;

	public void ChangeState(string stateName) {
		if (currentState != "Awaiting") {
			stateMachineAI.SetTrigger ("ChangeState");
		}	
		stateMachineAI.SetTrigger (stateName);
		currentState = stateName;
	}

	public virtual EventData GoOnHit(EventData args) {
		//Vector3 position = args.Get<Vector3> (0);
		//Move (position);
		return null;
	}

	public EventData OnDeath(EventData args) {
		Destroy (stateMachineAI);
		Destroy (agent);
		if (currentCover != null) {
			currentCover.isUsedNow = false;
		}
		if (shooter != null) {
			if (shooter.weapon != null) {
				Rigidbody body = shooter.weapon.GetComponent<Rigidbody> ();
				if (body != null) {
					shooter.weapon.transform.SetParent (null, true);
					body.isKinematic = false;
					body.useGravity = true;
				}
			}
		}
		return new EventData ();
	}

	public EventData OnDamage(EventData args) {
		Vector3 offset = MathHelper.RandomVector3 (0.0f, 2.0f);
		offset.y = 0.0f;
		RaycastHit[] hits = Physics.RaycastAll (transform.position, Vector3.down + offset);
		RaycastHit resultHit = new RaycastHit();
		float minDistance = 999999.0f;
		bool isPlaceAssigned = false;
		foreach (RaycastHit hit in hits) {
			if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Map")) {
				float distance = Vector3.Distance (transform.position, hit.point);
				if (distance < minDistance) {
					minDistance = distance;
					resultHit = hit;
					isPlaceAssigned = true;
				}
			}
		}
		if (isPlaceAssigned) {
			GameObject decalPrefab = bloodDecals [Random.Range (0, bloodDecals.Count - 1)];
			GameObject decalObject = Instantiate (decalPrefab);
			decalObject.transform.position = resultHit.point;
			decalObject.transform.rotation = Quaternion.LookRotation (-resultHit.normal);
			_Decal.DecalBuilder.BuildAndSetDirty (decalObject.GetComponent<_Decal.Decal> ());
			Destroy (decalObject, 60.0f);
		}
		return new EventData ();
	}

	public virtual void Start() {
		EventManager.AddEventListener<Events.RaycastFirearmBulletHit> (GoOnHit);
		shooter = GetComponentInChildren<WeaponShooterTP> ();
		data.onDeath.AddListener(OnDeath);
		data.onDamage.AddListener (OnDamage);
		ChangeState (startingState);
	}

	void Update() {
		stepTimer += Time.deltaTime;
		if (agent != null) {
			shouldMove = velocity.magnitude > 0.1f && agent.remainingDistance > agent.radius && agent.remainingDistance > agent.stoppingDistance;
			walkVelocityObjectHelper.position = transform.position + agent.desiredVelocity;
			Vector3 newVelocity = walkVelocityObjectHelper.localPosition.normalized;
			velocity = Vector3.Lerp (velocity, newVelocity, walkSmoothing * Time.deltaTime);
			animator.SetBool (moveBoolString, shouldMove);
			animator.SetFloat (velocityXFloatString, velocity.x);
			animator.SetFloat (velocityYFloatString, velocity.z);
		}
	}

	public override void Move(Vector3 position) {
		agent.destination = position;
		if (currentCover != null) {
			currentCover.isUsedNow = false;
			currentCover = null;
		}
	}

	public void MoveToNearestCover() {
		//NavMeshHit hit;
		//agent.FindClosestEdge (out hit);
		//if (hit.hit) {
		//	agent.destination = hit.position + MathHelper.RandomVector3(0.1f, 0.3f);
		//}

		BotCoverPosition coverPosition = BotPositionsInfo.GetNearestCover (transform.position);
		if (coverPosition != null) {
			coverPosition.isUsedNow = true;
			agent.destination = coverPosition.position;
			currentCover = coverPosition;
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

		EventManager.RunEventListeners<Events.HumanoidCharacter.Shoot> (target, status);

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

	public void WalkSoundEvent() {
		//Debug.LogError ("WALK");
		if (stepTimer > stepDelay && shouldMove) {
			PlayFootStepAudio ();
			stepTimer = 0.0f;
		}
	}

	private void PlayFootStepAudio()
	{
		//if(m_FootstepSounds.GetLength(0) > 0) {
		// pick & play a random footstep sound from the array,
		// excluding sound at index 0
		//    int n = Random.Range(1, m_FootstepSounds.Length);
		//     m_AudioSource.clip = m_FootstepSounds[n];
		//     m_AudioSource.PlayOneShot(m_AudioSource.clip);
		// move picked sound to index 0 so it's not picked next time
		//    m_FootstepSounds[n] = m_FootstepSounds[0];
		//    m_FootstepSounds[0] = m_AudioSource.clip;
		//}

		RaycastHit[] hits = Physics.RaycastAll (transform.position + new Vector3(0.0f, 0.5f, 0.0f), Vector3.down, 2.4f);
		RaycastHit neededHit = new RaycastHit ();
		bool neededHitFound = false;
		foreach (RaycastHit hit in hits) {
			if (hit.transform.root.tag != "Character") {
				if (hit.transform.GetComponent<Renderer> () == null) {
					continue;
				}
				if (hit.triangleIndex == -1) {
					continue;
				}
				neededHit = hit;
				neededHitFound = true;
				break;
			}
		}
		if (neededHitFound) {
			Material material = MaterialManager.instance.GetMaterialFromRaycast (neededHit);
			if (material == null) {
				return;
			} 
			SoundMaterialType type = SoundManager.instance.GetSoundMaterialType (material);
			if (type != null) {
				List<string> clips;
				//if (landing) {
				//	clips = type.landingClipNames;
				//} else if (isRunning) {
				//	clips = type.runClipNames;
				//} else {
					clips = type.walkClipNames;
				//}
				string clip = clips [Random.Range (0, clips.Count)];
				SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData (clip);
				data.volume = stepVolume;
				data.spatialBlend = 1.0f;
				data.minDistance = stepSoundMinDistance;
				data.maxDistance = stepSoundMaxDistance;
				SoundManager.instance.CreateSound (data, transform.position, transform);
			}
		}
	}

}
