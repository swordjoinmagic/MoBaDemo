Shader "Unity Shaders Book/Chapter 14/ToonShading" {
    Properties {
        // 用于控制整体色调
        _Color("Main Color",Color) = (1, 1, 1, 1)
        // 主纹理
        _MainTex("Main Tex",2D) = "white" {}
        // 用于控制漫反射色调的渐变纹理
        _Ramp("Ramp Texture",2D) = "white" {}
        // 用于控制轮廓线宽度
        _Outline("Outline",Range(0,1)) = 0.1
        // 轮廓线颜色
        _OutLineColor("Outline Color",Color) = (0, 0, 0, 1)
        // 高光反射颜色
        _Specular("Specular",Color) = (1, 1, 1, 1)
        // 用于控制高光反射时使用的阈值
        _SpecularScale("Specular Scale",Range(0,0.1)) = 0.01


        _EdgeLightColor("Edge Light Color",Color) = (1, 1, 1, 1)
        _EdgeLightPower("Edge Light Power",Float) = 1
    }
    SubShader {
        
        // 渲染背面的Pass,用于绘制轮廓线
        Pass {
            NAME "OUTLINE"

            // 使用Cull指令把正面的三角面片剔除,而只渲染背面
            Cull Front
            
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float _Outline;
            fixed4 _OutLineColor;
            fixed4 _EdgeLightColor;
            float _EdgeLightPower;

            struct a2v{
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct v2f{
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            v2f vert(a2v v){
                v2f o;

                float4 pos = mul(UNITY_MATRIX_MV,v.vertex);
                // 将顶点坐标沿法线方向扩展一段距离
                // float3 normal = UnityObjectToWorldNormal(v.normal);
                float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);  
                normal.z = -0.5;
                pos = pos+float4(normalize(normal),0) * _Outline;

                o.pos = mul(UNITY_MATRIX_P,pos);
                o.worldNormal = UnityObjectToWorldNormal(normal);
                o.worldPos = mul(unity_ObjectToWorld,mul(o.pos,UNITY_MATRIX_MVP));

                return o;
            }
            fixed4 frag(v2f i) : SV_TARGET{
                fixed3 worldNormal = normalize(i.worldNormal);
                fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
                fixed3 edgeLight = pow(( 1-max( 0,dot( worldViewDir,worldNormal ) ) ) * _EdgeLightColor.rgb,_EdgeLightPower) ;
                
                return fixed4(_OutLineColor.rgb,1.0);
            }

            ENDCG
        }

        // 渲染正面的Pass,用于绘制物体
        Pass{
            Tags{ "LightMode" = "ForwardBase" }
            Cull Back
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_fwdbase

                #include "Lighting.cginc"
                #include "AutoLight.cginc"

                sampler2D _MainTex;
                float4 _MainTex_ST;
                sampler2D _Ramp;
                fixed4 _Color;
                fixed4 _Specular;
                fixed _SpecularScale;

                struct a2v{
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f{
                    float4 pos : SV_POSITION;
                    float3 worldNormal : TEXCOORD0;
                    float3 worldPos : TEXCOORD1;
                    float2 uv : TEXCOORD2;
                    SHADOW_COORDS(3)
                };

                v2f vert(a2v v){
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
                    o.worldNormal = UnityObjectToWorldNormal(v.normal);
                    o.worldPos = mul(unity_ObjectToWorld,v.vertex);

                    TRANSFER_SHADOW(o);

                    return o;
                }

                fixed4 frag(v2f i) : SV_TARGET{
                    fixed3 worldNormal = normalize(i.worldNormal);
                    fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
                    fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
                    fixed3 worldHalfDir = normalize(worldLightDir+worldViewDir);

                    fixed4 c = tex2D(_MainTex,i.uv);
                    fixed3 albedo = c.rgb * _Color.rgb;

                    fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;

                    UNITY_LIGHT_ATTENUATION(atten,i,i.worldPos);

                    // 计算辐照度
                    fixed diff = dot(worldNormal,worldLightDir);
                    // 应用半兰伯特模型
                    diff = (diff*0.5+0.5) * atten;
  
                    fixed3 diffuse = _LightColor0.rgb * albedo * tex2D(_Ramp,float2(diff,diff)).rgb;

                    fixed spec = dot(worldNormal,worldHalfDir);
                    fixed w = fwidth(spec) * 2.0;

                    fixed3 specular = _Specular.rgb * smoothstep(-w,w,spec+_SpecularScale-1) * step(0.0001,_SpecularScale);

                    return fixed4(ambient+diffuse+specular,1.0);
                    
                }

            ENDCG
        }
    }
    FallBack "Diffuse"
    
}