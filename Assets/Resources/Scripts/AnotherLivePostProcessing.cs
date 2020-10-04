using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnotherLivePostProcessing : MonoBehaviour {

	public PostProcessingCaller caller;
	public float speed = 0.5f;
	public static AnotherLivePostProcessing instance;

	private bool started = false;
	private float currentContribution = 0.0f;
	private AppearPoint point;

	void Awake() {
		instance = this;
	}

	public void StartEffect() {
		started = true;
		Player.instance.controller.enableLogic = false;
		Player.instance.character.enabled = false;

		Player.instance.character.Health = Player.instance.character.MaxHealth;

		AppearPoint[] points = FindObjectsOfType<AppearPoint> ();
		point = points[Random.Range(0, points.Length - 1)];
		//

		Player.instance.killed = false;

		//
	}

	public void EndEffect() {
		started = false;
		Player.instance.transform.position = point.transform.position + new Vector3(0.0f, 1.8f, 0.0f);
		Player.instance.character.enabled = true;
		Player.instance.controller.enableLogic = true;
		Player.instance.character.killed = false;
		CharacterManager.Characters.Add (Player.instance.character);
	}

	void Update() {
		if (started) {
			currentContribution = Mathf.Lerp (currentContribution, 1.0f, Time.deltaTime * speed);
		} else {
			currentContribution = Mathf.Lerp (currentContribution, 0.0f, Time.deltaTime * speed);
		}
		if (currentContribution >= 0.99f) { 
			EndEffect ();
		}
		caller.enabled = currentContribution > 0.01f;
		caller.material.SetFloat ("_Contribution", currentContribution);
	}

}
