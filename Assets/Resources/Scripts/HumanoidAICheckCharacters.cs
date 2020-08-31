using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidAICheckCharacters : StateMachineBehaviour {

	public float FOV = 90.0f;
	public float visionDistance = 60.0f;
	public string aliveEnemyTriggerName = "0";
	public string deadEnemyTriggerName = "1";
	public string aliveFriendTriggerName = "2";
	public string deadFriendTriggerName = "3";
	public string nothingTriggerName = "4";

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		HumanoidCharacter character = animator.GetComponentInParent<HumanoidCharacter> ();

		if(character.target != null) {
			Character target = character.target.GetComponent<Character> ();
			if (!target.killed) {
				if (target.faction != character.data.faction) {
					RaycastHit hit;
					Vector3 start = character.data.raycaster.position;
					Vector3 end = target.transform.position;
					bool hitSomething = Physics.Linecast (start, end, out hit);
					if (hitSomething && target.transform.root == hit.transform.root) {
						animator.SetTrigger (aliveEnemyTriggerName);
						return;
					}
				}
			}
		}

		List<Character> aliveEnemies = new List<Character> ();
		List<Character> aliveFriends = new List<Character> ();
		List<Character> deadEnemies = new List<Character> ();
		List<Character> deadFriends = new List<Character> ();

		foreach (Character anotherCharacter in CharacterManager.Characters) {
			if (anotherCharacter == null) {
				continue;
			}

			if (Vector3.Distance (character.transform.position, anotherCharacter.transform.position) > visionDistance) {
				continue;
			}

			if (Vector3.Angle (anotherCharacter.raycaster.position - character.data.raycaster.position, character.transform.forward) <= FOV) {
				RaycastHit hit;
				Vector3 start = character.data.raycaster.position;
				Vector3 end = anotherCharacter.transform.position;
				bool hitSomething = Physics.Linecast (start, end, out hit);
				if (hitSomething && anotherCharacter.raycaster.root == hit.transform.root) {
					bool friend = anotherCharacter.faction == character.data.faction;
					if (anotherCharacter.killed) {
						if (friend) {
							deadFriends.Add(anotherCharacter);
						} else {
							deadEnemies.Add(anotherCharacter);
						}
					} else {
						if (friend) {
							aliveFriends.Add(anotherCharacter);
						} else {
							aliveEnemies.Add(anotherCharacter);
						}
					}
				}
			}
		}

		if (aliveEnemies.Count > 0) {
			character.target = aliveEnemies [0];
			animator.SetTrigger (aliveEnemyTriggerName);
		} else if (deadEnemies.Count > 0) {
			character.target = deadEnemies [0];
			animator.SetTrigger (deadEnemyTriggerName);
		} else if (deadFriends.Count > 0) {
			character.target = deadFriends [0];
			animator.SetTrigger (deadFriendTriggerName);
		} else if (aliveFriends.Count > 0) {
			character.target = aliveFriends [0];
			animator.SetTrigger (aliveFriendTriggerName);
		} else { 
			character.target = null;
			animator.SetTrigger (nothingTriggerName);
		}
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
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
