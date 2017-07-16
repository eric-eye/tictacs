// Default shader for Grid Framework

Shader "GridFramework/DefaultShader" {
	SubShader {
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off Cull Off Fog {
				Mode Off
			}
			BindChannels {
				Bind "vertex", vertex Bind "color", color
			}
		}
	}
}

