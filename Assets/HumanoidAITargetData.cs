using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidAITargetData : StateMachineBehaviour {

	public string aliveEnemyTriggerName = "0";
	public string aliveFriendTriggerName = "1";
	public string deadEnemyTriggerName = "2";
	public string deadFriendTriggerName = "3";
	public string nothingTriggerName = "4";

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		HumanoidCharacter character = animator.GetComponentInParent<HumanoidCharacter> ();
		if (character != null) {
			if (character.target != null) {
				Character target = character.target.GetComponent<Character> ();
				bool enemy = target.faction != character.data.faction;
				if (!target.killed) {
					if (enemy) {
						animator.SetTrigger (aliveEnemyTriggerName);
					} else {
						animator.SetTrigger (aliveFriendTriggerName);
					}
				} else {
					if (enemy) {
						animator.SetTrigger (deadEnemyTriggerName);
					} else {
						animator.SetTrigger (deadFriendTriggerName);
					}
				}
			}
		}
		animator.SetTrigger (nothingTriggerName);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
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
