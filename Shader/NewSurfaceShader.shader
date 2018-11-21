Shader "Custom/IntersectField"
{
    Properties
    {
        _MainColor("Main Color", Color) = (1,1,1,1)
        _IntersectionPower("Intersect Power", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }

        Pass
        {
            ZWrite Off
            Cull Off
            Blend DstAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD1;
            };

            sampler2D _CameraDepthTexture;
            fixed4 _MainColor;
            float _IntersectionPower;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                COMPUTE_EYEDEPTH(o.screenPos.z);
                return o;
            }
            float OrthoLinearEyeDepth(float z)
            {
                float far = _ProjectionParams.z;
                float near = _ProjectionParams.y;
                return z * (far - near) + near;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                //透视投影用LinearEyeDepth，正交投影用OrthoLinearEyeDepth
                float screenZ = OrthoLinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
                float diff = 1 - (screenZ - i.screenPos.z);
                float intersect = diff * _IntersectionPower;
                fixed4 c = _MainColor * intersect;
                return c;
            }
            ENDCG
        }
    }
}