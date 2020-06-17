using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InSightDisabler : MonoBehaviour {
	public Image image;

	void FixedUpdate () {
		image.enabled = !Input.GetMouseButton (1);
	}
}
