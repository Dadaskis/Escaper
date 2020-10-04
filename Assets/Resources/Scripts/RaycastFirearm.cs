using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events {
	class RaycastFirearmStartShoot {}
	class RaycastFirearmBulletHit {}
	class RaycastFirearmStartReload {}
	class RaycastFirearmEndReload {}
}

public class RaycastFirearm : IFirearm {
	public float maxXRecoil = 12.0f;
	public float minXRecoil = 2.0f;
	public float maxYRecoil = 5.0f;
	public float minYRecoil = 2.0f;
	public float maxZRecoil = 2.0f;
	public float minZRecoil = 2.0f;
	public float hitRigidbodyForceMultiplier = 1.0f;
	public GameObject tracerPrefab;
	public GameObject decalPrefab;
	public GameObject particlesPrefab;
	public float hitVolume = 1.0f;
	public float hitMinDistance = 1.0f;
	public float hitMaxDistance = 12.0f;

	public List<GameObject> onHitObjects;

	public override void FirearmStart () {

	}

	public override void FirearmUpdate () {

	}

	public override void Shoot () {
		EventData eventData = EventManager.RunEventListeners<Events.RaycastFirearmStartShoot> (this);
		if (eventData.Count > 0) {
			return;
		}

		EventManager.RunEventListeners<Events.IWeapon.Shot> (this);

		if (firstPerson) {
			CameraPuncher.instance.Punch (
				new Vector3 (
					Random.Range (-maxXRecoil, minXRecoil), 
					Random.Range (-maxYRecoil, minYRecoil), 
					Random.Range (-maxZRecoil, minZRecoil)
				)
			);

			RaycastHit hit = this.owner.Raycast ();

			Vector3 hitPos = hit.point;

			GameObject tracer = Instantiate (tracerPrefab);
			TracerObject tracerObject = tracer.GetComponent<TracerObject> ();
			tracerObject.SetLineSettings (this.owner.raycaster.position, hitPos);

			IDamagable part = hit.collider.GetComponent<IDamagable> ();
			
			if (part == null) {
				GameObject decalObject = Instantiate (decalPrefab);
				decalObject.transform.position = hitPos;
				decalObject.transform.rotation = Quaternion.LookRotation (-hit.normal);
				_Decal.DecalBuilder.BuildAndSetDirty (decalObject.GetComponent<_Decal.Decal> ());
				Destroy (decalObject, 60.0f);

				GameObject particlesObject = Instantiate (particlesPrefab);
				particlesObject.transform.position = hitPos;
				particlesObject.transform.rotation = Quaternion.LookRotation (hit.normal);
				ParticleSystem particles = particlesObject.GetComponent<ParticleSystem> ();
				particles.Play ();
				Destroy (particlesObject, particles.main.duration * 2.0f);

				Material material = MaterialManager.instance.GetMaterialFromRaycast (hit);
				if (material != null) {
					SoundMaterialType type = SoundManager.instance.GetSoundMaterialType (material);
					if (type != null) {
						List<string> clips;
						//if (landing) {
						//	clips = type.landingClipNames;
						//} else if (isRunning) {
						//	clips = type.runClipNames;
						//} else {
						clips = type.hitClipNames;
						//}
						if (clips.Count > 0) {
							string clip = clips [Random.Range (0, clips.Count)];
							SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData (clip);
							data.volume = hitVolume;
							data.minDistance = hitMinDistance;
							data.maxDistance = hitMaxDistance;
							SoundManager.instance.CreateSound (data, hit.point);
						}
					}
				} 

			} else {
				//part.character.ragdoll.EnableRagdoll ();
				part.Damage (damage, this.owner);
			}

			if (hit.rigidbody != null) {
				hit.rigidbody.AddForce (-hit.normal * hitRigidbodyForceMultiplier, ForceMode.Impulse);
			}

			EventManager.RunEventListeners<Events.RaycastFirearmBulletHit> (hitPos);
		} else {
			

			//RaycastHit hit;
			Transform raycaster = this.owner.raycaster;
			Vector3 direction = new Vector3 ();
			if (target != null) {
				if (overrideTargetHitPosition.magnitude == 0.0f) {
					direction = target.transform.position - raycaster.position;
				} else {
					direction = overrideTargetHitPosition - raycaster.position;
				}
			} else {
				if (overrideTargetHitPosition.magnitude == 0.0f) {
					direction = Vector3.forward;
				} else {
					direction = overrideTargetHitPosition - raycaster.position;
				}
			}

			RaycastHit[] rawHits = Physics.RaycastAll (raycaster.position, direction, 100000.0f);
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
						Material material = MaterialManager.instance.GetMaterialFromRaycast (hit);
						if (material != null) {
							SoundMaterialType type = SoundManager.instance.GetSoundMaterialType (material);
							if (type != null) {
								List<string> clips;
								//if (landing) {
								//	clips = type.landingClipNames;
								//} else if (isRunning) {
								//	clips = type.runClipNames;
								//} else {
								clips = type.hitClipNames;
								//}
								if (clips.Count > 0) {
									string clip = clips [Random.Range (0, clips.Count)];
									SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData (clip);
									data.volume = hitVolume;
									data.minDistance = hitMinDistance;
									data.maxDistance = hitMaxDistance;
									SoundManager.instance.CreateSound (data, hit.point);
								}
							}
						} 
						break;
					}

					CharacterDamagablePart part = hit.collider.GetComponent<CharacterDamagablePart> ();
					if (part != null) {
						part.Damage (damage, this.owner);
					} else {
						Vector3 hitPos = hit.point;

						GameObject tracer = Instantiate (tracerPrefab);
						TracerObject tracerObject = tracer.GetComponent<TracerObject> ();
						tracerObject.SetLineSettings (shootFireTransform.position, hitPos);
						//GameObject obj = Instantiate(onHitObjects [Random.Range (0, onHitObjects.Count)]);
						//obj.transform.position = hitPos;

						GameObject decalObject = Instantiate (decalPrefab);
						decalObject.transform.position = hitPos;
						decalObject.transform.rotation = Quaternion.LookRotation (-hit.normal);
						_Decal.DecalBuilder.BuildAndSetDirty (decalObject.GetComponent<_Decal.Decal> ());
						Destroy (decalObject, 60.0f);

						GameObject particlesObject = Instantiate (particlesPrefab);
						particlesObject.transform.position = hitPos;
						particlesObject.transform.rotation = Quaternion.LookRotation (hit.normal);
						ParticleSystem particles = particlesObject.GetComponent<ParticleSystem> ();
						particles.Play ();
						Destroy (particlesObject, particles.main.duration * 2.0f);

						EventManager.RunEventListeners<Events.RaycastFirearmBulletHit> (hitPos);
					}
				}
			}
		}

		currentAmmo--;
	}

	public override void SinglePrimaryFire () {

	}

	public override void SecondaryFire () {

	}

	public override void FirearmReloadAnimationEnd () {
		currentAmmo = fulfillAmmo;

		EventManager.RunEventListeners<Events.RaycastFirearmStartReload> (this);
	}

	public override void FirearmReloadEnd () {
		EventManager.RunEventListeners<Events.RaycastFirearmEndReload> (this);
	}

	public override bool FirearmReloadStart () {
		return true;
	}

	public override void CustomStartOnEnd () {

	}

	public override void PunchHit (RaycastHit hit) {
		CharacterDamagablePart part = hit.collider.GetComponent<CharacterDamagablePart> ();
		Debug.LogError ("Hit: " + hit.collider);
		if (part != null) {
			part.Damage (100000, this.owner);
		}
	}

	public override void PunchNotHit (RaycastHit hit) {
		Debug.LogError ("Not hit: " + hit.collider);
	}
}