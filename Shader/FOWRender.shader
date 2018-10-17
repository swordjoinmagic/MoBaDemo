// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "HOG/FOWRender"
{
	Properties
	{
		_MainTex("Fog Texture", 2D) = "white" {}
		_Unexplored("Unexplored Color", Color) = (0.05, 0.05, 0.05, 0.05)
		_Explored("Explored Color", Color) = (0.35, 0.35, 0.35, 0.35)
		_BlendFactor("Blend Factor", range(0,1)) = 0
	}
	
	SubShader
	{
		Tags{ "Queue" = "Transparent+151" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		ZTest Off
		// Cull Back
		
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag	
			#pragma fragmentoption ARB_precision_hint_fastest 

			#include "UnityCG.cginc" 

			sampler2D _MainTex;
			uniform half4 _Unexplored;
			uniform half4 _Explored;
			uniform half _BlendFactor;

			struct v2f
			{
				half4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			};

			v2f vert(in appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}
			fixed4 frag(v2f i) : SV_Target
			{
				half4 data = tex2D(_MainTex, i.uv);
				half2 fog = lerp(data.rg, data.ba, _BlendFactor);
				half4 color = lerp(_Unexplored, _Explored, fog.g);
				color.a = (1 - fog.r) * color.a;
				return color;
			}

			ENDCG 
		}	
	}
	Fallback off
}