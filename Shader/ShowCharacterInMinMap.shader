Shader "SJM/ShowCharacterInMinMap" {
	Properties {
		// 显示单位的一张贴图
		_MainTex("MainTex",2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True" }
		ZWrite Off
		Cull Off
		Lighting Off
		Blend SrcAlpha OneMinusSrcAlpha
		Pass {
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				sampler2D _MainTex;
				float4 _MainTex_ST;

				struct a2v{
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
				};
				struct v2f{
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
				};

				v2f vert(a2v v){
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
					return o;
				}
				fixed4 frag(v2f i) : SV_TARGET{
					fixed4 color = tex2D(_MainTex,i.uv);
					color.a = color.r+color.g;
					color.b = 0;
					return color;  
				}

			ENDCG
		}
	}
	FallBack "Diffuse"
}
