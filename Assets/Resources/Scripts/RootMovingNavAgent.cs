﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RootMovingNavAgent : MonoBehaviour {
	public Animator animator;
	public string moveBoolString;
	public string velocityXFloatString;
	public string velocityYFloatString;
	public NavMeshAgent navAgent;
	public float rootMotionSpeed = 40.0f;
	public float addSpeed = 1.0f;

	Vector2 smoothDeltaPosition = Vector2.zero;
	Vector2 velocity = Vector2.zero;
	Vector3 previousRootPosition = Vector3.zero;
	Vector3 rootVelocity = Vector3.zero;

	void Start() {
		//navAgent.updatePosition = false;
	}

	void Update() {
		/*
		Vector3 worldDeltaPosition = navAgent.nextPosition - transform.position;

		float dx = Vector3.Dot (transform.right, worldDeltaPosition);
		float dy = Vector3.Dot (transform.forward, worldDeltaPosition);
		Vector2 deltaPosition = new Vector2 (dx, dy);

		float smooth = Mathf.Min (1.0f, Time.deltaTime / 0.15f);
		smoothDeltaPosition = Vector2.Lerp (smoothDeltaPosition, deltaPosition, smooth);

		if (Time.deltaTime > 1e-5f) {
			velocity = smoothDeltaPosition / Time.deltaTime;
		}

		bool shouldMove = velocity.magnitude > 0.5f && navAgent.remainingDistance > navAgent.radius && navAgent.remainingDistance > navAgent.stoppingDistance;

		if (worldDeltaPosition.magnitude > navAgent.radius) {
			navAgent.nextPosition = transform.position + 0.9f * worldDeltaPosition;
		}

		animator.SetBool (moveBoolString, shouldMove);
		animator.SetFloat (velocityXFloatString, velocity.x);
		animator.SetFloat (velocityYFloatString, velocity.y);
		*/

		//if (worldDeltaPosition.magnitude > navAgent.radius) {
		//	transform.position = navAgent.nextPosition + 0.9f * worldDeltaPosition;
		//}
	}

	//private Vector3 tempRootPosition = Vector3.zero;

	void OnAnimatorMove() {
		//Vector3 position = animator.rootPosition;
		//position.y = navAgent.nextPosition.y;
		//transform.position = position;
		/*
		if (navAgent.velocity.y > 0.01f) {
			transform.position = navAgent.nextPosition;
		} else {
			Vector3 position = animator.rootPosition;
			position.y = navAgent.nextPosition.y;
			transform.position = position;
		}
		*/
		//transform.position = navAgent.nextPosition;
		//navAgent.speed = Mathf.Abs((tempRootPosition - animator.rootPosition).magnitude * rootMotionSpeed) + addSpeed;

		//tempRootPosition = animator.rootPosition;
	}
}
