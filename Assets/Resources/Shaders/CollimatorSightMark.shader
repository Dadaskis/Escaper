Shader "Custom/CollimatorSightMark" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		[HDR] _Emission ("Emission", Color) = (0, 0, 0, 0)
		_WhiteNoise ("White Noise", 2D) = "white" {}
		_Smoothness ("Smoothness", Range(0, 1)) = 0.5
		_Specular ("Specular", Color) = (0.5, 0.5, 0.5, 1.0)
		_WhiteNoiseAlphaMultiplier ("White Noise Alpha Multiplier", float) = 1.0
		_WhiteNoiseColorMultiplier ("White Noise Color Multiplier", float) = 1.0
	}
	SubShader { // +1
		Tags { "RenderType"="Opaque" "Queue" = "Transparent+3002" "ForceNoShadowCasting" = "True" }
		LOD 200
		ZTest always
		Stencil {
			Ref 32
			ReadMask 32
			Comp equal
			Pass keep
		}
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf StandardSpecular Lambert alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _WhiteNoise;

		struct Input {
			float2 uv_WhiteNoise;
		};

		fixed4 _Color;
		float4 _Emission;
		float _Smoothness;
		float4 _Specular;
		float _WhiteNoiseAlphaMultiplier, _WhiteNoiseColorMultiplier;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandardSpecular o) {
			// Albedo comes from a texture tinted by color
			fixed4 whiteNoise = tex2D (_WhiteNoise, IN.uv_WhiteNoise.xy + float2(_Time.w, _Time.z));
			fixed4 c = (whiteNoise * _WhiteNoiseColorMultiplier) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = _Color.a * (whiteNoise.r * _WhiteNoiseAlphaMultiplier);
			o.Emission = _Emission * (whiteNoise.r * _WhiteNoiseColorMultiplier);
			o.Smoothness = _Smoothness;
			o.Specular = _Specular;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
