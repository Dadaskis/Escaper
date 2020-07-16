Shader "Custom/CollimatorSightGlass" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_DisplacementRefraction ("Displacement refraction", 2D) = "white" {}
		_RefractionPower ("Refraction power", float) = 1.0
		_RefractionMultiply ("Refraction multiply", float) = 1.0
		_Specular ("Specular", Color) = (0.5, 0.5, 0.5, 1.0)
	}
	SubShader { // -1
		Tags { "RenderType"="Transparent" "Queue" = "Transparent+3001" "ForceNoShadowCasting" = "True" }

		GrabPass {} 

		//ColorMask 0

		//ZWrite off

		Stencil {
			Ref 32
			WriteMask 32
			Comp always
			Pass replace
		}
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf StandardSpecular fullforwardshadows alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float2 uv_DisplacementRefraction;
			float4 screenPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		sampler2D _DisplacementRefraction;
		float _RefractionPower;
		float _RefractionMultiply;
		sampler2D _GrabTexture;
		float4 _Specular;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandardSpecular o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 displacement = tex2D(_DisplacementRefraction, IN.uv_DisplacementRefraction);
			o.Albedo = (tex2Dproj(_GrabTexture, IN.screenPos + (displacement * _RefractionPower)) * _RefractionMultiply) * _Color;
			o.Smoothness = _Glossiness;
			o.Alpha = _Color.a;
			o.Specular = _Specular;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
