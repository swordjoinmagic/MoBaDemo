// 边缘光尝试
// 只计算边缘处的光照
Shader "SJM/EdgeLight" {
    Properties {
        _Diffuse("Diffuse",Color) = (1, 1, 1, 1)
        _EdgeColor("EdgeColor",Color) = (1, 1, 1, 1)
        _EdgeLightPower("Power",Float) = 1
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        Pass {
            Tags { "LightMode"="ForwardBase" }
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"
                #include "Lighting.cginc"

                fixed4 _Diffuse;
                fixed4 _EdgeColor;
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
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.worldPos = mul(unity_ObjectToWorld,v.vertex);
                    o.worldNormal = UnityObjectToWorldNormal(v.normal);
                    return o;
                }

                fixed4 frag(v2f i) : SV_TARGET{
                    fixed3 worldNormal = normalize(i.worldNormal);
                    fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));

                    // 环境光
                    fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;

                    // 计算漫反射
                    fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * max(0,dot(worldLightDir,worldNormal));

                    fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));

                    // 如果是边缘，让其自然变亮
                    fixed3 edgeLight = ( 1-max( 0,dot( worldViewDir,worldNormal ) ) ) * _EdgeColor.rgb * _EdgeLightPower;

                    return fixed4(diffuse+ambient+edgeLight,1.0);
                }
            ENDCG
        }
    }
    FallBack Off
    
}