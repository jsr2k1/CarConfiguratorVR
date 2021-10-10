
Shader "Custom/Lightmapped_Diffuse"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_DiffuseMult ("Albedo Multiplier", Range (1, 2)) = 1
		_DiffuseContrast ("Diffuse Contrast", Range (1, 2)) = 1
		_MainTex ("Albedo (RGB)", 2D) = "white" {}

		_ColorLightmap ("Lightmap Color", Color) = (1,1,1,1)
		_LightmapMult ("Lightmap Multiplier", Range (0, 7)) = 1.5
		_LightmapContrast ("Lightmap Contrast", Range (0, 3)) = 1
		_LightMap ("Lightmap (RGB)", 2D) = "black" {}
	}

	SubShader {
		LOD 200
		Tags { "RenderType" = "Opaque" }

		CGPROGRAM
		#pragma surface surf Lambert nodynlightmap

		struct Input
		{
		  float2 uv_MainTex;
		  float2 uv2_LightMap;
		};

		sampler2D _MainTex;
		sampler2D _LightMap;
		fixed4 _Color;
		fixed4 _ColorLightmap;
		half _LightmapMult;
		half _DiffuseMult;
		half _LightmapContrast;
		half _DiffuseContrast;

		void surf (Input IN, inout SurfaceOutput o)
		{
//		  o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * _Color;
//		  float4 lm = tex2D (_LightMap, IN.uv2_LightMap) * _ColorLightmap * _LightmapMult;

		  o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * _Color * _DiffuseMult;
		  o.Albedo = saturate(lerp(half4(0.5, 0.5, 0.5, 0.5), o.Albedo, _DiffuseContrast));

		  half4 lm = tex2D (_LightMap, IN.uv2_LightMap) * _ColorLightmap * _LightmapMult;
		  float4 lm_contrasted = saturate(lerp(half4(0.5, 0.5, 0.5, 0.5), lm, _LightmapContrast));

		  o.Emission = lm_contrasted.rgb * o.Albedo.rgb;

//		  o.Albedo = 8.0 * lm_contrasted.a * lm_contrasted.rgb * o.Albedo.rgb;
//		  o.Albedo = 4.0 * lm_contrasted.a * lm_contrasted.rgb * o.Albedo.rgb;
		}
		ENDCG
	}
	FallBack "Legacy Shaders/Lightmapped/VertexLit"
}
