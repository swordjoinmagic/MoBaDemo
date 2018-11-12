Shader "Custom/Water/DiffuseWaterOpaque"
{
	Properties
	{
		_WaterColor ("Water color", Color) = (1, 1, 1, 1)
		_WaterTex ("Water texture", 2D) = "white" {}
		_Tiling ("Water tiling", Vector) = (1, 1, 1, 1)
		_TextureVisibility("Texture visibility", Range(0, 1)) = 1

		[Space(20)]
		_DistTex ("Distortion", 2D) = "white" {}
		_DistTiling ("Distortion tiling", Vector) = (1, 1, 1, 1)

		[Space(20)]
		_WaterHeight ("Water height", Float) = 0

		[Space(20)]
		_MoveDirection ("Direction", Vector) = (0, 0, 0, 0)
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#pragma multi_compile_fog
			#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				fixed4 worldPos: TEXCOORD1;
				fixed camHeightOverWater : TEXCOORD2;
				UNITY_FOG_COORDS(3)
				float4 vertex : SV_POSITION;
			};

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _WaterTex;
			fixed2 _Tiling;
			fixed4 _WaterColor;

			sampler2D _DistTex;
			fixed2 _DistTiling;

			fixed _WaterHeight;
			fixed _TextureVisibility;

			fixed3 _MoveDirection;

			fixed2 WaterPlaneUV(fixed3 worldPos, fixed camHeightOverWater)
			{
				fixed3 camToWorldRay = worldPos - _WorldSpaceCameraPos;
				fixed3 rayToWaterPlane = (camHeightOverWater / camToWorldRay.y * camToWorldRay);
				return rayToWaterPlane.xz - _WorldSpaceCameraPos.xz;
			}

			v2f vert (appdata v)
			{
				v2f o;

				o.worldPos = mul(UNITY_MATRIX_M, v.vertex);
				o.vertex = mul(UNITY_MATRIX_VP, o.worldPos);
				
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.camHeightOverWater = _WorldSpaceCameraPos.y - _WaterHeight;

#if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)
				fixed3 camToWorldRay = o.worldPos - _WorldSpaceCameraPos;
				fixed3 rayToWaterPlane = (o.camHeightOverWater / camToWorldRay.y * camToWorldRay);

				fixed3 worldPosOnPlane = _WorldSpaceCameraPos - rayToWaterPlane;
				fixed3 positionForFog = lerp(worldPosOnPlane, o.worldPos.xyz, o.worldPos.y > _WaterHeight);
				fixed4 waterVertex = mul(UNITY_MATRIX_VP, fixed4(positionForFog, 1));
				UNITY_TRANSFER_FOG(o, waterVertex);
#endif

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed2 water_uv = WaterPlaneUV(i.worldPos, i.camHeightOverWater);
				fixed4 distortion = tex2D(_DistTex, water_uv * _DistTiling) * 2 - 1;
				fixed2 distorted_uv = ((water_uv + distortion.rg) - _Time.y * _MoveDirection.xz) * _Tiling;

				fixed4 waterCol = tex2D(_WaterTex, distorted_uv);
				waterCol = lerp(_WaterColor, fixed4(1, 1, 1, 1), waterCol.r * _TextureVisibility);

				UNITY_APPLY_FOG(i.fogCoord, waterCol);

				return waterCol;
			}
			ENDCG
		}
	}
}
