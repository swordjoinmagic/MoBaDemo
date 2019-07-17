Shader "SJM/UI/WaveHpDemo" {
    Properties {
        // 该UI的图片
        _SpriteImage("Sprite",2D) = "white" {}
        // 阈值
        _Fill("fill",Float) = 1
		// 用于控制物体整体颜色
		_Color("Color Tint",Color) = (1,1,1,1)
		
		// copy的，等会研究
        _ColorMask("Color Mask", Float) = 15
        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
    }
    SubShader {
        Tags { "RenderType"="Transparent" "IgnoreProjector"="False" "Queue"="Transparent" "PreviewType" = "Plane" "CanUseSpriteAtlas" = "True"}
        Stencil{
                Ref[_Stencil]
                Comp[_StencilComp]
                Pass[_StencilOp]
                ReadMask[_StencilReadMask]
                WriteMask[_StencilWriteMask]
            }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest[unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask[_ColorMask]

        Pass {
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                sampler2D _SpriteImage;
                float4 _SpriteImage_ST;
                float _Fill;
				fixed4 _Color;
				
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
                    float4 vertex = v.vertex;
                    vertex.x += sin(_Time.y)*59;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.texcoord,_SpriteImage);
                    return o;
                }
                fixed4 frag(v2f i) : SV_TARGET{
                    // 对贴图进行采样
                    fixed4 color = tex2D(_SpriteImage,i.uv);
                    half val = i.uv.y+ sin((i.uv.x * 4 + _Time.y * 4))*0.8; 
                    val *= 0.1;

                    if(i.uv.y+val > _Fill)
                        color.a = 0; 
					color.rgb = _Color.rgb;
                    return color;
                }

            ENDCG
        }
    }
    FallBack "Diffuse"
    
}