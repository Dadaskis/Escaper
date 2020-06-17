Shader "Custom/Text" {
	Properties {
		_Color ("Text Color", Color) = (1,1,1,1)
		_MainTex ("Font Texture", 2D) = "white" {}
	}
	SubShader {
		Tags { 
			"Queue" = "Transparent" 
			"IgnoreProjector" = "True" 
			"RenderType" = "Transparent" 
		}

		Lighting Off
		Cull Off
		Zwrite Off
		Fog {
			Mode Off
		}
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			Color [_Color]
			SetTexture [_MainTex] {
				combine primary, texture * primary
			}
		}
	}
}
