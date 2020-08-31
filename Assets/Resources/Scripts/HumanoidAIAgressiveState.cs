using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HumanoidAICurrentTargetStatus {
	NONE,
	ALIVE_ENEMY,
	INVISIBLE,
	NOT_IN_FOV
}

public class HumanoidAIAgressiveState : StateMachineBehaviour {

	HumanoidCharacter character;
	Character currentTarget = null;
	HumanoidAICurrentTargetStatus currentTargetStatus = HumanoidAICurrentTargetStatus.NONE;
	float noActionTimer = 0.0f;
	HumanoidCharacterVisionData visionData;
	int invisibleUpdateCounter = 0;
	bool enemiesAreClose = false;
	bool rushIfInvisible = false;
	float shootTimer = 0.0f;
	float shootDelay = 0.2f;

	public float visionFOV = 90.0f;
	public float visionDistance = 20.0f;
	public float hearDistance = 10.0f;
	public float tooCloseEnemyDistance = 10.0f;
	public float jammedNoActionTime = 0.5f;
	public float reloadNoActionTime = 0.8f;
	public int rushIfInvisibleChance = 50;
	public float randomPlaceMinOffset = 4.0f;
	public float randomPlaceMaxOffset = 12.0f;
	public float rushMinOffset = 1.0f;
	public float rushMaxOffset = 2.0f;
	public float minShootDelay = 0.2f;
	public float maxShootDelay = 0.8f;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		character = animator.GetComponentInParent<HumanoidCharacter> ();
		if (Random.Range (0, 100) > rushIfInvisibleChance) {
			rushIfInvisible = true;
		}
	}

	bool CheckVision() {
		visionData = character.GetCharacterVision (visionFOV, visionDistance, hearDistance);

		if (currentTargetStatus == HumanoidAICurrentTargetStatus.ALIVE_ENEMY) {
			return false;
		}

		if (visionData.aliveEnemies.Count > 0) {
			float distance = 999999.9f;
			foreach (Character enemy in visionData.aliveEnemies) {
				float distanceCheck = Vector3.Distance (character.data.raycaster.position, enemy.raycaster.position);
				if (distanceCheck < distance) {
					currentTarget = enemy;
					distance = distanceCheck;
				}
			}
			currentTargetStatus = HumanoidAICurrentTargetStatus.ALIVE_ENEMY;
			return true;
		} else if (visionData.invisible.Count > 0) {
			currentTarget = visionData.invisible [0];
			currentTargetStatus = HumanoidAICurrentTargetStatus.INVISIBLE;
			return true;
		}

		return false;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		noActionTimer -= Time.deltaTime;

		CheckVision ();

		if (currentTarget == null) {
			currentTargetStatus = HumanoidAICurrentTargetStatus.NONE;
		}

		enemiesAreClose = false;
		foreach (Character enemy in CharacterManager.Characters) {
			if (enemy == null) {
				continue;
			}
			if (enemy.faction == character.data.faction || enemy.killed) {
				continue;
			}
			if (Vector3.Distance (enemy.transform.position, character.transform.position) < tooCloseEnemyDistance) {
				character.StopMoving ();
				character.DontControlRotation ();
				character.RotateToTheTarget (enemy.raycaster.position);
				character.shooter.UnSave ();
				enemiesAreClose = true;
			}
		}

		if (noActionTimer > 0) {
			NoAction ();
			return;
		}

		switch (currentTargetStatus) {
		case HumanoidAICurrentTargetStatus.ALIVE_ENEMY:
			ProcessEnemy ();
			break;
		case HumanoidAICurrentTargetStatus.INVISIBLE:
			ProcessInvisible ();
			break;
		}	
	}

	void NoAction() {
		character.ControlRotation ();
	}

	void NullifyTarget() {
		currentTarget = null;
		currentTargetStatus = HumanoidAICurrentTargetStatus.NONE;
	}

	void ProcessEnemy() {
		character.DontControlRotation ();
		character.StopMoving ();
		character.RotateToTheTarget (currentTarget.raycaster.position);
		shootTimer += Time.deltaTime;
		if (shootTimer > shootDelay) {
			character.shooter.UnSave ();
			shootTimer = 0.0f;
			shootDelay = Random.Range (minShootDelay, maxShootDelay);
			HumanoidCharacterShootStatus shootStatus = character.Shoot (currentTarget);
			switch (shootStatus) {
			case HumanoidCharacterShootStatus.JAMMED:
				noActionTimer = jammedNoActionTime;
				//character.Move(character.transform.position + MathHelper.RandomVector3(randomPlaceMinOffset, randomPlaceMaxOffset));
				character.DontControlRotation ();
				character.MoveToNearestEdge ();
				break;
			case HumanoidCharacterShootStatus.RELOAD_NEEDED:
				noActionTimer = reloadNoActionTime;
				//character.Move(character.transform.position + MathHelper.RandomVector3(randomPlaceMinOffset, randomPlaceMaxOffset));
				character.MoveToNearestEdge ();
				character.Reload ();
				break;
			case HumanoidCharacterShootStatus.HIT:
				break;
			}
		}
	}

	void ProcessInvisible() {
		if (enemiesAreClose) {
			return;
		}
		character.shooter.Save ();
		//if (rushIfInvisible) {
			character.ControlRotation ();
			character.Chase (currentTarget, rushMinOffset, rushMaxOffset);
		//} else {
			//character.MoveToNearestEdge ();
		//}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//character.Move (character.transform.position);
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

}
