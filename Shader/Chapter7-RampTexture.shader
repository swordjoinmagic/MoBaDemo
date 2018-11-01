// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unity Shaders Book/Chapter 7/RampTexture" {
    Properties {
        // 物体整体颜色
        _Color("Color Tint",Color) = (1, 1, 1, 1)
        // 渐变纹理
        _RampTex("Ramp Tex",2D) = "white" {}
        // 材质高光反射的颜色
        _Specular("Specular",Color) = (1, 1, 1, 1)
        // 光泽度
        _Gloss("Gloss",Range(8.0,256)) = 20
    }
    SubShader {
        Pass {
            Tags { "LightMode" = "ForwardBase" }
            CGPROGRAM

                #pragma vertex vert
                #pragma fragment frag

                #include "Lighting.cginc"

                fixed4 _Color;
                sampler2D _RampTex;
                float4 _RampTex_ST;
                fixed4 _Specular;
                float _Gloss;

                // 定义顶点着色器的输入
                struct a2v{
                    float4 vertex : POSITION;
                    // 顶点法线
                    float3 normal : NORMAL;
                    // 输入纹理
                    float4 texcoord : TEXCOORD0;
                };

                // 定义顶点着色器的输出
                struct v2f{
                    float4 pos : SV_POSITION;
                    float3 worldNormal : TEXCOORD0;
                    float3 worldPos : TEXCOORD1;
                    float2 uv : TEXCOORD2;
                };

                v2f vert(a2v v){
                    v2f o;
                    // 变换顶点
                    o.pos = UnityObjectToClipPos(v.vertex);

                    // 计算世界坐标下的法线
                    o.worldNormal = UnityObjectToWorldNormal(v.normal);
                    // 计算顶点在世界空间下的坐标
                    o.worldPos = mul(unity_ObjectToWorld,v.vertex).xyz;

                    // 计算偏移-缩放后的uv坐标
                    o.uv = TRANSFORM_TEX(v.texcoord,_RampTex);

                    return o;
                }

                fixed4 frag(v2f i) : SV_TARGET{
                    fixed3 worldNormal = normalize(i.worldNormal);
                    fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));

                    // 获得环境光
                    fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

                    // 获得半兰伯特光照模型常量
                    fixed halfLambert = 0.5 * dot(worldNormal,worldLightDir) + 0.5;

                    fixed3 diffuseColor = tex2D(_RampTex,fixed2(halfLambert,halfLambert)).rgb * _Color.rgb;

                    fixed3 diffuse = _LightColor0.rgb * diffuseColor;

                    fixed3 viewDir = normalize( UnityWorldSpaceViewDir(i.worldPos) );
                    fixed3 halfDir = normalize(worldLightDir+viewDir);
                    
                    // 计算高光反射
                    fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(max(0,dot(worldNormal,halfDir)),_Gloss);

                    return fixed4(ambient+diffuse+specular,1.0);
                }

            ENDCG
        }
    }
    FallBack "Diffuse"
    
}