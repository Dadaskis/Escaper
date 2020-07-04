using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUISaveGame : MonoBehaviour {

	public Text textUI;

	public void SaveGame() {
		string saveName = Serializer.instance.saves.Count.ToString ();
		Serializer.instance.Save (saveName);
		textUI.text = "Game saved.\n\n Save name: " + saveName;
	}

}
