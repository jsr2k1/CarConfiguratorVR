// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "VW Shaders/Carpaint-Unity5-Wireframe" { Properties { _ClearcoatIntensity ("Clearcoat Intensity", Float ) = 1 _ClearFresnel ("Clearcoat Fresnel", Float ) = 6 _MinClearCoat ("Clearcoat Minimum", Float ) = 0.25 _ReflectionFalloff ("Reflection falloff", Float ) = 0.05 _DiffuseColor ("Diffuse Color", Color) = (0.5,0.5,0.5,1) _ReflectiveColor ("Reflective Color", Color) = (0.5,0.5,0.5,1) _Glossiness ("Glossiness", Float ) = 1 _PaintFresnel ("Paint Fresnel", Float ) = 0 _FlakeColor ("Flake Color", Color) = (0.2,0.2,0.2,1) _FlakesTex ("FlakesTex", 2D) = "flakes" {} _FlakeIntensity ("Flake Intensity", Float ) = 0.5 _FlakeDensity ("Flake Density", Float ) = 1 _FlakeViewDistance ("Flake View Distance", Float ) = 3 _FlakeScale ("Flake Scale", Float ) = 4 _FlakeBlendSharpness ("Flake Blend Sharpness", Float ) = 1 _DiffuseFactor ("Diffuse Color Factor", Float) = 0.5 _SpecularFactor ("Specular Color Factor", Float) = 0.5 _Occlusionmap ("Occlusionmap", 2D) = "white" {} _WireframeBorder("Wireframe Border", Range(0, 1)) = 1.0 } SubShader { Tags { "RenderType"="Opaque" } Pass { Name "FORWARD" Tags { "LightMode"="ForwardBase" } Cull Back 
CGPROGRAM
#pragma geometry geo
#pragma vertex vert
#pragma fragment frag
#define UNITY_PASS_FORWARDBASE
#define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
#define _GLOSSYENV 1
#define GRIDCELL_PARAM1_VPOS
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
#pragma shader_feature SPECULAR_ON
#pragma shader_feature FLAKES_ON
#pragma multi_compile OCCLUSIONMAP_OFF OCCLUSIONMAP_ON
#pragma shader_feature WIREFRAME_ON
#pragma exclude_renderers xbox360 ps3
#pragma target 3.0
uniform half4 _DiffuseColor; uniform half4 _ReflectiveColor; uniform half _ReflectionFalloff; uniform half _PaintFresnel; uniform half _ClearFresnel; uniform half _MinClearCoat; uniform half _FlakeIntensity; uniform half _FlakeDensity; uniform half _FlakeViewDistance; uniform half _DiffuseFactor; uniform half _SpecularFactor; uniform half4 _FlakeColor; uniform sampler2D _FlakesTex; uniform float4 _FlakesTex_ST; uniform half _FlakeScale; uniform half _FlakeBlendSharpness; uniform half _ClearcoatIntensity; uniform half _Glossiness; uniform sampler2D _Occlusionmap; uniform float4 _Occlusionmap_ST; uniform half _WireframeBorder; struct VertexInput { float4 vertex : POSITION; float3 normal : NORMAL; float4 tangent : TANGENT; float2 texcoord0 : TEXCOORD0; float2 texcoord1 : TEXCOORD1; float2 texcoord2 : TEXCOORD2; }; VertexOutput vert (VertexInput v) { VertexOutput o = (VertexOutput)0; o.uv0 = v.texcoord0; o.uv1 = v.texcoord1; 
#ifdef LIGHTMAP_ON
o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw; o.ambientOrLightmapUV.zw = 0; 
#elif UNITY_SHOULD_SAMPLE_SH
#endif
#ifdef DYNAMICLIGHTMAP_ON
o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw; 
#endif
o.normalDir = UnityObjectToWorldNormal(v.normal); o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz ); o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w); o.posWorld = mul(unity_ObjectToWorld, v.vertex); o.pos = UnityObjectToClipPos(v.vertex); o.vpos = v.vertex; TRANSFER_VERTEX_TO_FRAGMENT(o) return o; } float4 frag(VertexOutput i) : COLOR { fixed4 edgeColor = wireframe_frag(i, _WireframeBorder); if (edgeColor.x != 0) return edgeColor; float2 inputUV = i.uv0; i.normalDir = normalize(i.normalDir); float3 l7 = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz); float3 k5 = i.normalDir; float3 i8 = reflect( -l7, k5 ); float3 k1 = normalize(_WorldSpaceLightPos0.xyz); float3 k6 = _LightColor0.rgb; float3 l6 = normalize(l7+k1); half l8 = LIGHT_ATTENUATION(i); half3 l9 = l8 * _LightColor0.xyz; UnityLight light; 
#ifdef LIGHTMAP_OFF
light.color = k6; light.dir = k1; light.ndotl = LambertTerm (k5, light.dir); 
#else
light.color = half3(0.f, 0.f, 0.f); light.ndotl = 0.0f; light.dir = half3(0.f, 0.f, 0.f); 
#endif
UnityGIInput d; d.light = light; d.worldPos = i.posWorld.xyz; d.worldViewDir = l7; d.atten = l8; 
#if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
d.ambient = 0; d.lightmapUV = i.ambientOrLightmapUV; 
#else
d.ambient = i.ambientOrLightmapUV; 
#endif
d.boxMax[0] = unity_SpecCube0_BoxMax; d.boxMin[0] = unity_SpecCube0_BoxMin; d.probePosition[0] = unity_SpecCube0_ProbePosition; d.probeHDR[0] = unity_SpecCube0_HDR; d.boxMax[1] = unity_SpecCube1_BoxMax; d.boxMin[1] = unity_SpecCube1_BoxMin; d.probePosition[1] = unity_SpecCube1_ProbePosition; d.probeHDR[1] = unity_SpecCube1_HDR; half l1; 
#ifdef OCCLUSIONMAP_ON
l1 = tex2D(_Occlusionmap, TRANSFORM_TEX(i.uv1.rg, _Occlusionmap)).r; 
#else
l1 = 1; 
#endif
UnityGI gi = UnityGlobalIllumination(d, l1, 1, i8, true); 
#ifdef SPECULAR_ON
UnityGI giRough = UnityGlobalIllumination(d, l1, 0.75, i8, true); 
#endif
k1 = gi.light.dir; k6 = gi.light.color; half3 i1 = 0; 
#ifdef FLAKES_ON
half k4 = length(_WorldSpaceCameraPos.xyz - i.posWorld.xyz); half2 yUV = i.vpos.xz * _FlakeScale; half2 xUV = i.vpos.zy * _FlakeScale; half2 zUV = i.vpos.xy * _FlakeScale; half3 yDiff = UnpackNormal(tex2D (_FlakesTex, TRANSFORM_TEX(yUV, _FlakesTex))); half3 xDiff = UnpackNormal(tex2D (_FlakesTex, TRANSFORM_TEX(xUV, _FlakesTex))); half3 zDiff = UnpackNormal(tex2D (_FlakesTex, TRANSFORM_TEX(zUV, _FlakesTex))); half3 blendWeights = pow (abs(k5), _FlakeBlendSharpness); blendWeights = blendWeights / (blendWeights.x + blendWeights.y + blendWeights.z); half3 k8 = (xDiff * blendWeights.x) + (yDiff * blendWeights.y) + (zDiff * blendWeights.z); 
#endif
half l5 = dot(l7, k5); half3 l3 = light.ndotl * l9; l3 *= l1; l3 += gi.indirect.diffuse; half3 i3 = 0; 
#ifdef SPECULAR_ON
half k3 = exp2(_Glossiness * 10.0 + 1.0); i3 = pow(max(0,dot(l6, k5)), k3) * l9; i3 *= l1; half3 i7 = giRough.indirect.specular; 
#endif
half3 l2 = gi.indirect.specular; half l4 = (((1 - _MinClearCoat) * pow(1.0 - l5, _ClearFresnel) + _MinClearCoat)) * _ClearcoatIntensity; if (l5 < _ReflectionFalloff) { l4 *= l5 / _ReflectionFalloff; } half3 i5 = pow(l5, _PaintFresnel); 
#ifdef FLAKES_ON
half k7 = length(k8.rg); half k9 = saturate((1 - (min(_FlakeViewDistance, k4) / _FlakeViewDistance)) * _FlakeIntensity); 
#endif
i1 = l2 * l4; half i2 = max(0, 1 - l4); 
#ifdef SPECULAR_ON
i3 += i7; i3 *= _SpecularFactor; 
#endif
#ifdef FLAKES_ON
half3 o2 = i3 * k9 * i2; half3 o1 = saturate(k8.r * _FlakeDensity); i1 += (_FlakeColor * o1) * o2; 
#endif
#ifdef SPECULAR_ON
half3 i4 = i3 * i5 * i2; i1 += _ReflectiveColor * i4; 
#endif
half i6 = i5 * i2; i1 += _DiffuseColor * l3 * i6 * _DiffuseFactor; i1 = saturate(i1); return fixed4(i1, 1); } 
ENDCG
} Pass { Name "FORWARD_DELTA" Tags{ "LightMode" = "ForwardAdd" } Blend One One Fog{ Color(0,0,0,0) } ZWrite Off ZTest LEqual 
CGPROGRAM
#pragma target 3.0
#pragma exclude_renderers gles
#pragma vertex vert
#pragma fragment frag
#define UNITY_PASS_FORWARDADD
#define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
#define _GLOSSYENV 1
#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "Lighting.cginc"
#include "UnityPBSLighting.cginc"
#include "UnityStandardBRDF.cginc"
#pragma multi_compile_fwdadd_fullshadows
#pragma multi_compile_fog
#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
#pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
#pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
#pragma shader_feature SPECULAR_ON
#pragma shader_feature FLAKES_ON
#pragma multi_compile OCCLUSIONMAP_OFF OCCLUSIONMAP_ON
#pragma exclude_renderers xbox360 ps3
#pragma target 3.0
uniform half4 _DiffuseColor; uniform half4 _ReflectiveColor; uniform half _ReflectionFalloff; uniform half _PaintFresnel; uniform half _ClearFresnel; uniform half _MinClearCoat; uniform half _FlakeIntensity; uniform half _FlakeDensity; uniform half _FlakeViewDistance; uniform half _DiffuseFactor; uniform half _SpecularFactor; uniform half4 _FlakeColor; uniform sampler2D _FlakesTex; uniform float4 _FlakesTex_ST; uniform half _FlakeScale; uniform half _FlakeBlendSharpness; uniform half _ClearcoatIntensity; uniform half _Glossiness; uniform half _MicroflakePerturbation; uniform half _NormalPerturbation; uniform half _MicroflakeSpecular; uniform sampler2D _Occlusionmap; uniform float4 _Occlusionmap_ST; struct VertexInput { float4 vertex : POSITION; float3 normal : NORMAL; float4 tangent : TANGENT; float2 texcoord0 : TEXCOORD0; float2 texcoord1 : TEXCOORD1; float2 texcoord2 : TEXCOORD2; }; struct VertexOutput { float4 pos : SV_POSITION; float2 uv0 : TEXCOORD0; float2 uv1 : TEXCOORD1; float4 vpos : TEXCOORD2; float4 posWorld : TEXCOORD3; float3 normalDir : TEXCOORD4; float3 tangentDir : TEXCOORD5; float3 bitangentDir : TEXCOORD6; LIGHTING_COORDS(7,8) 
#if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
float4 ambientOrLightmapUV : TEXCOORD9; 
#endif
}; VertexOutput vert(VertexInput v) { VertexOutput o = (VertexOutput)0; o.uv0 = v.texcoord0; o.uv1 = v.texcoord1; 
#ifdef LIGHTMAP_ON
o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw; o.ambientOrLightmapUV.zw = 0; 
#elif UNITY_SHOULD_SAMPLE_SH
#endif
#ifdef DYNAMICLIGHTMAP_ON
o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw; 
#endif
o.normalDir = UnityObjectToWorldNormal(v.normal); o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz); o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w); o.posWorld = mul(unity_ObjectToWorld, v.vertex); o.pos = UnityObjectToClipPos(v.vertex); TRANSFER_VERTEX_TO_FRAGMENT(o) return o; } float4 frag(VertexOutput i) : COLOR { float2 inputUV = i.uv0; i.normalDir = normalize(i.normalDir); float3 l7 = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz); float3 k5 = i.normalDir; float3 i8 = reflect(-l7, k5); float3 k1 = normalize(_WorldSpaceLightPos0.xyz); float3 k6 = _LightColor0.rgb; float3 l6 = normalize(l7 + k1); half l8 = LIGHT_ATTENUATION(i); half3 l9 = l8 * _LightColor0.xyz; UnityLight light; 
#ifdef LIGHTMAP_OFF
light.color = k6; light.dir = k1; light.ndotl = LambertTerm(k5, light.dir); 
#else
light.color = half3(0.f, 0.f, 0.f); light.ndotl = 0.0f; light.dir = half3(0.f, 0.f, 0.f); 
#endif
UnityGIInput d; d.light = light; d.worldPos = i.posWorld.xyz; d.worldViewDir = l7; d.atten = l8; 
#if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
d.ambient = 0; d.lightmapUV = i.ambientOrLightmapUV; 
#else
d.ambient = i.ambientOrLightmapUV; 
#endif
d.boxMax[0] = unity_SpecCube0_BoxMax; d.boxMin[0] = unity_SpecCube0_BoxMin; d.probePosition[0] = unity_SpecCube0_ProbePosition; d.probeHDR[0] = unity_SpecCube0_HDR; d.boxMax[1] = unity_SpecCube1_BoxMax; d.boxMin[1] = unity_SpecCube1_BoxMin; d.probePosition[1] = unity_SpecCube1_ProbePosition; d.probeHDR[1] = unity_SpecCube1_HDR; half l1; 
#ifdef OCCLUSIONMAP_ON
l1 = tex2D(_Occlusionmap, TRANSFORM_TEX(i.uv1.rg, _Occlusionmap)).r; 
#else
l1 = 1; 
#endif
UnityGI gi = UnityGlobalIllumination(d, l1, 1, i8, true); 
#ifdef SPECULAR_ON
UnityGI giRough = UnityGlobalIllumination(d, l1, 0.75, i8, true); 
#endif
k1 = gi.light.dir; k6 = gi.light.color; half3 i1 = 0; 
#ifdef FLAKES_ON
half k4 = length(_WorldSpaceCameraPos.xyz - i.posWorld.xyz); half2 yUV = i.vpos.xz * _FlakeScale; half2 xUV = i.vpos.zy * _FlakeScale; half2 zUV = i.vpos.xy * _FlakeScale; half3 yDiff = UnpackNormal(tex2D(_FlakesTex, TRANSFORM_TEX(yUV, _FlakesTex))); half3 xDiff = UnpackNormal(tex2D(_FlakesTex, TRANSFORM_TEX(xUV, _FlakesTex))); half3 zDiff = UnpackNormal(tex2D(_FlakesTex, TRANSFORM_TEX(zUV, _FlakesTex))); half3 blendWeights = pow(abs(k5), _FlakeBlendSharpness); blendWeights = blendWeights / (blendWeights.x + blendWeights.y + blendWeights.z); half3 k8 = (xDiff * blendWeights.x) + (yDiff * blendWeights.y) + (zDiff * blendWeights.z); 
#endif
half l5 = dot(l7, k5); half3 l3 = light.ndotl * l9; l3 *= l1; l3 += gi.indirect.diffuse; half3 i3 = 0; 
#ifdef SPECULAR_ON
half k3 = exp2(_Glossiness * 10.0 + 1.0); i3 = pow(max(0,dot(l6, k5)), k3) * l9; i3 *= l1; half3 i7 = giRough.indirect.specular; 
#endif
half3 l2 = gi.indirect.specular; half l4 = (((1 - _MinClearCoat) * pow(1.0 - l5, _ClearFresnel) + _MinClearCoat)) * _ClearcoatIntensity; if (l5 < _ReflectionFalloff) { l4 *= l5 / _ReflectionFalloff; } half3 i5 = pow(l5, _PaintFresnel); 
#ifdef FLAKES_ON
half k7 = length(k8.rg); half k9 = saturate((1 - (min(_FlakeViewDistance, k4) / _FlakeViewDistance)) * _FlakeIntensity); 
#endif
half i2 = max(0, 1 - l4); 
#ifdef SPECULAR_ON
i3 += i7; i3 *= _SpecularFactor; 
#endif
#ifdef FLAKES_ON
half3 o2 = i3 * k9 * i2; half3 o1 = saturate(k8.r * _FlakeDensity); i1 += (_FlakeColor * o1) * o2 * l8; 
#endif
#ifdef SPECULAR_ON
half3 i4 = i3 * i5 * i2; i1 += _ReflectiveColor * i4 * l8; 
#endif
half i6 = i5 * i2; i1 += _DiffuseColor * l3 * i6 * _DiffuseFactor; if (dot(k5, k1) < 0.0) { i1 = 0; } i1 = saturate(i1); return fixed4(i1, 1); } 
ENDCG
} } FallBack "Diffuse" CustomEditor "GridcellShaderGUI" } 