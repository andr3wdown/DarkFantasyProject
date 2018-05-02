Shader "Custom/WindWaker" 
{
	Properties
	{
		_ShowTroughColor("ShowTroughColor", Color) = (0,0,0,0)
		_Color("Color", Color) = (1,1,1,1) //Color multiplied to the texture
		_MainTex("Albedo (RGB)", 2D) = "white" {} //Texture
		_CelShadingBlurWidth("Cell Shading Blur Width", Range(0,2)) = 0.2 //Blur between thresholds
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparrent" }
		/*Pass
		{
			
			Cull Off
			ZWrite Off
			ZTest Always

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
			};

			float4 _ShowTroughColor;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return _ShowTroughColor;
			}
			ENDCG
		}*/


		Tags{ "RenderType" = "Transparrent" }
		LOD 200

		CGPROGRAM

#pragma surface surf Toon fullforwardshadows 

#pragma target 3.0

	sampler2D _MainTex;
	sampler2D _RampTex;

	struct Input {
		float2 uv_MainTex;
	};

	half _CelShadingBlurWidth;
	fixed4 _Color;

	void surf(Input IN, inout SurfaceOutput o) {

		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}

	fixed4 LightingToon(SurfaceOutput s, fixed3 lightDir,fixed atten)
	{
		half NdotL = dot(s.Normal, lightDir);  //Value between 0 and 1

        half cel;

        /// 0 | threshold 1  |  blur  | threshold 2 | 1
        /// 0 |**************|<- .5 ->|xxxxxxxxxxxxx| 1

		if (NdotL < 0.5 - _CelShadingBlurWidth / 2)                                         // Outside of the blur but dark
            cel = 0;
		else if (NdotL > 0.5 + _CelShadingBlurWidth / 2)                                    // Outside of the blur but lit
            cel = 1;
		else                                                                                // Inside of the blur 
            cel = 1- ((0.5 + _CelShadingBlurWidth / 2 - NdotL) / _CelShadingBlurWidth);

		half4 c;

		c.rgb = (cel + 0.3)/2.5  * s.Albedo * _LightColor0.rgb * atten; // So it does not look too lit
		c.a = s.Alpha;

		return c;
	}

	ENDCG
	}
		FallBack "Diffuse"
}
