using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClearStencilAfterLightPass : MonoBehaviour {

	public Camera camera;

	void Start () {
		camera.clearStencilAfterLightingPass = true;
	}
}
