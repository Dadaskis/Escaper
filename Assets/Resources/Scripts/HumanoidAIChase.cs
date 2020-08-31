using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidAIChase : StateMachineBehaviour {

	public string triggerName = "0";
	public float minOffset = 0.5f;
	public float maxOffset = 2.0f;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		HumanoidCharacter character = animator.GetComponentInParent<HumanoidCharacter> ();
		if (character != null) { 
			float X = Random.Range (minOffset, maxOffset); 
			float Y = Random.Range (minOffset, maxOffset); 
			float Z = Random.Range (minOffset, maxOffset); 
			Vector3 position = character.transform.position;
			if (character.target != null) {
				position = character.target.transform.position;
			}
			character.Move (position + new Vector3(X, Y, Z));
		}
		animator.SetTrigger (triggerName);
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
