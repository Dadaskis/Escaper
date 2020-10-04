using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
class VolumetricLightRenderer : MonoBehaviour
{
	public Shader CalculateFogShader = null;
	public Shader BlurShader = null;
	public Shader DownscaleDepthShader = null;
	public Shader ApplyFogShader = null;
	public Texture2D NoiseTexture = null;

	public float FogDensity = 0.2f;
	public float ScatteringCoeff = 0.25f;
	public float ExtinctionCoeff = 0.01f;
	public float MaxRayDistance = 300;
	public Color ShadowedFogColour = Color.white;
	public float Shadowed = 1;
	public float DepthThreshold = 0.01f;
	public float BlurDepthFalloff = 0.01f;
	public int depthQualityDivider = 2;

	private Material DownscaleDepthMaterial = null;
	private Material CalculateFogMaterial = null;
	private Material BlurMaterial = null;
	private Material ApplyFogMaterial = null;
	private Light light;

	public bool CheckResources ()
	{
		//CheckSupport (true);

		//DownscaleDepthMaterial = CheckShaderAndCreateMaterial(DownscaleDepthShader,DownscaleDepthMaterial);
		//CalculateFogMaterial = CheckShaderAndCreateMaterial (CalculateFogShader, CalculateFogMaterial);
		//BlurMaterial = CheckShaderAndCreateMaterial (BlurShader, BlurMaterial);
		//ApplyFogMaterial = CheckShaderAndCreateMaterial (ApplyFogShader, ApplyFogMaterial);

		if (DownscaleDepthMaterial == null) {
			DownscaleDepthMaterial = new Material (DownscaleDepthShader);
		}
		if (CalculateFogMaterial == null) {
			CalculateFogMaterial = new Material (CalculateFogShader);
		}
		if (BlurMaterial == null) {
			BlurMaterial = new Material (BlurShader);
		}
		if (ApplyFogMaterial == null) {
			ApplyFogMaterial = new Material (ApplyFogShader);
		}
		if (light == null) {
			Light[] lights = FindObjectsOfType<Light> ();
			foreach (Light light in lights) {
				if (light.type == LightType.Directional && light.isBaked == false) { 
					light.gameObject.AddComponent<VolumetricLight> ();
					this.light = light;
					break;
				}
			}
		}

		//if (!isSupported)
			//ReportAutoDisable ();
		if (light == null) {
			return false;
		}
		return true;
	}
	
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		if (CheckResources()==false)
		{
			Graphics.Blit (source, destination);
			return;
		}

		RenderTextureFormat formatRF32 = RenderTextureFormat.RFloat;
		int lowresDepthWidth= source.width/depthQualityDivider;
		int lowresDepthHeight= source.height/depthQualityDivider;

		RenderTexture lowresDepthRT = RenderTexture.GetTemporary (lowresDepthWidth, lowresDepthHeight, 0, formatRF32);

		//downscale depth buffer to quarter resolution
		Graphics.Blit (source, lowresDepthRT, DownscaleDepthMaterial);

		lowresDepthRT.filterMode = FilterMode.Point;

		RenderTextureFormat format = RenderTextureFormat.ARGBHalf;
		int fogRTWidth= source.width/2;
		int fogRTHeight= source.height/2;

		RenderTexture fogRT1 = RenderTexture.GetTemporary (fogRTWidth, fogRTHeight, 0, format);
		RenderTexture fogRT2 = RenderTexture.GetTemporary (fogRTWidth, fogRTHeight, 0, format);

		fogRT1.filterMode = FilterMode.Bilinear;
		fogRT2.filterMode = FilterMode.Bilinear;

		Light light = FindObjectOfType<VolumetricLight> ().GetComponent<Light> ();
		

		Camera camera = GetComponent<Camera>();

		Matrix4x4 worldViewProjection = camera.worldToCameraMatrix * camera.projectionMatrix;
		Matrix4x4 invWorldViewProjection = worldViewProjection.inverse;

		NoiseTexture.wrapMode = TextureWrapMode.Repeat;
		NoiseTexture.filterMode = FilterMode.Bilinear;

		CalculateFogMaterial.SetTexture ("LowResDepth", lowresDepthRT);
		CalculateFogMaterial.SetTexture ("NoiseTexture", NoiseTexture);

		CalculateFogMaterial.SetMatrix( "InverseViewMatrix", camera.cameraToWorldMatrix);
		CalculateFogMaterial.SetMatrix( "InverseProjectionMatrix", camera.projectionMatrix.inverse);
		CalculateFogMaterial.SetFloat ("FogDensity", FogDensity);
		CalculateFogMaterial.SetFloat ("ScatteringCoeff", ScatteringCoeff);
		CalculateFogMaterial.SetFloat ("ExtinctionCoeff", ExtinctionCoeff);
		CalculateFogMaterial.SetFloat ("MaxRayDistance", MaxRayDistance);
		CalculateFogMaterial.SetVector ("LightColour", light.color.linear);
		CalculateFogMaterial.SetFloat ("LightIntensity", light.intensity);
		CalculateFogMaterial.SetColor ("ShadowedFogColour", ShadowedFogColour);
		CalculateFogMaterial.SetVector ("TerrainSize", new Vector3(100,50,100));

		//render fog, quarter resolution
		Graphics.Blit (source, fogRT1, CalculateFogMaterial);

		//blur fog, quarter resolution
		BlurMaterial.SetFloat ("BlurDepthFalloff", BlurDepthFalloff);
		BlurMaterial.SetTexture ("LowresDepthSampler", lowresDepthRT);

		BlurMaterial.SetVector ("BlurDir", new Vector2(0,1));
		Graphics.Blit (fogRT1, fogRT2, BlurMaterial);

		//blur fog, quarter resolution
		BlurMaterial.SetVector ("BlurDir", new Vector2(1,0));
		Graphics.Blit (fogRT2, fogRT1, BlurMaterial);

		//blur fog, quarter resolution
		BlurMaterial.SetVector ("BlurDir", new Vector2(0,1));
		Graphics.Blit (fogRT1, fogRT2, BlurMaterial);
		
		//blur fog, quarter resolution
		BlurMaterial.SetVector ("BlurDir", new Vector2(1,0));
		Graphics.Blit (fogRT2, fogRT1, BlurMaterial);
			
		//apply fog to main scene
		fogRT1.filterMode = FilterMode.Bilinear;
		ApplyFogMaterial.SetTexture ("FogRendertargetPoint", fogRT1);

		fogRT2.filterMode = FilterMode.Bilinear;
		ApplyFogMaterial.SetTexture ("FogRendertargetLinear", fogRT1);
		ApplyFogMaterial.SetTexture ("LowResDepthTexture", lowresDepthRT);
		ApplyFogMaterial.SetFloat ("DepthThreshold", DepthThreshold);

		//upscale fog and apply to main rendertarget
		Graphics.Blit (source, destination, ApplyFogMaterial);

		RenderTexture.ReleaseTemporary(lowresDepthRT);
		RenderTexture.ReleaseTemporary(fogRT1);
		RenderTexture.ReleaseTemporary(fogRT2);

	}
}
