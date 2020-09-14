using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapButton : IUsableObject {

	public MapTriggerCaller generalCaller;

	public string showTextActive = "Use";
	public string showTextInactive = "Cant be used";

	public string activeSound = "";
	public string inactiveSound = "";

	public bool oneTimeUsable = false;

	string ConvertInputKey(string input) {
		return System.Enum.GetName (typeof(KeyCode), InputManager.instance.keys [input]);
	}

	public override string ShowText () {
		if (generalCaller.active) {
			return "[" + ConvertInputKey ("Use") + "] " + showTextActive;
		}
		return showTextInactive;
	}

	public override void Use () {
		if (generalCaller.active) {
			generalCaller.Call ();
			if (activeSound.Length > 1) {
				SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData (activeSound);
				data.spatialBlend = 0.0f;
				SoundManager.instance.CreateSound (data);
			}
			if (oneTimeUsable) {
				enabled = false;
			}
		} else {
			if (inactiveSound.Length > 1) {
				SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData (inactiveSound);
				data.spatialBlend = 0.0f;
				SoundManager.instance.CreateSound (data);
			}
		}
	}

}
