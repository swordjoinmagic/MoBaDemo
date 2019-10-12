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

			// 表示战争迷雾贴图,其中r通道表示当前区域,g通道表示已通行区域
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
				// r通道表示当前区域(即由当前所有单位所揭开的战争迷雾区域),如果某个区域是亮的,则当前r值为1.0
				// g通道表示已经通过的区域,如果某个通道之前已经通过过,那么该区域g值为1.0
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