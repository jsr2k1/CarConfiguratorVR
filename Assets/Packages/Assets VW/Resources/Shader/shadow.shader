// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "VW Shaders/Shadow-Unity5" { Properties { _Occlusionmap ("Occlusionmap", 2D) = "white" {} _Strength ("Strength", Range(0, 5) ) = 1  } SubShader { Tags { "RenderType"="Transparent" "Queue"="Transparent-10" "IgnoreProjector"="True" } Pass { Name "FORWARD" Tags { "LightMode"="ForwardBase" } Blend SrcAlpha OneMinusSrcAlpha Cull Back 
CGPROGRAM

#pragma vertex vert
#pragma fragment frag
#define UNITY_PASS_FORWARDBASE
#define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
#define _GLOSSYENV 1
#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "Lighting.cginc"
#include "UnityPBSLighting.cginc"
#include "UnityStandardBRDF.cginc"
#include "gridcell_wireframe.cginc"
#pragma multi_compile_fwdbase_fullshadows
#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
#pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
#pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
#pragma shader_feature WIREFRAME_ON
#pragma exclude_renderers xbox360 ps3
#pragma target 3.0
uniform sampler2D _Occlusionmap; uniform float4 _Occlusionmap_ST; uniform half _Strength; uniform half _WireframeBorder; struct VertexInput { float4 vertex : POSITION; float3 normal : NORMAL; float4 tangent : TANGENT; float2 texcoord0 : TEXCOORD0; float2 texcoord1 : TEXCOORD1; float2 texcoord2 : TEXCOORD2; }; VertexOutput vert (VertexInput v) { VertexOutput o = (VertexOutput)0; o.uv0 = v.texcoord0; o.uv1 = v.texcoord1; 
#ifdef LIGHTMAP_ON
o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw; o.ambientOrLightmapUV.zw = 0; 
#elif UNITY_SHOULD_SAMPLE_SH
#endif
#ifdef DYNAMICLIGHTMAP_ON
o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw; 
#endif
o.normalDir = UnityObjectToWorldNormal(v.normal); o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz ); o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w); o.posWorld = mul(unity_ObjectToWorld, v.vertex); o.pos = UnityObjectToClipPos(v.vertex); TRANSFER_VERTEX_TO_FRAGMENT(o) return o; } fixed4 frag(VertexOutput i, float l5Input : VFACE) : COLOR { fixed4 edgeColor = wireframe_frag(i, _WireframeBorder); if (edgeColor.x != 0) return edgeColor; float2 inputUV = i.uv0; i.normalDir = normalize(i.normalDir); half l1 = 1 - tex2D(_Occlusionmap, TRANSFORM_TEX(i.uv1.rg, _Occlusionmap)).r; { l1 *= _Strength; } return fixed4(0, 0, 0, l1); } 
ENDCG
} } FallBack "Diffuse" CustomEditor "GridcellShaderGUI" } 