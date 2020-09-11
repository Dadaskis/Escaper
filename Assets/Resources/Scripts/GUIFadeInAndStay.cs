using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIFadeInAndStay : MonoBehaviour {

	public Image image;
	public float seconds = 3.0f;
	private float timer = 0.0f;

	void Update () {
		timer += Time.deltaTime;
		float alpha = image.color.a;
		alpha = Mathf.Lerp (alpha, 1.0f, ((timer / seconds) * Time.deltaTime) * 3.0f);
		alpha = Mathf.Clamp01 (alpha);
		if (alpha > 0.95f) {
			alpha = 1.0f;
		}
		image.color = new Color (image.color.r, image.color.g, image.color.b, alpha);
	}
}
