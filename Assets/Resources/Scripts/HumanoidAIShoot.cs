using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidAIShoot : StateMachineBehaviour {

	public string doneTriggerName = "0";
	public string outOfAmmoTriggerName = "1";
	public string jammedTriggerName = "2";

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		HumanoidCharacter character = animator.GetComponentInParent<HumanoidCharacter> ();
		character.shooter.Shoot (character.target.gameObject, character.target.raycaster.position);

		if (character.shooter.weapon.currentAmmo <= 0) {
			animator.SetTrigger (outOfAmmoTriggerName);
			return;
		} else if (character.shooter.IsJammed ()) {
			animator.SetTrigger (jammedTriggerName);		
			return;
		}

		animator.SetTrigger (doneTriggerName);
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
