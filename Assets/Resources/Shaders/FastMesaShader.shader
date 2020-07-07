Shader "Custom/FastMesaShader" {
	Properties {
	    _Color ("Main Color", Color) = (1,1,1,1)
	    _MainTex ("Base (RGB)", 2D) = "white" {}
	    [HDR] _EmissionColor("Color", Color) = (0,0,0)
        _EmissionMap("Emission", 2D) = "white" {}
	}

	SubShader {
	    LOD 200
	    Tags { "RenderType" = "Opaque" }
		CGPROGRAM

		#pragma surface surf Lambert nodynlightmap

		struct Input {
		  float2 uv_MainTex;
		  float2 uv_EmissionMap;
		};

		sampler2D _MainTex;
		sampler2D _EmissionMap;
		fixed4 _EmissionColor;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o)
		{
		  o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * _Color;
		  half4 emissionValue = tex2D (_EmissionMap, IN.uv_EmissionMap);
		  o.Emission = (emissionValue.rgb * _EmissionColor) * o.Albedo.rgb;
		}

		ENDCG

	}
	FallBack "Legacy Shaders/Lightmapped/VertexLit"
}
