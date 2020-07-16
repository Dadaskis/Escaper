using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransformIdleAnimationState { 
	STAY,
	WALK,
	RUN
}

public class TransformIdleAnimationGenerator : MonoBehaviour {

	public TransformIdleAnimationState state;

	public float stayXSpeed = 0.1f;
	public float stayXScale = 0.01f;
	public float stayYSpeed = 0.1f;
	public float stayYScale = 0.01f;
	public float walkXSpeed = 0.1f;
	public float walkXScale = 0.01f;
	public float walkYSpeed = 0.1f;
	public float walkYScale = 0.01f;
	public float runXSpeed = 0.1f;
	public float runXScale = 0.01f;
	public float runYSpeed = 0.1f;
	public float runYScale = 0.01f;
	public float positionPower = 5.0f;

	private Transform myTransform;

	void Start() {
		myTransform = transform;
	}

	void ProcessAnimation() {
		Vector3 position = transform.localPosition;
		float speed = 0.0f;
		float scale = 0.0f;

		if (state == TransformIdleAnimationState.STAY) {
			position.x = Mathf.Cos (Time.time * stayXSpeed) * stayXScale;
			position.y = Mathf.Sin (Time.time * stayYSpeed) * stayYScale;
		} else if (state == TransformIdleAnimationState.WALK) {
			position.x = Mathf.Cos (Time.time * walkXSpeed) * walkXScale;
			position.y = Mathf.Sin (Time.time * walkYSpeed) * walkYScale;
		} else if (state == TransformIdleAnimationState.RUN) {
			position.x = Mathf.Cos (Time.time * runXSpeed) * runXScale;
			position.y = Mathf.Sin (Time.time * runYSpeed) * runYScale;
		} 
		position.z = 0.0f;
		transform.localPosition = Vector3.Slerp(transform.localPosition, position, Time.deltaTime * positionPower);
	}

	void Update() {
		if (InputManager.GetButton ("PlayerForward") || InputManager.GetButton ("PlayerBackward")) {
			state = TransformIdleAnimationState.WALK;
		} else if (InputManager.GetButton ("PlayerLeft") || InputManager.GetButton ("PlayerRight")) {
			state = TransformIdleAnimationState.WALK;
		} else {
			state = TransformIdleAnimationState.STAY;
		}

		if(InputManager.GetButton("PlayerRun") && state == TransformIdleAnimationState.WALK) {
			state = TransformIdleAnimationState.RUN;
		}

		ProcessAnimation ();
	}

}
