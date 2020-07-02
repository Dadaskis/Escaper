using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostProcessingCaller : MonoBehaviour {

	public Material material;
	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		Graphics.Blit(source, destination, material);
	}

	void Start(){
		Camera cam = GetComponent<Camera>();
		cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.Depth;
	}
}
