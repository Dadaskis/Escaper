using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIAmmoText : MonoBehaviour {

	public Text text;
	public float seconds = 1.0f;

	public static GUIAmmoText instance;

	IEnumerator DisableText() {
		yield return new WaitForSeconds (seconds);
		text.gameObject.SetActive (false);
	}

	public void ShowText(IWeapon weapon) {
		text.text = weapon.currentAmmo + " / " + weapon.maxAmmo;
		text.gameObject.SetActive (true);
		StartCoroutine (DisableText());
	}

	void Awake() {
		instance = this;
	}

}
