Shader "Custom/LightSpot" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex("Dummy", 2D) = "white"
	}
	SubShader {
		Tags { "RenderType"="Fade" }

		GrabPass {} 

		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		#include "UnityCG.cginc"

		struct Input {
			float4 screenPos;
			float2 uv_MainTex;
		};

		fixed4 _Color;

		sampler2D _CameraDepthTexture;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = Linear01Depth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos)));
			//o.Emission = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, IN.uv_MainTex));
			//o.Alpha = 1.0 - Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, IN.uv_MainTex));
			o.Alpha = 1.0;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
