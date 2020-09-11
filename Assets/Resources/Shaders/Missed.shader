Shader "Hidden/Missed"
{
	Properties
	{
		_MainTex ("Screen texture", 2D) = "white" {}
		_DisplacementMap ("Displacement map", 2D) = "white" {}
		_MissedColor ("Missed color", Color) = (1, 1, 1, 1)
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
			sampler2D _DisplacementMap;
			fixed4 _MissedColor;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 result;

				fixed4 mainColor = tex2D(_MainTex, i.uv);

				float vignette = (0.65 - distance(half2(0.5, 0.5), i.uv)) * 5.0;
				vignette = 1.0 - clamp(vignette, 0.0, 1.0);
				//result = vignette;

				fixed4 refraction = tex2D(_DisplacementMap, i.uv * sin(_Time.y));
				fixed4 refractionMainColor = tex2D(_MainTex, i.uv + (refraction.xy * 0.03));

				float4 missedResult = (refractionMainColor * 3.0) * _MissedColor + (_MissedColor * 0.03);

				result = lerp(mainColor, missedResult, vignette.r);

				return result;
			}
			ENDCG
		}
	}
}
