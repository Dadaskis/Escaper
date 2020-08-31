using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidAIGoToRandomPlace : StateMachineBehaviour {
	public float radius = 1.0f;
	public string triggerName = "0";

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		HumanoidCharacter character = animator.GetComponentInParent<HumanoidCharacter> ();
		if (character != null) { 
			if (character.agent.remainingDistance <= character.agent.stoppingDistance + 0.05f) {
				float X = Random.Range (-radius, radius);
				float Y = Random.Range (-radius, radius);
				float Z = Random.Range (-radius, radius);
				character.Move (character.transform.position + new Vector3 (X, Y, Z));
			}
		}
		animator.SetTrigger (triggerName);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//	if (character.agent.remainingDistance <= character.agent.stoppingDistance + 0.05f) {
	//		animator.SetTrigger (triggerName);
	//	}
	//}

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
