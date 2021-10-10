// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Dado que Unity no permite hacer lightmaps progresivamente, hemos creado un shader que permite tener lightmaps generados por Unity.
//También permite combinar esos lightmaps con sombras en tiempo real.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Shader "Custom/Lightmapped_Diffuse_Shadows"
{
	Properties{
		_Color ("Main Color", Color) = (1,1,1,1)
		_DiffuseMult ("Albedo Multiplier", Range (1, 5)) = 1
		_DiffuseContrast ("Diffuse Contrast", Range (1, 2)) = 1
		_MainTex ("Base (RGB)", 2D) = "white" {}

		_ColorLightmap ("Lightmap Color", Color) = (1,1,1,1)
		_LightmapMult ("Lightmap Multiplier", Range (0, 7)) = 1.5
		_LightmapContrast ("Lightmap Contrast", Range (0, 3)) = 1
		_LightMap ("Lightmap (RGB)", 2D) = "black" {}
	}

	SubShader
	{
		Tags {"Queue" = "Geometry" "RenderType" = "Opaque"}

		Pass {
			Tags {"LightMode" = "ForwardBase"}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			#pragma fragmentoption ARB_fog_exp2
			#pragma fragmentoption ARB_precision_hint_fastest
			
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			
			struct v2f{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				LIGHTING_COORDS(2,3)
			};

			float4 _MainTex_ST;
			float4 _LightMap_ST;
			fixed4 _Color;
			fixed4 _ColorLightmap;
			half _DiffuseMult;
			half _LightmapMult;
			half _DiffuseContrast;
			half _LightmapContrast;
			sampler2D _MainTex;
			sampler2D _LightMap;

			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			//Vertex Shader
			v2f vert (appdata_full v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos( v.vertex);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex).xy;
				o.uv2 = TRANSFORM_TEX (v.texcoord1, _LightMap).xy;
				TRANSFER_VERTEX_TO_FRAGMENT(o);
				return o;
			}

			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			//Fragment Shader
			fixed4 frag(v2f i) : COLOR
			{
//				fixed atten = LIGHT_ATTENUATION(i);	// Light attenuation + shadows.
				fixed atten = SHADOW_ATTENUATION(i); // Shadows ONLY.

				fixed4 lm = tex2D(_LightMap, i.uv2);
				fixed4 color_lightmap_corrected = lerp(half4(1,1,1,1), _ColorLightmap, 1-lm.b);
				lm *= color_lightmap_corrected * _LightmapMult;

				if(_LightmapContrast != 0){
					lm = saturate(lerp(half4(0.5, 0.5, 0.5, 0.5), lm, _LightmapContrast));
				}
				//Evitamos que las sombras en tiempo real aparezcan encima de las sombras de baked
				atten = (atten * (lm.r - 0.15)) + (1 - (lm.r + 0.15));

				fixed4 col = tex2D(_MainTex, i.uv) * atten * _Color * _DiffuseMult;
				col = saturate(lerp(half4(0.5, 0.5, 0.5, 0.5), col, _DiffuseContrast));

				//Añadimos un poco de color azul a las sombras
				if(atten<0.15){
					col = fixed4(col.r*0.7, col.g*1.2, col.b*1.5, col.a);
				}
				return lm * col;
			}
			ENDCG
		}
	}
	FallBack "VertexLit"
}

