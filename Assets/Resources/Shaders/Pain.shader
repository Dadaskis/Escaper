Shader "Hidden/Pain"
{
	Properties
	{
		_MainTex ("Screen texture", 2D) = "white" {}
		_BloodyTexture ("Bloody texture (overlayed)", 2D) = "white" {}
		_FogColor("Fog color", Color) = (1, 1, 1, 1)
		_FogDistance("Fog distance", float) = 0.0
		_FogRefractionPower("Fog refraction power", float) = 0.0
		_DisplacementTex("Displacement texture", 2D) = "white" {}
		_EffectPower ("Effect power", float) = 1.0
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
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _BloodyTexture;
			sampler2D _CameraDepthTexture;
			sampler2D _DisplacementTex;
			float _FogDistance;
			float _FogPower;
			float4 _FogColor;
			float _EffectPower;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 mainColor = tex2D(_MainTex, i.uv);
				fixed4 result = mainColor;
				fixed4 displacement = tex2D(_DisplacementTex, i.uv + sin(_Time.y));

				float depth = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv));
				float fogIssuePower = ((0.4 + (sin(_Time.y) * 0.1)) - distance(half2(0.5, 0.5), i.uv)) * 10.0;
				fogIssuePower = clamp(fogIssuePower, 0.0, 1.0);

				depth = max(0.0, depth) * _FogDistance;
				depth = clamp(depth, 0.0, 1.0);
				float4 refractionedResult = tex2D(_MainTex, i.uv + (displacement.xy * 0.2));
				float4 foggyResult = lerp(refractionedResult, _FogColor, depth);

				//result = lerp(result * fogIssue, result, fogIssuePower);
				//result *= fogIssue

				result = lerp(foggyResult, result, fogIssuePower);
				//result = fogIssuePower;

				fixed4 bloodyColor = tex2D(_BloodyTexture, i.uv);
				float bloodyBlending = abs(0.5 + (sin(_Time.y) * 0.1)) * 5.0;
				bloodyBlending = clamp(bloodyBlending + 0.5, 0.95, 1.0);
				result = lerp(result, bloodyColor, bloodyColor.a * bloodyBlending);

				//result = fogIssue;

				return lerp(mainColor, result, _EffectPower);
			}
			ENDCG
		}
	}
}
