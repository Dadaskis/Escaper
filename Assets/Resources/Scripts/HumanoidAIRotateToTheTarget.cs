using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidAIRotateToTheTarget : StateMachineBehaviour {

	HumanoidCharacter character;
	public bool triggerWhenCantSee = false;
	public bool dontRotateWhenCantSee = true;
	public string cantSeeTriggerName = "0";
	public string doneRotatingTriggerName = "1";

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		character = animator.GetComponentInParent<HumanoidCharacter> ();

		if (character.target == null) {
			animator.SetTrigger (cantSeeTriggerName);
			return;
		}

		bool cantSee = false;
		RaycastHit hit;
		if (Physics.Linecast (character.data.raycaster.position, character.target.transform.position, out hit)) {
			if (hit.transform.root != character.target.transform.root) {
				cantSee = true;
			}
		}

		if (triggerWhenCantSee && cantSee) {
			animator.SetTrigger (cantSeeTriggerName);
		} else if (!cantSee || !dontRotateWhenCantSee) {
			//Quaternion rotation = character.transform.rotation;
			//Quaternion newRotation = Quaternion.FromToRotation (character.target.transform.position, character.transform.position);
			//character.transform.rotation = new Quaternion (rotation.x, newRotation.y, rotation.z, rotation.w);
			Vector3 direction = character.target.transform.position - character.transform.position;
			direction.y = 0.0f;
			direction = direction.normalized;
			character.transform.rotation = Quaternion.LookRotation (direction);
			animator.SetTrigger (doneRotatingTriggerName);
		} 
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
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
