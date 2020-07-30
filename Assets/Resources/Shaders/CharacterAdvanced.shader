Shader "Custom/CharacterAdvanced" {
	// Secret property of Dudka inc.
	// Dont copy or we will find you.
	// But stop, i'm the only dude working in this nonofficial team...
	// ITS NOT MATTER AT ALL
	Properties {
		// DiffuseMap is just a bunch of pixels for colors, nothing hard
		_DiffuseMap ("Diffuse map (UV0)", 2D) = "white" {}
		// NormalMap is just a bunch of pixels which describe which direction pixel has. Sounds trippy? Google what normals is.
		[Normal] _NormalMap ("Normal map (UV0)", 2D) = "white" {}
		// SpecularMap is just a bunch of pixels (from black to white) which tells renderer how pixel is reflective. What? Not physically correct? I dont give a shit.
		// Black means reflectivity is on minimum, what is impossible in real life though
		// White means that material is so reflective like its clear metal, its dangerous to have this value.
		// Why? Because renderer is based on rasterisation, not fucking raytracing.
		// You dont understand what am i saying? Ok, just saying that rasterisation cant reproduce reflections good
		// and in current game i have them only because of cubemaps. What is cubemaps? FUCKING GOOGLE IT!.. and blow your mind up.
		// Last thing i say about this thing is that "reflective" metals in game will have maximum ~128 value (remember limit of [0 .. 255]).
		//
		// Alpha channel of the texture (i mean "transparency") means how "smoothness" value is powerful. What is this value?
		// It means how hard we apply cubemap itself. You know, on minimum value of this and on maximum value of specularity it will be white-ish mess
		// and it will burn anyones eyes when "bloom" activated... its hard to describe, test it yourself, download/open Unity, get Post-Processing Stack v2,
		// set it up, create material with white specular color and 255 alpha value, enable bloom, and see.
		// 
		// "If you cant describe it normally to 3-year-old child than you dont understand it" ha-ha funny not in that case
		_SpecularMap ("Specular map (UV0)", 2D) = "white" {}
		// DisplacementMap is just a bunch of pixels which (from black to white) how parallax-mapping effect applied.
		// Parallax-mapping is... the thing that you will Google. Okay? I'm saying its only "extrudes" pixels.
		_DisplacementMap ("Displacement map (UV0)", 2D) = "white" {}
		// AmbientOcclusionMap is just a bunch of pixels which says how dark is the pixel. Maybe its better to apply it on DiffuseMap
		// in, for example, paint, not in shader.
		// _AmbientOcclusionMap ("Ambient Occlusion map (UV0)", 2D) = "white" {}
		// Okay on this point i dont say "A BUNCH OF PIXELS"
		// AmbientOcclusionMap as i understood now is not that needed thing. The method named "paint.exe preprocessing" is better.
		// Why? For example videomemory economy. I have 256 megabytes videomemory limit, you know...
		// and it sucks. But i dont care, 3D in games is about funny suffering.
		// DetailNormalMap is like NormalMap, but it applied by using second UV channel. Just magical stuff, yep.
		[Normal] _DetailNormalMap ("Detail normal map (UV1)", 2D) = "white" {}
		// DetailAmbientOcclusionMap is needed to darken some areas by using second UV channel. I baked it and i can "simulate"
		// clothes displacements on minimal settings without costing anything that much. Normal maps are expensive, you know. 
		// And it also empowers effect when used with normal maps, and i like it. So it will be part of the shader, and maybe
		// its only reason why am i creating this.
		_DetailAmbientOcclusionMap ("Detail Ambient Occlusion map (UV1)", 2D) = "white" {}
		// Smoothness multiplier.
		_Smoothness ("Smoothness", Range(0, 3)) = 0.5
		// Power of "parallax mapping", its multiplier 
		_DisplacementPower ("Displacement power", Range(0, 0.02)) = 0.015
		// "Why do you wrote all these comments" because its funny, and yes, i dont need them all. This shader is basic and simple.
		// Just read "vectors", and apply. Nothing hard.
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
        // Keywords! We can use them to say shader not to process some things.
        // Why do we cant use simple if statements?
        // Because they are really expensive in GPU. So, macros-power go brr

		sampler2D _DiffuseMap;
		sampler2D _NormalMap;
		sampler2D _SpecularMap;
		sampler2D _DisplacementMap;
		sampler2D _DetailNormalMap;
		sampler2D _DetailAmbientOcclusionMap;
		float _Smoothness;
		float _DisplacementPower;
		// Well... 6 textures is too many, but its minimum in "advanced" shader.
		// What is "advanced" one? Well, its a thing that game have. Its secret. You cant Google it.
		// Its simple to tell you, and it was implemented in Stalker...
		// but i'm lazy to tell what is this and how it works.

		//UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here... ah yes i dont know how to do it properly
		//UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandardSpecular o) {
			// So you decided to see what kind of "magic" i'm doing here?
			// Idk what to say here to be honest. 

			#ifdef _DISPLACEMENT
				float4 displacementColor = tex2D(_DisplacementMap, IN.uv_DiffuseMap);
				IN.uv_DiffuseMap += ParallaxOffset(displacementColor, _DisplacementPower, IN.viewDir);
				IN.uv2_DetailAmbientOcclusionMap += ParallaxOffset(displacementColor, _DisplacementPower, IN.viewDir);
			#endif

			float4 diffuseColor = tex2D(_DiffuseMap, IN.uv_DiffuseMap);
			float4 detailAmbientOcclusionColor = tex2D(_DetailAmbientOcclusionMap, IN.uv2_DetailAmbientOcclusionMap);
			o.Albedo = diffuseColor.rgb * detailAmbientOcclusionColor.r;
			 
			#ifdef _NORMAL 
				float4 normalColor = tex2D(_NormalMap, IN.uv_DiffuseMap);
				float4 detailNormalColor = tex2D(_DetailNormalMap, IN.uv2_DetailAmbientOcclusionMap);
				//normalColor.x += detailNormalColor.x * _NormalDetailPower;
				//normalColor.y += detailNormalColor.y * _NormalDetailPower;
				normalColor += detailNormalColor - 0.5; 
				o.Normal = UnpackNormal(normalColor);
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
