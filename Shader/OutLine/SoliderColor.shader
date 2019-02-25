// 用于描边的纯色Shader
Shader "Volume xx/Outline/Solider Color" {
    Properties {
        _OutlineColor("Color",Color) = (1, 1, 1, 1)
    }
    SubShader {
        Tags{"RenderType"="Opaque" "Queue"="Geometry"}

        // 沿法线挤出一点
        // Pass{
        //     CGPROGRAM
            
        //     #pragma vertex vert
        //     #pragma fragment frag

        //     struct a2v{
        //         float4 vertex : POSITION;
        //         float3 normal : NORMAL;
        //     };
        //     struct v2f{
        //         float4 pos : SV_POSITION;
        //     };

        //     v2f vert(a2v v){
        //         v2f o;
        //         // v.vertex.xyz += 
        //         o.pos = UnityObjectToClipPos(v.vertex);

        //         return o;
        //     }

        //     fixed4 frag(v2f i) : SV_TARGET{
        //         return fixed4(_OutlineColor.rgb,1.0);
        //     }

        //     ENDCG
        // }

        // 直接输出颜色
        Pass {
            Tags {"LightMode"="ForwardBase"}
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            fixed4 _OutlineColor;

            struct a2v{
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct v2f{
                float4 pos : SV_POSITION;
            };

            v2f vert(a2v v){
                v2f o;
                v.vertex.xyz += v.normal * 0.03;
                o.pos = UnityObjectToClipPos(v.vertex);
                
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET{
                return fixed4(_OutlineColor.rgb,1.0);
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
    
}