// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Projection2" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader {
	Pass{
		Tags {"RenderType"="Transparent"}
		LOD 200
		ZWrite off
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		float4x4 unity_Projector;

		struct v2f
		{
			float4 pos:SV_POSITION;
			float4 texc:TEXCOORD0;
		};

		v2f vert(appdata_base v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			//将顶点变换到矩阵空间
			o.texc = mul(unity_Projector,v.vertex);
			return o;
		}

		float4 frag(v2f o):COLOR
		{
			//对光环图片进行投影采样
			float4 c = tex2Dproj(_MainTex,o.texc);
			//限制投影方向
			c = c*step(0,o.texc.w);
            if(c.r>0.5 && c.g<=0.5 && c.b<=0.5)
                return fixed4(0,1,0,0.4);
            return c;
		}

		ENDCG
		}
	}
	FallBack "Diffuse"
}