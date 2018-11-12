Shader "Custom/Water/TwoSide/Depth/DiffuseWater"
{
	Properties
	{
		_Color ("Color", Color) = (1, 1, 1, 1)
		_MainTex ("Texture", 2D) = "white" {}
		
		[Space(20)]
		_WaterColor ("Water color", Color) = (1, 1, 1, 1)
		_WaterTex("Water texture", 2D) = "white" {}
		_Tiling ("Water tiling", Vector) = (1, 1, 1, 1)
		_TextureVisibility ("Texture visibility", Range(0, 1)) = 1

		[Space(20)]
		_DistTex ("Distortion", 2D) = "white" {}
		_DistTiling ("Distortion tiling", Vector) = (1, 1, 1, 1)

		[Space(20)]
		//_DeepColor ("Water deep color", Color) = (1, 1, 1, 1)
		_WaterHeight ("Water height", Float) = 0
		_WaterDeep ("Water deep", Float) = 0
		_WaterDepth ("Water depth param", Range(0, 0.1)) = 0
		_WaterMinAlpha ("Water min alpha", Range(0, 1)) = 0
		
		[Space(20)]
		_BorderColor ("Border color", Color) = (1, 1, 1, 1)
		_BorderWidth ("Border width", Range(0, 1)) = 0

		[Space(20)]
		_MoveDirection ("Direction", Vector) = (0, 0, 0, 0)
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" "BW" = "TrueProbes" "LightMode" = "ForwardBase" }
		LOD 100
		Cull Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"
			#pragma multi_compile_fog
			#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float2 uv : TEXCOORD0;
#if LIGHTMAP_ON
				float2 lightmap_uv : TEXCOORD1;
#endif
		};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				fixed4 worldPos : TEXCOORD1;
				fixed camHeightOverWater : TEXCOORD2;
				fixed waterDepth : TEXCOORD3;
				UNITY_FOG_COORDS(4)
#if LIGHTMAP_ON
				fixed2 lightmap_uv : TEXCOORD5;
#else
				fixed4 diffuseLight : TEXCOORD5;
#endif
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

			fixed4 _DeepColor;
			fixed _WaterHeight;
			fixed _TextureVisibility;
			fixed _WaterDeep;
			fixed _WaterDepth;
			fixed _WaterMinAlpha;

			fixed4 _BorderColor;
			fixed _BorderWidth;
			fixed _BorderVisibility;

			fixed3 _MoveDirection;

			fixed4 DiffuseLight(fixed3 worldNormal)
			{
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));

				fixed4 diff = nl * _LightColor0;
				diff.rgb += ShadeSH9(half4(worldNormal, 1));

				return diff;
			}

			fixed2 WaterPlaneUV(fixed3 worldPos, fixed camHeightOverWater)
			{
				fixed3 camToWorldRay = worldPos - _WorldSpaceCameraPos;
				fixed3 rayToWaterPlane = (camHeightOverWater / camToWorldRay.y * camToWorldRay);
				return rayToWaterPlane.xz - _WorldSpaceCameraPos.xz;
			}

			fixed3 LightmapColor(fixed2 lightmap_uv)
			{
				fixed4 lightmapCol = UNITY_SAMPLE_TEX2D(unity_Lightmap, lightmap_uv);
				return DecodeLightmap(lightmapCol);
			}

			fixed4 MainColor(v2f i)
			{
				fixed4 mainCol = tex2D(_MainTex, i.uv) * _Color;
#if LIGHTMAP_ON
				mainCol.rgb *= LightmapColor(i.lightmap_uv);
#else
				mainCol.rgb *= i.diffuseLight;
#endif

				return mainCol;
			}

			v2f vert (appdata v)
			{
				v2f o;

				o.worldPos = mul(UNITY_MATRIX_M, v.vertex);
				o.vertex = mul(UNITY_MATRIX_VP, o.worldPos);
				
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				fixed3 camToWorldRay = o.worldPos - _WorldSpaceCameraPos;
				o.camHeightOverWater = _WorldSpaceCameraPos.y - _WaterHeight;

				fixed3 rayToWaterPlane = o.camHeightOverWater / (-camToWorldRay.y) * camToWorldRay;
				fixed depth = length(camToWorldRay - rayToWaterPlane);
				o.waterDepth = depth * _WaterDepth * saturate(rayToWaterPlane.y - camToWorldRay.y);;

#if LIGHTMAP_ON
				o.lightmap_uv = v.lightmap_uv.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#else
				fixed4 worldNormal = normalize(mul(UNITY_MATRIX_M, float4(v.normal.xyz, 0)));
				o.diffuseLight = DiffuseLight(worldNormal.xyz);
#endif

#if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)

				fixed3 worldPosOnPlane = _WorldSpaceCameraPos + rayToWaterPlane;
				fixed3 positionForFog = lerp(worldPosOnPlane, o.worldPos.xyz, o.worldPos.y > _WaterHeight);
				fixed4 waterVertex = mul(UNITY_MATRIX_VP, fixed4(positionForFog, 1));
				UNITY_TRANSFER_FOG(o, waterVertex);
#endif

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed lengthUnderWater = max(0, _WaterHeight - i.worldPos.y);
				fixed underWater = lerp(0, 1, lengthUnderWater > 0);
				fixed borderAlpha = lerp(underWater * _BorderColor.a, 0, saturate(lengthUnderWater / _BorderWidth));
				fixed waterAlpha = saturate(lengthUnderWater / _WaterDeep + _WaterMinAlpha + i.waterDepth);

				fixed4 mainCol = MainColor(i);

				fixed2 water_uv = WaterPlaneUV(i.worldPos, i.camHeightOverWater);
				fixed4 distortion = tex2D(_DistTex, water_uv * _DistTiling) * 2 - 1;
				fixed2 distorted_uv = ((water_uv + distortion.rg) - _Time.y * _MoveDirection.xz) * _Tiling;

				fixed4 waterCol = tex2D(_WaterTex, distorted_uv);
				waterCol = lerp(_WaterColor, fixed4(1, 1, 1, 1), waterCol.r * _TextureVisibility);

				fixed4 finalCol = lerp(mainCol, waterCol, _WaterColor.a * waterAlpha * underWater);
				finalCol.rgb = lerp(finalCol.rgb, _BorderColor.rgb, borderAlpha);

				UNITY_APPLY_FOG(i.fogCoord, finalCol);

				//return fixed4(reflection, 0, 0, 1);
				return finalCol;
			}
			ENDCG
		}
	}
}
