using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidAIPatrollingState : StateMachineBehaviour {

	HumanoidCharacter character;
	float visionTimer = 0.0f;
	float visionDelay = 1.0f;
	float awaitingTimer = 0.0f;
	float awaitingDelay = 1.0f;

	public float visionFOV = 20.0f;
	public float visionDistance = 20.0f;
	public float hearDistance = 10.0f;
	public float minVisionDelay = 0.5f;
	public float maxVisionDelay = 2.0f;
	public float minAwaitingDelay = 20.0f;
	public float maxAwaitingDelay = 40.0f;

	EventData OnDamage(EventData data) {
		character.MoveToNearestCover ();
		character.ChangeState ("Agressive");

		return new EventData ();
	}

	bool CheckVision() {
		HumanoidCharacterVisionData visionData = character.GetCharacterVision (visionFOV, visionDistance, hearDistance);

		//if (currentTargetStatus == HumanoidAICurrentTargetStatus.ALIVE_ENEMY) {
		//	return false;
		//}

		if (visionData.aliveEnemies.Count > 0) {
			character.ChangeState ("Agressive");
			return true;
		} 

		return false;
	}

	EventData OnWeaponShot(EventData data) {

		IWeapon weapon = data.Get<IWeapon> (0);
		if (weapon != null) {
			Character owner = weapon.owner;
			if (owner != null) {
				if (owner.faction != character.data.faction) {
					character.DontControlRotation ();
					character.RotateToTheTarget (owner.raycaster.position);
				}
			}
		}

		return new EventData ();
	}

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		character = animator.GetComponentInParent<HumanoidCharacter> ();
		character.data.onDamage.AddListener (OnDamage);
		EventManager.AddEventListener<Events.IWeapon.Shot> (OnWeaponShot);
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		visionTimer += Time.deltaTime;

		if (visionTimer > visionDelay) {			
			CheckVision ();
			visionDelay = Random.Range (minVisionDelay, maxVisionDelay);
			visionTimer = 0.0f;
		}

		if (character.agent.remainingDistance < 2.0f || !character.agent.hasPath) {
			awaitingTimer += Time.deltaTime;
			if (awaitingTimer > awaitingDelay) {
				awaitingDelay = Random.Range (minAwaitingDelay, maxAwaitingDelay);
				awaitingTimer = 0.0f;
				character.Move (PatrolPointsManager.GetPoint (character.transform.position));
			}
		}

	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		character.data.onDamage.RemoveListener (OnDamage);
	}

}
