using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIAfterRaidStatsValuesSetup : MonoBehaviour {

	public Text enemiesKilled;
	public Text walkDistance;
	public Text earnedEXP;

	void Start() {
		LastRaidResult result = GameLogic.GetLastRaidResult ();
		enemiesKilled.text = result.killed.ToString();
		walkDistance.text = result.walkDistance.ToString ();
		earnedEXP.text = result.earnedEXP.ToString ();
	}

}
