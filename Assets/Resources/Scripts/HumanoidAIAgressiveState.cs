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
	float rushInvisibleDelay = 1.0f;
	float rushInvisibleTimer = 0.0f;
	float visionTimer = 0.0f;
	float visionDelay = 1.0f;
	float assultingTimer = 0.0f;
	float assultingDelay = 0.0f;

	public float visionFOV = 90.0f;
	public float visionDistance = 20.0f;
	public float hearDistance = 10.0f;
	public float tooCloseEnemyDistance = 10.0f;
	public float jammedNoActionTime = 0.5f;
	public float reloadNoActionTime = 0.8f;
	public float rushIfInvisibleUpdateMinDelay = 30.0f;
	public float rushIfInvisibleUpdateMaxDelay = 30.0f;
	public float randomPlaceMinOffset = 4.0f;
	public float randomPlaceMaxOffset = 12.0f;
	public float rushMinOffset = 1.0f;
	public float rushMaxOffset = 2.0f;
	public float minShootDelay = 0.2f;
	public float maxShootDelay = 0.8f;
	public float minVisionDelay = 0.5f;
	public float maxVisionDelay = 2.0f;
	public float assultingMinDelay = 2.0f;
	public float assultingMaxDelay = 10.0f;
	public float assultingMinOffset = 5.0f;
	public float assultingMaxOffset = 10.0f;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		character = animator.GetComponentInParent<HumanoidCharacter> ();
		character.data.onDamage.AddListener (OnDamage);
		EventManager.AddEventListener<Events.IWeapon.Shot> (OnWeaponShot);
		character.MoveToNearestCover ();
	}

	EventData OnDamage(EventData data) {
		int count = data.Get<int> (0);
		Character damager = data.Get<Character> (1);
		currentTarget = damager;
		currentTargetStatus = HumanoidAICurrentTargetStatus.ALIVE_ENEMY;
		character.MoveToNearestCover ();

		return new EventData ();
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

	bool CheckVision() {
		visionData = character.GetCharacterVision (visionFOV, visionDistance, hearDistance);

		if (currentTarget != null) {
			if (currentTarget.killed) {
				NullifyTarget ();
			}
		}

		//if (currentTargetStatus == HumanoidAICurrentTargetStatus.ALIVE_ENEMY) {
		//	return false;
		//}

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
		visionTimer += Time.deltaTime;
		rushInvisibleTimer += Time.deltaTime;
		assultingTimer += Time.deltaTime;

		if (rushInvisibleTimer > rushInvisibleDelay) {
			rushInvisibleDelay = Random.Range (rushIfInvisibleUpdateMinDelay, rushIfInvisibleUpdateMaxDelay);
			rushIfInvisible = !rushIfInvisible;
			rushInvisibleTimer = 0.0f;
		}

		if (visionTimer > visionDelay) {			
			CheckVision ();
			visionDelay = Random.Range (minVisionDelay, maxVisionDelay);
			visionTimer = 0.0f;

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
		}

		if (currentTarget == null) {
			NullifyTarget ();
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
		//character.StopMoving ();
		character.RotateToTheTarget (currentTarget.raycaster.position);
		shootTimer += Time.deltaTime;

		if (assultingTimer > assultingDelay) {
			assultingTimer = 0.0f;
			assultingDelay = Random.Range (assultingMinDelay, assultingMaxDelay);
			character.Chase (currentTarget, assultingMinOffset, assultingMaxOffset);
		}

		if (shootTimer > shootDelay) {
			character.shooter.UnSave ();
			shootTimer = 0.0f;
			shootDelay = Random.Range (minShootDelay, maxShootDelay);
			HumanoidCharacterShootStatus shootStatus = character.Shoot (currentTarget);
			switch (shootStatus) {
			case HumanoidCharacterShootStatus.JAMMED:
				noActionTimer = jammedNoActionTime;
				//character.Move(character.transform.position + MathHelper.RandomVector3(randomPlaceMinOffset, randomPlaceMaxOffset));
				character.ControlRotation();
				character.MoveToNearestCover ();
				break;
			case HumanoidCharacterShootStatus.RELOAD_NEEDED:
				noActionTimer = reloadNoActionTime;
				//character.Move(character.transform.position + MathHelper.RandomVector3(randomPlaceMinOffset, randomPlaceMaxOffset));
				character.ControlRotation();
				character.MoveToNearestCover ();
				character.Reload ();
				break;
			case HumanoidCharacterShootStatus.HIT:
				character.MoveToNearestCover ();
				break;
			}
		}
	}

	void ProcessInvisible() {
		if (enemiesAreClose) {
			return;
		}
		character.shooter.Save ();
		if (rushIfInvisible) {
			character.ControlRotation ();
			character.Chase (currentTarget, rushMinOffset, rushMaxOffset);
		} else {
			character.MoveToNearestCover ();
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		character.data.onDamage.RemoveListener (OnDamage);
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

}
