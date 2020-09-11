Shader "Hidden/FadeOut"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_CurrentTime ("Current time", float) = 0.0
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
			float _CurrentTime;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 result;
				fixed4 mainColor = tex2D(_MainTex, i.uv);

				result = mainColor;

				float vignette = ((1.0 - _CurrentTime) - distance(half2(0.5, 0.5), i.uv)) * (5.0 + _CurrentTime);
				vignette = clamp(vignette, 0.0, 1.0);

				result = mainColor * vignette;

				return result;
			}
			ENDCG
		}
	}
}
