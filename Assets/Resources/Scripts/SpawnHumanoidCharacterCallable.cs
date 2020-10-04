using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHumanoidCharacterCallable : IMapTriggerCallable {

	public GameObject prefab;
	public Transform movePoint;
	public string overrideState;

	IEnumerator MoveCharacter(HumanoidCharacter character) {
		yield return new WaitForEndOfFrame ();
		yield return new WaitForEndOfFrame ();
		character.Move (movePoint.position);
	}

	public override void Call () {
		GameObject characterObject = Instantiate (prefab);
		HumanoidCharacter character = characterObject.GetComponent<HumanoidCharacter> ();
		character.agent.Warp (transform.position);

		if (overrideState.Length > 1) {
			character.ChangeState (overrideState);
		}

		if (movePoint != null) {
			StartCoroutine (MoveCharacter (character));
		}
	}

}
