// 用于将轮廓图和场景图叠加的Shader
Shader "Volume xx/Outline/Outline Addtional" {
    Properties {
        // 主纹理，即场景图
        _MainTex("Main Texture",2D) = "white" {}
        // 即轮廓图
        _BlurTex("Blur Texture",2D) = "white" {}
        // 描边的强度
        _OutlineStrength("OutlineStrength",Range(1.0,10)) = 3.0
    }
    SubShader {
        Pass {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            sampler2D _BlurTex;
            float _OutlineStrength;

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
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET{

                // 场景图
                fixed4 source = tex2D(_MainTex,i.uv);
                // 轮廓图
                fixed4 other = tex2D(_BlurTex,i.uv);

                // 结果颜色
                fixed4 final = source + other*_OutlineStrength;

                // 模糊图(胖的)减去原图(瘦的)
                return final;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
    
}