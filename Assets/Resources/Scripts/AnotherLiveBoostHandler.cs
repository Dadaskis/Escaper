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

			AnotherLivePostProcessing.instance.StartEffect ();

			return new EventData (true);
		}
		return new EventData ();
	}

	void Start() {
		EventManager.AddEventListener<Events.GameLogic.PlayerDeath> (OnPlayerDeath);
	}

}
