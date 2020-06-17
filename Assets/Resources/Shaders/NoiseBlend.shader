Shader "Custom/NoiseBlend" {
	Properties {
		//_Color ("Color", Color) = (1,1,1,1)
		_Mask ("Mask", 2D) = "white" {}
		_MaskPower("Mask power", Float) = 0.5
		_Offset("Offset", Vector) = (0.0, 0.0, 0.0)
		_Scale ("Scale", Float) = 1
		_Power ("Power", Float) = 1
		_UpCheckValue ("Up check value", Float) = 1
		_UpPower ("Up power", Float) = 1

		_ColorMultiplier1 ("Color multiplier 1", Color) = (1, 1, 1, 1)
		_Color1 ("Color 1", 2D) = "white" {}
		_Normal1 ("Normal 1", 2D) = "white" {}
		_Specular1 ("Specular 1", 2D) = "black" {}
		_Displacement1 ("Displacement 1", 2D) = "white" {}

		_ColorMultiplier2 ("Color multiplier 2", Color) = (1, 1, 1, 1)
		_Color2 ("Color 2", 2D) = "white" {}
		_Normal2 ("Normal 2", 2D) = "white" {}
		_Specular2 ("Specular 2", 2D) = "black" {}
		_Displacement2 ("Displacement 2", 2D) = "white" {}

		_DisplacementPower1 ("Displacement power 1", Float) = 0.2
		_DisplacementPower2 ("Displacement power 2", Float) = 0.2

		_Glossiness1 ("Smoothness 1", Float) = 0.5
		_Glossiness2 ("Smoothness 2", Float) = 0.5

		_NormalPower1 ("Normal power 1", Float) = 1.0
		_NormalPower2 ("Normal power 2", Float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf StandardSpecular fullforwardshadows

		#pragma shader_feature __ _NORMALMAP
        #pragma shader_feature __ _SPECULARHIGHLIGHTS_OFF
        #pragma shader_feature __ _PARALLAXMAP

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input {
			float2 uv_Color1, uv_Color2, uv_Normal1, uv_Normal2, uv_Specular1, uv_Specular2, uv_Displacement1, uv_Displacement2;
			float3 worldPos;
			float3 viewDir;
			float3 worldNormal;
			INTERNAL_DATA
		};

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

        float hash( float n )
		{
		    return frac(sin(n)*43758.5453);
		}

		float noise( float3 x )
		{
		    // The noise function returns a value in the range -1.0f -> 1.0f

		    float3 p = floor(x);
		    float3 f = frac(x);

		    f       = f*f*(3.0-2.0*f);
		    float n = p.x + p.y*57.0 + 113.0*p.z;

		    return lerp(lerp(lerp( hash(n+0.0), hash(n+1.0),f.x),
		                   lerp( hash(n+57.0), hash(n+58.0),f.x),f.y),
		               lerp(lerp( hash(n+113.0), hash(n+114.0),f.x),
		                   lerp( hash(n+170.0), hash(n+171.0),f.x),f.y),f.z);
		}

		float _Scale, _Power, _MaskPower, _UpPower, _UpCheckValue;
		float3 _Offset;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		sampler2D _Mask, _Color1, _Color2, _Normal1, _Normal2, _Specular1, _Specular2, _Displacement1, _Displacement2;
		float _DisplacementPower1, _DisplacementPower2, _Glossiness1, _Glossiness2, _NormalPower1, _NormalPower2;
		fixed4 _ColorMultiplier1, _ColorMultiplier2;

		void surf (Input IN, inout SurfaceOutputStandardSpecular o) {
			float noiseValue = noise((IN.worldPos + _Offset) * _Scale) * _Power;
			float mask = tex2D(_Mask, IN.uv_Color1).r * _MaskPower;
			noiseValue *= mask; 

			float normalValue = 1.0;
			#ifdef _NORMALMAP
			normalValue = WorldNormalVector(IN, float3(0.0, 0.0, 1.0)).y;
			#else
			normalValue = IN.worldNormal.y;
			#endif

			noiseValue *= (_UpCheckValue <= normalValue) ? _UpPower : 1.0;
			noiseValue = smoothstep(0.0, 1.0, noiseValue);

			#ifdef _PARALLAX_MAP
			half displacement1 = tex2D(_Displacement1, IN.uv_Displacement1).r * _DisplacementPower1;
			half displacement2 = tex2D(_Displacement2, IN.uv_Displacement2).r * _DisplacementPower2;
			half displacement = lerp(displacement1, displacement2, noiseValue);

			IN.uv_MainTex += ParallaxOffset(displacement, _DisplacementPower, IN.viewDir);
			#endif

			fixed3 firstColor = tex2D(_Color1, IN.uv_Color1) * _ColorMultiplier1;
			fixed3 secondColor = tex2D(_Color2, IN.uv_Color2) * _ColorMultiplier2;
			o.Albedo = lerp(firstColor, secondColor, noiseValue);

			#ifdef _NORMALMAP
			fixed4 normal1 = tex2D(_Normal1, IN.uv_Normal1) * _NormalPower1;
			fixed4 normal2 = tex2D(_Normal2, IN.uv_Normal2) * _NormalPower2;
			fixed4 normalMap = lerp(normal1, normal2, noiseValue);
			o.Normal = UnpackNormal(normalMap);
			#endif
			 
			#ifdef _SPECULARHIGHLIGHTS_OFF
			firstColor = tex2D(_Specular1, IN.uv_Specular1) * _Glossiness1;
			secondColor = tex2D(_Specular2, IN.uv_Specular2) * _Glossiness2;
			fixed3 specularResult = lerp(firstColor, secondColor, noiseValue);
			o.Specular = fixed3(specularResult.x, specularResult.y, specularResult.z);
			#endif

			o.Smoothness = _Glossiness; 
		}
		ENDCG
	}
	FallBack "Diffuse"
}
