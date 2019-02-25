// 用于描边的纯色Shader
Shader "Volume xx/Outline/Outline Substraction" {
    Properties {
        // 主纹理，即目标的纯色图
        _MainTex("Main Texture",2D) = "white" {}
        // 经过高斯模糊后的纯色图
        _BlurTex("Blur Texture",2D) = "white" {}
    }
    SubShader {
        Pass {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            sampler2D _BlurTex;

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

                // 原图
                fixed4 source = tex2D(_MainTex,i.uv);
                // 模糊图
                fixed4 other = tex2D(_BlurTex,i.uv);

                // 模糊图(胖的)减去原图(瘦的)
                return fixed4(source.rgb - other.rgb,1.0);
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
    
}