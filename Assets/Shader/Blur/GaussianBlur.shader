// 用于屏幕后处理的高斯模糊效果
Shader "Volume xx/PostEffect/Gaussian Blur" {
    Properties {
        // 主纹理
        _MainTex("Main Texture",2D) = "white" {}
        // 模糊半径
        _BlurSize("Blur Size",Float) = 1.0
    }
    SubShader {

        CGINCLUDE

        #include "UnityCG.cginc"

        sampler2D _MainTex;
        // 纹素，x = 1/width, y = 1/height, z = width, w = height
        half4 _MainTex_TexelSize;
        float _BlurSize;

        struct v2f{
            float4 pos : SV_POSITION;
            half2 uv[5] : TEXCOORD0;
        };

        // 使用竖直方向的高斯核进行滤波
        v2f vertBlurVertical(appdata_img v){
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);

            half2 uv = v.texcoord;

            // 原点
            o.uv[0] = uv;
            // 原点上面的像素
            o.uv[1] = uv + float2(0.0,_MainTex_TexelSize.y*1.0) * _BlurSize;
            // 原点下面的像素
            o.uv[2] = uv - float2(0.0,_MainTex_TexelSize.y*1.0) * _BlurSize;
            // 原点上面的上面的像素
            o.uv[3] = uv + float2(0.0,_MainTex_TexelSize.y*2.0) * _BlurSize;
            // 原点下的下面的像素
            o.uv[4] = uv - float2(0.0,_MainTex_TexelSize.y*2.0) * _BlurSize;

            return o;
        }

        v2f vertBlurHorizontal(appdata_img v){
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);

            half2 uv = v.texcoord;

            // 原点
            o.uv[0] = uv;
            // 原点右边的像素
            o.uv[1] = uv + float2(_MainTex_TexelSize.y*1.0,0.0) * _BlurSize;
            // 原点左边的像素
            o.uv[2] = uv - float2(_MainTex_TexelSize.y*1.0,0.0) * _BlurSize;
            // 原点右边的右边的像素
            o.uv[3] = uv + float2(_MainTex_TexelSize.y*2.0,0.0) * _BlurSize;
            // 原点左边的左边的像素
            o.uv[4] = uv - float2(_MainTex_TexelSize.y*2.0,0.0) * _BlurSize;

            return o;  
        }

        fixed4 fragBlur(v2f i) : SV_TARGET{
            float weight[3] = {0.4026,0.242,0.0545};
        
            fixed3 sum = tex2D(_MainTex,i.uv[0]).rgb * weight[0];

            for(int a=1;a<3;a++){
                sum += tex2D(_MainTex,i.uv[a*2-1]).rgb * weight[a];
                
                sum += tex2D(_MainTex,i.uv[a*2]).rgb * weight[a];
            }

            return fixed4(sum,1.0);
        }

        ENDCG

        // 使用竖直方向高斯核进行卷积的Pass
        Pass {
            NAME "GAUSSIAN_BLUR_VERTICAL"

            CGPROGRAM
            
            #pragma vertex vertBlurVertical
            #pragma fragment fragBlur

            ENDCG
        }

        // 使用水平方向高斯核进行卷积的Pass
        Pass{
            NAME "GAUSSIAN_BLUR_HORIZONTAL"

            CGPROGRAM

            #pragma vertex vertBlurHorizontal
            #pragma fragment fragBlur

            ENDCG
        }
    }
    FallBack "Diffuse"
    
}