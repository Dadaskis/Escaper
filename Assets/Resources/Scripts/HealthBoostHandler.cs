using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoostHandler : IPlayerBoost {

	public int healthOnPoint = 100;

	public override void Apply (PlayerBoostData data) {
		Character playerCharacter = Player.instance.character;
		int health = playerCharacter.Health;
		health += healthOnPoint * data.points;
		playerCharacter.Health = health;
	}

}
