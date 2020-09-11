Shader "Custom/CharacterAdvanced" {
	Properties {
		_DiffuseMap ("Diffuse map (UV0)", 2D) = "white" {}
		_SpecularMap ("Specular map (UV0)", 2D) = "white" {}
		_DisplacementMap ("Displacement map (UV0)", 2D) = "white" {}
		_DetailMap ("Detail map (UV0)", 2D) = "white" {}
		[Normal] _DetailNormalMap ("Detail normal map (UV1)", 2D) = "white" {}
		_DetailAmbientOcclusionMap ("Detail Ambient Occlusion map (UV1)", 2D) = "white" {}
		_Smoothness ("Smoothness", Range(0, 3)) = 0.5
		_DisplacementPower ("Displacement power", Range(0, 0.02)) = 0.015
		_DetailScale ("Detail map scale", float) = 1.0
		_NormalPower ("Detail normal map power", float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM

		#pragma surface surf StandardSpecular fullforwardshadows

		#pragma target 3.0

		struct Input {
			float2 uv_DiffuseMap;
			float2 uv2_DetailAmbientOcclusionMap;
			float3 viewDir;
		};

		#pragma shader_feature __ _NORMAL
        #pragma shader_feature __ _SPECULAR
        #pragma shader_feature __ _DISPLACEMENT

		sampler2D _DiffuseMap;
		sampler2D _SpecularMap;
		sampler2D _DisplacementMap;
		sampler2D _DetailMap;
		sampler2D _DetailNormalMap;
		sampler2D _DetailAmbientOcclusionMap;
		float _Smoothness;
		float _DisplacementPower;
		float _DetailScale;
		float _NormalPower;

		void surf (Input IN, inout SurfaceOutputStandardSpecular o) {
			#ifdef _DISPLACEMENT
				float4 displacementColor = tex2D(_DisplacementMap, IN.uv_DiffuseMap);
				IN.uv_DiffuseMap += ParallaxOffset(displacementColor, _DisplacementPower, IN.viewDir);
				IN.uv2_DetailAmbientOcclusionMap += ParallaxOffset(displacementColor, _DisplacementPower, IN.viewDir);
			#endif

			float4 diffuseColor = tex2D(_DiffuseMap, IN.uv_DiffuseMap);
			float4 detailAmbientOcclusionColor = tex2D(_DetailAmbientOcclusionMap, IN.uv2_DetailAmbientOcclusionMap);

			float detailMapColor = tex2D(_DetailMap, IN.uv_DiffuseMap * _DetailScale).r;
			detailMapColor *= 2;

			o.Albedo = diffuseColor.rgb * detailAmbientOcclusionColor.r;
			o.Albedo *= detailMapColor;
			 
			#ifdef _NORMAL 
				float4 detailNormalColor = tex2D(_DetailNormalMap, IN.uv2_DetailAmbientOcclusionMap);
				//detailNormalColor.z /= _NormalPower;
				//detailNormalColor = normalize(detailNormalColor);
				//normalColor += detailNormalColor - 0.5; 
				float3 detailNormal = UnpackNormal(detailNormalColor);
				o.Normal = detailNormal;
				//o.Normal.z /= _NormalPower;
				//o.Normal = normalize(o.Normal);
			#endif

			#ifdef _SPECULAR
				float4 specularColor = tex2D(_SpecularMap, IN.uv_DiffuseMap);
				o.Specular = specularColor.rgb;
				o.Smoothness = specularColor.a * _Smoothness;
			#endif
		}
		ENDCG
	}
	FallBack "Diffuse"
}
