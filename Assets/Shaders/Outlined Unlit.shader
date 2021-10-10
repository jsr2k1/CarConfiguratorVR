// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Custom/Outlined Unlit"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_MainTexOutline ("Albedo Outline", 2D) = "white" {}
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _Outline ("Outline width", Range (.002, 0.03)) = .005
	}

	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100
		
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGINCLUDE
		#include "UnityCG.cginc"

		sampler2D _MainTexOutline;

		struct v2f {
            float4 pos : SV_POSITION;
            UNITY_FOG_COORDS(0)
            float2 uv : TEXCOORD0;
        };

        uniform float _Outline;
        uniform float4 _OutlineColor;
        float4 _MainTexOutline_ST;

		v2f vert(appdata_base v)
        {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
 
            float3 norm   = normalize(mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal));
            float2 offset = TransformViewToProjection(norm.xy);
 
            o.pos.xy += offset * o.pos.z * _Outline;
			o.uv = TRANSFORM_TEX (v.texcoord, _MainTexOutline);
            UNITY_TRANSFER_FOG(o,o.pos);
            return o;
        }
        ENDCG      
 
        Pass {
            Name "OUTLINE"
            Tags { "LightMode" = "Always" }
            Cull Front
            ZWrite On
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_APPLY_FOG(i.fogCoord, i.color);
                fixed4 texcol = tex2D (_MainTexOutline, i.uv);
				return texcol * _OutlineColor;
            }
            ENDCG
        }
		
		Pass {  
			CGPROGRAM
				#pragma vertex vert_base
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_fog
				
				struct appdata_t {
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f_base {
					float4 vertex : SV_POSITION;
					half2 texcoord : TEXCOORD0;
					UNITY_FOG_COORDS(1)
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				fixed4 _Color;
				
				v2f_base vert_base (appdata_t v)
				{
					v2f_base o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}
				
				fixed4 frag (v2f_base i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.texcoord) * _Color;
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
			ENDCG
		}
	}
}
