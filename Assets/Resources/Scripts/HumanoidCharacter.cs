using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanoidCharacter : INonPlayerCharacter {

	public NavMeshAgent agent;
	public Animator animator;
	public bool enableLeftHandIKWeight = false;
	public bool enableRightHandIKWeight = false;

	private float leftHandIKWeight = 0.0f;
	private float rightHandIKWeight = 0.0f;


	public virtual EventData GoOnHit(EventData args) {
		Vector3 position = args.Get<Vector3> (0);
		Move (position);
		return null;
	}

	public virtual void Start() {
		EventManager.AddEventListener<Events.RaycastFirearmBulletHit> (GoOnHit);
	}

	public virtual void Update() {

	}

	public virtual void OnAnimatorIK(int layerIndex) {

	}

	public override void Move(Vector3 position) {
		agent.destination = position;
	}



}
