﻿Shader "JopiShaderWorks/Deform"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_WaveScale("WaveScale", Float) = 0.3
		_Scroll ("Scroll", Vector) = (0,0,0,0)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Scroll;
			float _WaveScale;
			float test = 0;
			float test2 = 0;
			
			v2f vert (appdata v)
			{
				test += 0.2f;
				test2 -= 0.2f;
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.vertex.y += sin(worldPos.x + _Scroll.x + test) * _WaveScale;
				o.vertex.y -= sin(worldPos.z + _Scroll.y + test2) * _WaveScale;
				o.vertex.y += sin(-worldPos.y) / _Scroll.z * _WaveScale;
				o.vertex.y -= sin(-worldPos.x) / _Scroll.w * _WaveScale;


				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
