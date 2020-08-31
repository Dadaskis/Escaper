using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSpeedBoostHandler : IPlayerBoost {

	public float runModifier = 1.30f;

	public override void Apply (PlayerBoostData data) {
		for (int counter = 0; counter < data.points; counter++) {
			Player.instance.controller.runSpeed *= runModifier;
		}
	}

}
