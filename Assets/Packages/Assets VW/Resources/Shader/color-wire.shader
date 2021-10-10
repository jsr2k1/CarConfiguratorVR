// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "VW Shaders/Color-Unity5-Wireframe" { Properties { _DiffuseColor ("Diffuse Color", Color) = (0.5,0.5,0.5,1) _WireframeBorder("Wireframe Border", Range(0, 1)) = 1.0 } SubShader { Tags { "RenderType"="Opaque" } Pass { Name "FORWARD" Tags { "LightMode"="ForwardBase" } Cull Back 
CGPROGRAM
#pragma geometry geo
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
uniform float4 _DiffuseColor; uniform half _WireframeBorder; struct VertexInput { float4 vertex : POSITION; }; VertexOutput vert (VertexInput v) { VertexOutput o = (VertexOutput)0; o.pos = UnityObjectToClipPos(v.vertex); TRANSFER_VERTEX_TO_FRAGMENT(o) return o; } float4 frag(VertexOutput i) : COLOR { fixed4 edgeColor = wireframe_frag(i, _WireframeBorder); if (edgeColor.x != 0) return edgeColor; return fixed4(_DiffuseColor.rgb, 1); } 
ENDCG
} } FallBack "Diffuse" CustomEditor "GridcellShaderGUI" } 