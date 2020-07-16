Shader "Custom/UnderWaterPostProcessing"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1, 1, 1, 1)
		_FogColor("Fog color", Color) = (1, 1, 1, 1)
		_FogDistance("Fog distance", float) = 0.0
		_FogRefractionPower("Fog refraction power", float) = 0.0
		_FogPower("Fog power", float) = 0.0
		_NoiseTex("Noise texture", 2D) = "white" {}
		_NoiseDepthMultiplier("Noise depth multiplier", float) = 0.0
		_NoiseDepthOffset("Noise depth offset", float) = 0.0
		_NoiseFogMultiplier("Noise fog multiplier", float) = 0.0
		_NoiseFogOffset("Noise fog offset", float) = 0.0
		_DisplacementTex("Displacement texture", 2D) = "white" {}
		_RefractionMix("Refraction mix", float) = 0.1
		_RefractionPower("Refraction displacement power", float) = 1.0
		_RefractionDepthMultiplier("Refraction depth multiplier", float) = 1.0
		_RefractionDepthOffset("Refraction depth offset", float) = 1.0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 ray : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			//float3 RotateAroundYAxis(float3 v, float deg)
		    //{
		    //    float alpha = deg * UNITY_PI / 180.0;
		    //    float sina, cosa;
		    //    sincos(alpha, sina, cosa);
		    //    float2x2 m = float2x2(cosa, -sina, sina, cosa);
		    //    return float3(mul(m, v.xz), v.y).xzy;
		    //}

			v2f vert (appdata_full v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;
				//o.ray = RotateAroundYAxis(v.texcoord1.xyz, 0.0);
				return o;
			}

			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;
			sampler2D _NoiseTex;
			sampler2D _DisplacementTex;
			float4 _FogColor;
			float4 _Color;
			float _FogDistance;
			float _FogRefractionPower;
			float _FogPower;
			float _RefractionMix;
			float _RefractionPower;
			float _RefractionDepthMultiplier;
			float _RefractionDepthOffset;
			float _NoiseDepthMultiplier;
			float _NoiseDepthOffset;
			float _NoiseFogMultiplier;
			float _NoiseFogOffset;

			float4 ApplyFog(float4 col, float depth, float noise) {
				depth = max(0.0, depth) * _FogDistance;
				depth = saturate(depth + ((noise * _NoiseDepthMultiplier) + _NoiseDepthOffset));
				float4 colFoggy = lerp(col * _FogPower, _FogColor * ((noise * _NoiseFogMultiplier) + _NoiseFogOffset), depth);
				col = lerp(col, colFoggy, depth);
				//col *= _FogColor * ((noise * _NoiseFogMultiplier) + _NoiseFogOffset);
				return col;
			}

			float4 ApplyRefraction(float4 col, float2 displacement, float2 uv, float depth) {
				depth = 1.0 - depth;
				col = lerp(col, tex2D(_MainTex, uv + (displacement * _RefractionPower)), (_RefractionMix * _RefractionDepthMultiplier) + _RefractionDepthOffset);
				return col;
			}

			float4 ApplyColor(float4 col0, float4 col1) {
				return (col0 * col1) + col1;
			}

			float4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);
				float depth = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv));
				float2 uv = i.uv + (_Time.y * 0.02);
				float2 displacement = tex2D(_DisplacementTex, uv);
				float noise = tex2D(_NoiseTex, (uv * 2.0) + ((displacement + sin(_Time.y * 0.03)) * _FogRefractionPower));
				col = ApplyColor(col, _Color);
				col = ApplyRefraction(col, displacement, i.uv, depth);
				col = ApplyFog(col, depth, noise);

				return col;
			}
			ENDCG
		}
	}
}
