using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnotherLiveBoostHandler : IPlayerBoost {

	public int lives = 0;

	public override void Apply (PlayerBoostData data) {
		lives += data.points;
	}

	EventData OnPlayerDeath(EventData args) {
		if (lives > 0) {
			lives--;
			Player.instance.character.Health = Player.instance.character.MaxHealth;

			AppearPoint[] points = FindObjectsOfType<AppearPoint> ();
			AppearPoint point = points[Random.Range(0, points.Length - 1)];
			Player.instance.transform.position = point.transform.position + new Vector3(0.0f, 1.8f, 0.0f);

			Player.instance.killed = false;
			Player.instance.character.enabled = true;

			return new EventData (true);
		}
		return new EventData ();
	}

	void Start() {
		EventManager.AddEventListener<Events.GameLogic.PlayerDeath> (OnPlayerDeath);
	}

}
