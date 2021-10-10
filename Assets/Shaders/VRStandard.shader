Shader "VRStandard" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _GlossMapScale("Smoothness Scale", Range(0.0, 1.0)) = 1.0
        [NoScaleOffset] _MetallicGlossMap("Metallic", 2D) = "white" {}
        _BumpScale("Scale", Float) = 1.0
        [NoScaleOffset] _BumpMap("Normal Map", 2D) = "bump" {}
        _OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
        [NoScaleOffset] _OcclusionMap("Occlusion", 2D) = "white" {}
        [Toggle(_CENTROIDNORMAL)] _CentroidNormal ("Enable Vertex Normal Centroid Sampling Fixup", Float) = 0
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
     
     
    // ------------------------------------------------------------
    // Surface shader code generated out of a CGPROGRAM block:
 
 
    // ---- forward rendering base pass:
    Pass {
        Name "FORWARD"
        Tags { "LightMode" = "ForwardBase" }
 
CGPROGRAM
// compile directives
#pragma vertex vert_surf
#pragma fragment frag_surf
#pragma target 5.0
#pragma multi_compile_instancing
#pragma shader_feature _CENTROIDNORMAL
#pragma multi_compile_fog
#pragma multi_compile_fwdbase
#include "HLSLSupport.cginc"
#include "UnityShaderVariables.cginc"
#include "UnityShaderUtilities.cginc"
// -------- variant for: <when no other keywords are defined>
#if !defined(INSTANCING_ON)
// Surface shader code generated based on:
// vertex modifier: 'vert'
// writes to per-pixel normal: YES
// writes to emission: no
// writes to occlusion: YES
// needs world space reflection vector: no
// needs world space normal vector: YES
// needs screen space position: no
// needs world space position: no
// needs view direction: no
// needs world space view direction: no
// needs world space position for lighting: YES
// needs world space view direction for lighting: YES
// needs world space view direction for lightmaps: no
// needs vertex color: no
// needs VFACE: no
// passes tangent-to-world matrix to pixel shader: YES
// reads from normal: no
// 1 texcoords actually used
//   float2 _MainTex
#define UNITY_PASS_FORWARDBASE
#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "UnityPBSLighting.cginc"
#include "AutoLight.cginc"
 
#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
 
#ifdef DUMMY_PREPROCESSOR_TO_WORK_AROUND_HLSL_COMPILER_LINE_HANDLING
#endif
/* UNITY: Original start of shader */
        // Physically based Standard lighting model, and enable shadows on all light types
        //#pragma surface surf Standard fullforwardshadows vertex:vert
 
        // Use shader model 3.0 target, to get nicer looking lighting
        //#pragma target 5.0
 
        #include "UnityStandardUtils.cginc"
 
        sampler2D _MainTex;
        sampler2D _MetallicGlossMap;
        sampler2D _BumpMap;
        sampler2D _OcclusionMap;
 
        struct Input {
            float2 uv_MainTex;
            float3 worldNormal; INTERNAL_DATA
        };
 
        fixed4 _Color;
        half _GlossMapScale;
        half _BumpScale;
        half _OcclusionStrength;
 
        void surf (Input IN, inout SurfaceOutputStandard o) {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
 
            fixed4 mg = tex2D(_MetallicGlossMap, IN.uv_MainTex);
            o.Metallic = mg.r;
            o.Smoothness = mg.a * _GlossMapScale;
 
            float3 vNormalWsDdx = ddx_fine( IN.worldNormal.xyz );
            float3 vNormalWsDdy = ddy_fine( IN.worldNormal.xyz );
            float flGeometricRoughnessFactor = pow( saturate( max( dot( vNormalWsDdx.xyz, vNormalWsDdx.xyz ), dot( vNormalWsDdy.xyz, vNormalWsDdy.xyz ) ) ), 0.333 );
            o.Smoothness = min( o.Smoothness, 1.0 - flGeometricRoughnessFactor ); // Ensure we don’t double-count roughness if normal map encodes geometric roughness
 
            half occ = tex2D(_OcclusionMap, IN.uv_MainTex).g;
            o.Occlusion = LerpOneTo (occ, _OcclusionStrength);
 
            half3 normal = UnpackScaleNormal(tex2D(_BumpMap, IN.uv_MainTex), _BumpScale);
            o.Normal = normal;
        }
     
 
// vertex-to-fragment interpolation data
// no lightmaps:
#ifndef LIGHTMAP_ON
struct v2f_surf {
    float4 pos : SV_POSITION;
    float2 pack0 : TEXCOORD0; // _MainTex
    float4 tSpace0 : TEXCOORD1;
    float4 tSpace1 : TEXCOORD2;
    float4 tSpace2 : TEXCOORD3;
    #if defined(_CENTROIDNORMAL)
    float3 centroidNormal : TEXCOORD4_centroid;
    #endif
    #if UNITY_SHOULD_SAMPLE_SH
    half3 sh : TEXCOORD5; // SH
    #endif
    UNITY_SHADOW_COORDS(6)
    UNITY_FOG_COORDS(7)
    #if SHADER_TARGET >= 30
    float4 lmap : TEXCOORD8;
    #endif
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
#endif
// with lightmaps:
#ifdef LIGHTMAP_ON
struct v2f_surf {
    float4 pos : SV_POSITION;
    float2 pack0 : TEXCOORD0; // _MainTex
    float4 tSpace0 : TEXCOORD1;
    float4 tSpace1 : TEXCOORD2;
    float4 tSpace2 : TEXCOORD3;
    #if defined(_CENTROIDNORMAL)
    float3 centroidNormal : TEXCOORD4_centroid;
    #endif
    float4 lmap : TEXCOORD5;
    UNITY_SHADOW_COORDS(6)
    UNITY_FOG_COORDS(7)
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
#endif
float4 _MainTex_ST;
 
// vertex shader
v2f_surf vert_surf (appdata_full v) {
    UNITY_SETUP_INSTANCE_ID(v);
    v2f_surf o;
    UNITY_INITIALIZE_OUTPUT(v2f_surf,o);
    UNITY_TRANSFER_INSTANCE_ID(v,o);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    o.pos = UnityObjectToClipPos(v.vertex);
    o.pack0.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
    float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
    float3 worldNormal = UnityObjectToWorldNormal(v.normal);
    float3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
    float tangentSign = v.tangent.w * unity_WorldTransformParams.w;
    float3 worldBinormal = cross(worldNormal, worldTangent) * tangentSign;
    o.tSpace0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
    o.tSpace1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
    o.tSpace2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);
    #if defined(_CENTROIDNORMAL)
    o.centroidNormal = worldNormal;
    #endif
    #ifdef DYNAMICLIGHTMAP_ON
    o.lmap.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
    #endif
    #ifdef LIGHTMAP_ON
    o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
    #endif
 
    // SH/ambient and vertex lights
    #ifndef LIGHTMAP_ON
        #if UNITY_SHOULD_SAMPLE_SH
            o.sh = 0;
            // Approximated illumination from non-important point lights
            #ifdef VERTEXLIGHT_ON
                o.sh += Shade4PointLights (
                    unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
                    unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
                    unity_4LightAtten0, worldPos, worldNormal);
            #endif
            o.sh = ShadeSHPerVertex (worldNormal, o.sh);
        #endif
    #endif // !LIGHTMAP_ON
 
    UNITY_TRANSFER_SHADOW(o,v.texcoord1.xy); // pass shadow coordinates to pixel shader
    UNITY_TRANSFER_FOG(o,o.pos); // pass fog coordinates to pixel shader
    return o;
}
 
// fragment shader
fixed4 frag_surf (v2f_surf IN) : SV_Target {
    UNITY_SETUP_INSTANCE_ID(IN);
    // prepare and unpack data
    Input surfIN;
    UNITY_INITIALIZE_OUTPUT(Input,surfIN);
    surfIN.uv_MainTex.x = 1.0;
    surfIN.uv_MainTex = IN.pack0.xy;
    float3 worldPos = float3(IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w);
    #ifndef USING_DIRECTIONAL_LIGHT
        fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
    #else
        fixed3 lightDir = _WorldSpaceLightPos0.xyz;
    #endif
    fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
    surfIN.worldNormal = float3(IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z);
    surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
    surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
    surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
    #ifdef UNITY_COMPILER_HLSL
    SurfaceOutputStandard o = (SurfaceOutputStandard)0;
    #else
    SurfaceOutputStandard o;
    #endif
    o.Albedo = 0.0;
    o.Emission = 0.0;
    o.Alpha = 0.0;
    o.Occlusion = 1.0;
    fixed3 normalWorldVertex = fixed3(0,0,1);
 
    #if defined(_CENTROIDNORMAL)
    if ( dot( surfIN.worldNormal, surfIN.worldNormal ) >= 1.01 )
    {
        IN.tSpace0.z = IN.centroidNormal.x;
        IN.tSpace1.z = IN.centroidNormal.y;
        IN.tSpace2.z = IN.centroidNormal.z;
    }
    #endif
 
    // call surface function
    surf (surfIN, o);
 
    // compute lighting & shadowing factor
    UNITY_LIGHT_ATTENUATION(atten, IN, worldPos)
    fixed4 c = 0;
    fixed3 worldN;
    worldN.x = dot(IN.tSpace0.xyz, o.Normal);
    worldN.y = dot(IN.tSpace1.xyz, o.Normal);
    worldN.z = dot(IN.tSpace2.xyz, o.Normal);
    o.Normal = normalize(worldN);
 
    // Setup lighting environment
    UnityGI gi;
    UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
    gi.indirect.diffuse = 0;
    gi.indirect.specular = 0;
    gi.light.color = _LightColor0.rgb;
    gi.light.dir = lightDir;
    // Call GI (lightmaps/SH/reflections) lighting function
    UnityGIInput giInput;
    UNITY_INITIALIZE_OUTPUT(UnityGIInput, giInput);
    giInput.light = gi.light;
    giInput.worldPos = worldPos;
    giInput.worldViewDir = worldViewDir;
    giInput.atten = atten;
    #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
        giInput.lightmapUV = IN.lmap;
    #else
        giInput.lightmapUV = 0.0;
    #endif
    #if UNITY_SHOULD_SAMPLE_SH
        giInput.ambient = IN.sh;
    #else
        giInput.ambient.rgb = 0.0;
    #endif
    giInput.probeHDR[0] = unity_SpecCube0_HDR;
    giInput.probeHDR[1] = unity_SpecCube1_HDR;
    #if defined(UNITY_SPECCUBE_BLENDING) || defined(UNITY_SPECCUBE_BOX_PROJECTION)
        giInput.boxMin[0] = unity_SpecCube0_BoxMin; // .w holds lerp value for blending
    #endif
    #ifdef UNITY_SPECCUBE_BOX_PROJECTION
        giInput.boxMax[0] = unity_SpecCube0_BoxMax;
        giInput.probePosition[0] = unity_SpecCube0_ProbePosition;
        giInput.boxMax[1] = unity_SpecCube1_BoxMax;
        giInput.boxMin[1] = unity_SpecCube1_BoxMin;
        giInput.probePosition[1] = unity_SpecCube1_ProbePosition;
    #endif
    LightingStandard_GI(o, giInput, gi);
 
    // realtime lighting: call lighting function
    c += LightingStandard (o, worldViewDir, gi);
    UNITY_APPLY_FOG(IN.fogCoord, c); // apply fog
    UNITY_OPAQUE_ALPHA(c.a);
    return c;
}
 
 
#endif
 
// -------- variant for: INSTANCING_ON
#if defined(INSTANCING_ON)
// Surface shader code generated based on:
// vertex modifier: 'vert'
// writes to per-pixel normal: YES
// writes to emission: no
// writes to occlusion: YES
// needs world space reflection vector: no
// needs world space normal vector: YES
// needs screen space position: no
// needs world space position: no
// needs view direction: no
// needs world space view direction: no
// needs world space position for lighting: YES
// needs world space view direction for lighting: YES
// needs world space view direction for lightmaps: no
// needs vertex color: no
// needs VFACE: no
// passes tangent-to-world matrix to pixel shader: YES
// reads from normal: no
// 1 texcoords actually used
//   float2 _MainTex
#define UNITY_PASS_FORWARDBASE
#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "UnityPBSLighting.cginc"
#include "AutoLight.cginc"
 
#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
 
#ifdef DUMMY_PREPROCESSOR_TO_WORK_AROUND_HLSL_COMPILER_LINE_HANDLING
#endif
/* UNITY: Original start of shader */
        // Physically based Standard lighting model, and enable shadows on all light types
        //#pragma surface surf Standard fullforwardshadows vertex:vert
 
        // Use shader model 3.0 target, to get nicer looking lighting
        //#pragma target 5.0
 
        #include "UnityStandardUtils.cginc"
 
        sampler2D _MainTex;
        sampler2D _MetallicGlossMap;
        sampler2D _BumpMap;
        sampler2D _OcclusionMap;
 
        struct Input {
            float2 uv_MainTex;
            float3 worldNormal; INTERNAL_DATA
        };
 
        fixed4 _Color;
        half _GlossMapScale;
        half _BumpScale;
        half _OcclusionStrength;
 
        void surf (Input IN, inout SurfaceOutputStandard o) {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
 
            fixed4 mg = tex2D(_MetallicGlossMap, IN.uv_MainTex);
            o.Metallic = mg.r;
            o.Smoothness = mg.a * _GlossMapScale;
 
            float3 vNormalWsDdx = ddx_fine( IN.worldNormal.xyz );
            float3 vNormalWsDdy = ddy_fine( IN.worldNormal.xyz );
            float flGeometricRoughnessFactor = pow( saturate( max( dot( vNormalWsDdx.xyz, vNormalWsDdx.xyz ), dot( vNormalWsDdy.xyz, vNormalWsDdy.xyz ) ) ), 0.333 );
            o.Smoothness = min( o.Smoothness, 1.0 - flGeometricRoughnessFactor ); // Ensure we don’t double-count roughness if normal map encodes geometric roughness
 
            half occ = tex2D(_OcclusionMap, IN.uv_MainTex).g;
            o.Occlusion = LerpOneTo (occ, _OcclusionStrength);
 
            half3 normal = UnpackScaleNormal(tex2D(_BumpMap, IN.uv_MainTex), _BumpScale);
            o.Normal = normal;
        }
     
 
// vertex-to-fragment interpolation data
// no lightmaps:
#ifndef LIGHTMAP_ON
struct v2f_surf {
    float4 pos : SV_POSITION;
    float2 pack0 : TEXCOORD0; // _MainTex
    float4 tSpace0 : TEXCOORD1;
    float4 tSpace1 : TEXCOORD2;
    float4 tSpace2 : TEXCOORD3;
    #if defined(_CENTROIDNORMAL)
    float3 centroidNormal : TEXCOORD4_centroid;
    #endif
    #if UNITY_SHOULD_SAMPLE_SH
    half3 sh : TEXCOORD5; // SH
    #endif
    UNITY_SHADOW_COORDS(6)
    UNITY_FOG_COORDS(7)
    #if SHADER_TARGET >= 30
    float4 lmap : TEXCOORD8;
    #endif
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
#endif
// with lightmaps:
#ifdef LIGHTMAP_ON
struct v2f_surf {
    float4 pos : SV_POSITION;
    float2 pack0 : TEXCOORD0; // _MainTex
    float4 tSpace0 : TEXCOORD1;
    float4 tSpace1 : TEXCOORD2;
    float4 tSpace2 : TEXCOORD3;
    #if defined(_CENTROIDNORMAL)
    float3 centroidNormal : TEXCOORD4_centroid;
    #endif
    float4 lmap : TEXCOORD5;
    UNITY_SHADOW_COORDS(6)
    UNITY_FOG_COORDS(7)
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
#endif
float4 _MainTex_ST;
 
// vertex shader
v2f_surf vert_surf (appdata_full v) {
    UNITY_SETUP_INSTANCE_ID(v);
    v2f_surf o;
    UNITY_INITIALIZE_OUTPUT(v2f_surf,o);
    UNITY_TRANSFER_INSTANCE_ID(v,o);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    o.pos = UnityObjectToClipPos(v.vertex);
    o.pack0.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
    float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
    float3 worldNormal = UnityObjectToWorldNormal(v.normal);
    float3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
    float tangentSign = v.tangent.w * unity_WorldTransformParams.w;
    float3 worldBinormal = cross(worldNormal, worldTangent) * tangentSign;
    o.tSpace0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
    o.tSpace1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
    o.tSpace2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);
    #if defined(_CENTROIDNORMAL)
    o.centroidNormal = worldNormal;
    #endif
    #ifdef DYNAMICLIGHTMAP_ON
    o.lmap.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
    #endif
    #ifdef LIGHTMAP_ON
    o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
    #endif
 
    // SH/ambient and vertex lights
    #ifndef LIGHTMAP_ON
        #if UNITY_SHOULD_SAMPLE_SH
            o.sh = 0;
            // Approximated illumination from non-important point lights
            #ifdef VERTEXLIGHT_ON
                o.sh += Shade4PointLights (
                    unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
                    unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
                    unity_4LightAtten0, worldPos, worldNormal);
            #endif
            o.sh = ShadeSHPerVertex (worldNormal, o.sh);
        #endif
    #endif // !LIGHTMAP_ON
 
    UNITY_TRANSFER_SHADOW(o,v.texcoord1.xy); // pass shadow coordinates to pixel shader
    UNITY_TRANSFER_FOG(o,o.pos); // pass fog coordinates to pixel shader
    return o;
}
 
// fragment shader
fixed4 frag_surf (v2f_surf IN) : SV_Target {
    UNITY_SETUP_INSTANCE_ID(IN);
    // prepare and unpack data
    Input surfIN;
    UNITY_INITIALIZE_OUTPUT(Input,surfIN);
    surfIN.uv_MainTex.x = 1.0;
    surfIN.uv_MainTex = IN.pack0.xy;
    float3 worldPos = float3(IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w);
    #ifndef USING_DIRECTIONAL_LIGHT
        fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
    #else
        fixed3 lightDir = _WorldSpaceLightPos0.xyz;
    #endif
    fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
    surfIN.worldNormal = float3(IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z);
    surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
    surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
    surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
    #ifdef UNITY_COMPILER_HLSL
    SurfaceOutputStandard o = (SurfaceOutputStandard)0;
    #else
    SurfaceOutputStandard o;
    #endif
    o.Albedo = 0.0;
    o.Emission = 0.0;
    o.Alpha = 0.0;
    o.Occlusion = 1.0;
    fixed3 normalWorldVertex = fixed3(0,0,1);
 
    #if defined(_CENTROIDNORMAL)
    if ( dot( surfIN.worldNormal, surfIN.worldNormal ) >= 1.01 )
    {
        IN.tSpace0.z = IN.centroidNormal.x;
        IN.tSpace1.z = IN.centroidNormal.y;
        IN.tSpace2.z = IN.centroidNormal.z;
    }
    #endif
 
    // call surface function
    surf (surfIN, o);
 
    // compute lighting & shadowing factor
    UNITY_LIGHT_ATTENUATION(atten, IN, worldPos)
    fixed4 c = 0;
    fixed3 worldN;
    worldN.x = dot(IN.tSpace0.xyz, o.Normal);
    worldN.y = dot(IN.tSpace1.xyz, o.Normal);
    worldN.z = dot(IN.tSpace2.xyz, o.Normal);
    o.Normal = normalize(worldN);
 
    // Setup lighting environment
    UnityGI gi;
    UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
    gi.indirect.diffuse = 0;
    gi.indirect.specular = 0;
    gi.light.color = _LightColor0.rgb;
    gi.light.dir = lightDir;
    // Call GI (lightmaps/SH/reflections) lighting function
    UnityGIInput giInput;
    UNITY_INITIALIZE_OUTPUT(UnityGIInput, giInput);
    giInput.light = gi.light;
    giInput.worldPos = worldPos;
    giInput.worldViewDir = worldViewDir;
    giInput.atten = atten;
    #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
        giInput.lightmapUV = IN.lmap;
    #else
        giInput.lightmapUV = 0.0;
    #endif
    #if UNITY_SHOULD_SAMPLE_SH
        giInput.ambient = IN.sh;
    #else
        giInput.ambient.rgb = 0.0;
    #endif
    giInput.probeHDR[0] = unity_SpecCube0_HDR;
    giInput.probeHDR[1] = unity_SpecCube1_HDR;
    #if defined(UNITY_SPECCUBE_BLENDING) || defined(UNITY_SPECCUBE_BOX_PROJECTION)
        giInput.boxMin[0] = unity_SpecCube0_BoxMin; // .w holds lerp value for blending
    #endif
    #ifdef UNITY_SPECCUBE_BOX_PROJECTION
        giInput.boxMax[0] = unity_SpecCube0_BoxMax;
        giInput.probePosition[0] = unity_SpecCube0_ProbePosition;
        giInput.boxMax[1] = unity_SpecCube1_BoxMax;
        giInput.boxMin[1] = unity_SpecCube1_BoxMin;
        giInput.probePosition[1] = unity_SpecCube1_ProbePosition;
    #endif
    LightingStandard_GI(o, giInput, gi);
 
    // realtime lighting: call lighting function
    c += LightingStandard (o, worldViewDir, gi);
    UNITY_APPLY_FOG(IN.fogCoord, c); // apply fog
    UNITY_OPAQUE_ALPHA(c.a);
    return c;
}
 
 
#endif
 
 
ENDCG
 
}
 
    // ---- forward rendering additive lights pass:
    Pass {
        Name "FORWARD"
        Tags { "LightMode" = "ForwardAdd" }
        ZWrite Off Blend One One
 
CGPROGRAM
// compile directives
#pragma vertex vert_surf
#pragma fragment frag_surf
#pragma target 5.0
#pragma multi_compile_instancing
#pragma shader_feature _CENTROIDNORMAL
#pragma multi_compile_fog
#pragma skip_variants INSTANCING_ON
#pragma multi_compile_fwdadd_fullshadows
#include "HLSLSupport.cginc"
#include "UnityShaderVariables.cginc"
#include "UnityShaderUtilities.cginc"
// -------- variant for: <when no other keywords are defined>
#if !defined(INSTANCING_ON)
// Surface shader code generated based on:
// vertex modifier: 'vert'
// writes to per-pixel normal: YES
// writes to emission: no
// writes to occlusion: YES
// needs world space reflection vector: no
// needs world space normal vector: YES
// needs screen space position: no
// needs world space position: no
// needs view direction: no
// needs world space view direction: no
// needs world space position for lighting: YES
// needs world space view direction for lighting: YES
// needs world space view direction for lightmaps: no
// needs vertex color: no
// needs VFACE: no
// passes tangent-to-world matrix to pixel shader: YES
// reads from normal: no
// 1 texcoords actually used
//   float2 _MainTex
#define UNITY_PASS_FORWARDADD
#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "UnityPBSLighting.cginc"
#include "AutoLight.cginc"
 
#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
 
#ifdef DUMMY_PREPROCESSOR_TO_WORK_AROUND_HLSL_COMPILER_LINE_HANDLING
#endif
/* UNITY: Original start of shader */
        // Physically based Standard lighting model, and enable shadows on all light types
        //#pragma surface surf Standard fullforwardshadows vertex:vert
 
        // Use shader model 3.0 target, to get nicer looking lighting
        //#pragma target 5.0
 
        #include "UnityStandardUtils.cginc"
 
        sampler2D _MainTex;
        sampler2D _MetallicGlossMap;
        sampler2D _BumpMap;
        sampler2D _OcclusionMap;
 
        struct Input {
            float2 uv_MainTex;
            float3 worldNormal; INTERNAL_DATA
        };
 
        fixed4 _Color;
        half _GlossMapScale;
        half _BumpScale;
        half _OcclusionStrength;
 
        void surf (Input IN, inout SurfaceOutputStandard o) {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
 
            fixed4 mg = tex2D(_MetallicGlossMap, IN.uv_MainTex);
            o.Metallic = mg.r;
            o.Smoothness = mg.a * _GlossMapScale;
 
            float3 vNormalWsDdx = ddx_fine( IN.worldNormal.xyz );
            float3 vNormalWsDdy = ddy_fine( IN.worldNormal.xyz );
            float flGeometricRoughnessFactor = pow( saturate( max( dot( vNormalWsDdx.xyz, vNormalWsDdx.xyz ), dot( vNormalWsDdy.xyz, vNormalWsDdy.xyz ) ) ), 0.333 );
            o.Smoothness = min( o.Smoothness, 1.0 - flGeometricRoughnessFactor ); // Ensure we don’t double-count roughness if normal map encodes geometric roughness
 
            half occ = tex2D(_OcclusionMap, IN.uv_MainTex).g;
            o.Occlusion = LerpOneTo (occ, _OcclusionStrength);
 
            half3 normal = UnpackScaleNormal(tex2D(_BumpMap, IN.uv_MainTex), _BumpScale);
            o.Normal = normal;
        }
     
 
// vertex-to-fragment interpolation data
struct v2f_surf {
    float4 pos : SV_POSITION;
    float2 pack0 : TEXCOORD0; // _MainTex
    fixed3 tSpace0 : TEXCOORD1;
    fixed3 tSpace1 : TEXCOORD2;
    fixed3 tSpace2 : TEXCOORD3;
    float3 worldPos : TEXCOORD4;
    float3 centroidNormal : TEXCOORD5; // centroidNormal
    UNITY_SHADOW_COORDS(6)
    UNITY_FOG_COORDS(7)
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
float4 _MainTex_ST;
 
// vertex shader
v2f_surf vert_surf (appdata_full v) {
    UNITY_SETUP_INSTANCE_ID(v);
    v2f_surf o;
    UNITY_INITIALIZE_OUTPUT(v2f_surf,o);
    UNITY_TRANSFER_INSTANCE_ID(v,o);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    o.pos = UnityObjectToClipPos(v.vertex);
    o.pack0.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
    float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
    float3 worldNormal = UnityObjectToWorldNormal(v.normal);
    float3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
    float tangentSign = v.tangent.w * unity_WorldTransformParams.w;
    float3 worldBinormal = cross(worldNormal, worldTangent) * tangentSign;
    o.tSpace0 = fixed3(worldTangent.x, worldBinormal.x, worldNormal.x);
    o.tSpace1 = fixed3(worldTangent.y, worldBinormal.y, worldNormal.y);
    o.tSpace2 = fixed3(worldTangent.z, worldBinormal.z, worldNormal.z);
    #if defined(_CENTROIDNORMAL)
    o.centroidNormal = worldNormal;
    #endif
    o.worldPos = worldPos;
 
    UNITY_TRANSFER_SHADOW(o,v.texcoord1.xy); // pass shadow coordinates to pixel shader
    UNITY_TRANSFER_FOG(o,o.pos); // pass fog coordinates to pixel shader
    return o;
}
 
// fragment shader
fixed4 frag_surf (v2f_surf IN) : SV_Target {
    UNITY_SETUP_INSTANCE_ID(IN);
    // prepare and unpack data
    Input surfIN;
    UNITY_INITIALIZE_OUTPUT(Input,surfIN);
    surfIN.uv_MainTex.x = 1.0;
    surfIN.uv_MainTex = IN.pack0.xy;
    float3 worldPos = IN.worldPos;
    #ifndef USING_DIRECTIONAL_LIGHT
        fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
    #else
        fixed3 lightDir = _WorldSpaceLightPos0.xyz;
    #endif
    fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
    surfIN.worldNormal = float3(IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z);
    surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
    surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
    surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
    #ifdef UNITY_COMPILER_HLSL
    SurfaceOutputStandard o = (SurfaceOutputStandard)0;
    #else
    SurfaceOutputStandard o;
    #endif
    o.Albedo = 0.0;
    o.Emission = 0.0;
    o.Alpha = 0.0;
    o.Occlusion = 1.0;
    fixed3 normalWorldVertex = fixed3(0,0,1);
 
    #if defined(_CENTROIDNORMAL)
    if ( dot( surfIN.worldNormal, surfIN.worldNormal ) >= 1.01 )
    {
        IN.tSpace0.z = IN.centroidNormal.x;
        IN.tSpace1.z = IN.centroidNormal.y;
        IN.tSpace2.z = IN.centroidNormal.z;
    }
    #endif
 
    // call surface function
    surf (surfIN, o);
    UNITY_LIGHT_ATTENUATION(atten, IN, worldPos)
    fixed4 c = 0;
    fixed3 worldN;
    worldN.x = dot(IN.tSpace0.xyz, o.Normal);
    worldN.y = dot(IN.tSpace1.xyz, o.Normal);
    worldN.z = dot(IN.tSpace2.xyz, o.Normal);
    o.Normal = normalize(worldN);
 
    // Setup lighting environment
    UnityGI gi;
    UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
    gi.indirect.diffuse = 0;
    gi.indirect.specular = 0;
    gi.light.color = _LightColor0.rgb;
    gi.light.dir = lightDir;
    gi.light.color *= atten;
    c += LightingStandard (o, worldViewDir, gi);
    c.a = 0.0;
    UNITY_APPLY_FOG(IN.fogCoord, c); // apply fog
    UNITY_OPAQUE_ALPHA(c.a);
    return c;
}
 
 
#endif
 
 
ENDCG
 
}
 
    // ---- deferred shading pass:
    Pass {
        Name "DEFERRED"
        Tags { "LightMode" = "Deferred" }
 
CGPROGRAM
// compile directives
#pragma vertex vert_surf
#pragma fragment frag_surf
#pragma target 5.0
#pragma multi_compile_instancing
#pragma shader_feature _CENTROIDNORMAL
#pragma exclude_renderers nomrt
#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
#pragma multi_compile_prepassfinal
#include "HLSLSupport.cginc"
#include "UnityShaderVariables.cginc"
#include "UnityShaderUtilities.cginc"
// -------- variant for: <when no other keywords are defined>
#if !defined(INSTANCING_ON)
// Surface shader code generated based on:
// vertex modifier: 'vert'
// writes to per-pixel normal: YES
// writes to emission: no
// writes to occlusion: YES
// needs world space reflection vector: no
// needs world space normal vector: YES
// needs screen space position: no
// needs world space position: no
// needs view direction: no
// needs world space view direction: no
// needs world space position for lighting: YES
// needs world space view direction for lighting: YES
// needs world space view direction for lightmaps: no
// needs vertex color: no
// needs VFACE: no
// passes tangent-to-world matrix to pixel shader: YES
// reads from normal: no
// 1 texcoords actually used
//   float2 _MainTex
#define UNITY_PASS_DEFERRED
#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "UnityPBSLighting.cginc"
 
#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
 
#ifdef DUMMY_PREPROCESSOR_TO_WORK_AROUND_HLSL_COMPILER_LINE_HANDLING
#endif
/* UNITY: Original start of shader */
        // Physically based Standard lighting model, and enable shadows on all light types
        //#pragma surface surf Standard fullforwardshadows vertex:vert
 
        // Use shader model 3.0 target, to get nicer looking lighting
        //#pragma target 5.0
 
        #include "UnityStandardUtils.cginc"
 
        sampler2D _MainTex;
        sampler2D _MetallicGlossMap;
        sampler2D _BumpMap;
        sampler2D _OcclusionMap;
 
        struct Input {
            float2 uv_MainTex;
            float3 worldNormal; INTERNAL_DATA
        };
 
        fixed4 _Color;
        half _GlossMapScale;
        half _BumpScale;
        half _OcclusionStrength;
 
        void surf (Input IN, inout SurfaceOutputStandard o) {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
 
            fixed4 mg = tex2D(_MetallicGlossMap, IN.uv_MainTex);
            o.Metallic = mg.r;
            o.Smoothness = mg.a * _GlossMapScale;
 
            float3 vNormalWsDdx = ddx_fine( IN.worldNormal.xyz );
            float3 vNormalWsDdy = ddy_fine( IN.worldNormal.xyz );
            float flGeometricRoughnessFactor = pow( saturate( max( dot( vNormalWsDdx.xyz, vNormalWsDdx.xyz ), dot( vNormalWsDdy.xyz, vNormalWsDdy.xyz ) ) ), 0.333 );
            o.Smoothness = min( o.Smoothness, 1.0 - flGeometricRoughnessFactor ); // Ensure we don’t double-count roughness if normal map encodes geometric roughness
 
            half occ = tex2D(_OcclusionMap, IN.uv_MainTex).g;
            o.Occlusion = LerpOneTo (occ, _OcclusionStrength);
 
            half3 normal = UnpackScaleNormal(tex2D(_BumpMap, IN.uv_MainTex), _BumpScale);
            o.Normal = normal;
        }
     
 
// vertex-to-fragment interpolation data
struct v2f_surf {
    float4 pos : SV_POSITION;
    float2 pack0 : TEXCOORD0; // _MainTex
    float4 tSpace0 : TEXCOORD1;
    float4 tSpace1 : TEXCOORD2;
    float4 tSpace2 : TEXCOORD3;
    #if defined(_CENTROIDNORMAL)
    float3 centroidNormal : TEXCOORD4_centroid;
    #endif
#ifndef DIRLIGHTMAP_OFF
    half3 viewDir : TEXCOORD5;
#endif
    float4 lmap : TEXCOORD6;
#ifndef LIGHTMAP_ON
    #if UNITY_SHOULD_SAMPLE_SH
        half3 sh : TEXCOORD7; // SH
    #endif
#else
    #ifdef DIRLIGHTMAP_OFF
        float4 lmapFadePos : TEXCOORD7;
    #endif
#endif
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
float4 _MainTex_ST;
 
// vertex shader
v2f_surf vert_surf (appdata_full v) {
    UNITY_SETUP_INSTANCE_ID(v);
    v2f_surf o;
    UNITY_INITIALIZE_OUTPUT(v2f_surf,o);
    UNITY_TRANSFER_INSTANCE_ID(v,o);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    o.pos = UnityObjectToClipPos(v.vertex);
    o.pack0.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
    float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
    float3 worldNormal = UnityObjectToWorldNormal(v.normal);
    float3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
    float tangentSign = v.tangent.w * unity_WorldTransformParams.w;
    float3 worldBinormal = cross(worldNormal, worldTangent) * tangentSign;
    o.tSpace0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
    o.tSpace1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
    o.tSpace2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);
    #if defined(_CENTROIDNORMAL)
    o.centroidNormal = worldNormal;
    #endif
    float3 viewDirForLight = UnityWorldSpaceViewDir(worldPos);
    #ifndef DIRLIGHTMAP_OFF
    o.viewDir.x = dot(viewDirForLight, worldTangent);
    o.viewDir.y = dot(viewDirForLight, worldBinormal);
    o.viewDir.z = dot(viewDirForLight, worldNormal);
    #endif
#ifdef DYNAMICLIGHTMAP_ON
    o.lmap.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
#else
    o.lmap.zw = 0;
#endif
#ifdef LIGHTMAP_ON
    o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
    #ifdef DIRLIGHTMAP_OFF
        o.lmapFadePos.xyz = (mul(unity_ObjectToWorld, v.vertex).xyz - unity_ShadowFadeCenterAndType.xyz) * unity_ShadowFadeCenterAndType.w;
        o.lmapFadePos.w = (-UnityObjectToViewPos(v.vertex).z) * (1.0 - unity_ShadowFadeCenterAndType.w);
    #endif
#else
    o.lmap.xy = 0;
        #if UNITY_SHOULD_SAMPLE_SH
            o.sh = 0;
            o.sh = ShadeSHPerVertex (worldNormal, o.sh);
        #endif
#endif
    return o;
}
#ifdef LIGHTMAP_ON
float4 unity_LightmapFade;
#endif
fixed4 unity_Ambient;
 
// fragment shader
void frag_surf (v2f_surf IN,
        out half4 outGBuffer0 : SV_Target0,
        out half4 outGBuffer1 : SV_Target1,
        out half4 outGBuffer2 : SV_Target2,
        out half4 outEmission : SV_Target3
#if defined(SHADOWS_SHADOWMASK) && (UNITY_ALLOWED_MRT_COUNT > 4)
        , out half4 outShadowMask : SV_Target4
#endif
) {
    UNITY_SETUP_INSTANCE_ID(IN);
    // prepare and unpack data
    Input surfIN;
    UNITY_INITIALIZE_OUTPUT(Input,surfIN);
    surfIN.uv_MainTex.x = 1.0;
    surfIN.uv_MainTex = IN.pack0.xy;
    float3 worldPos = float3(IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w);
    #ifndef USING_DIRECTIONAL_LIGHT
        fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
    #else
        fixed3 lightDir = _WorldSpaceLightPos0.xyz;
    #endif
    fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
    surfIN.worldNormal = float3(IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z);
    surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
    surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
    surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
    #ifdef UNITY_COMPILER_HLSL
    SurfaceOutputStandard o = (SurfaceOutputStandard)0;
    #else
    SurfaceOutputStandard o;
    #endif
    o.Albedo = 0.0;
    o.Emission = 0.0;
    o.Alpha = 0.0;
    o.Occlusion = 1.0;
    fixed3 normalWorldVertex = fixed3(0,0,1);
 
    // call surface function
    surf (surfIN, o);
fixed3 originalNormal = o.Normal;
    fixed3 worldN;
    worldN.x = dot(IN.tSpace0.xyz, o.Normal);
    worldN.y = dot(IN.tSpace1.xyz, o.Normal);
    worldN.z = dot(IN.tSpace2.xyz, o.Normal);
    o.Normal = normalize(worldN);
    half atten = 1;
 
    // Setup lighting environment
    UnityGI gi;
    UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
    gi.indirect.diffuse = 0;
    gi.indirect.specular = 0;
    gi.light.color = 0;
    gi.light.dir = half3(0,1,0);
    // Call GI (lightmaps/SH/reflections) lighting function
    UnityGIInput giInput;
    UNITY_INITIALIZE_OUTPUT(UnityGIInput, giInput);
    giInput.light = gi.light;
    giInput.worldPos = worldPos;
    giInput.worldViewDir = worldViewDir;
    giInput.atten = atten;
    #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
        giInput.lightmapUV = IN.lmap;
    #else
        giInput.lightmapUV = 0.0;
    #endif
    #if UNITY_SHOULD_SAMPLE_SH
        giInput.ambient = IN.sh;
    #else
        giInput.ambient.rgb = 0.0;
    #endif
    giInput.probeHDR[0] = unity_SpecCube0_HDR;
    giInput.probeHDR[1] = unity_SpecCube1_HDR;
    #if defined(UNITY_SPECCUBE_BLENDING) || defined(UNITY_SPECCUBE_BOX_PROJECTION)
        giInput.boxMin[0] = unity_SpecCube0_BoxMin; // .w holds lerp value for blending
    #endif
    #ifdef UNITY_SPECCUBE_BOX_PROJECTION
        giInput.boxMax[0] = unity_SpecCube0_BoxMax;
        giInput.probePosition[0] = unity_SpecCube0_ProbePosition;
        giInput.boxMax[1] = unity_SpecCube1_BoxMax;
        giInput.boxMin[1] = unity_SpecCube1_BoxMin;
        giInput.probePosition[1] = unity_SpecCube1_ProbePosition;
    #endif
    LightingStandard_GI(o, giInput, gi);
 
    // call lighting function to output g-buffer
    outEmission = LightingStandard_Deferred (o, worldViewDir, gi, outGBuffer0, outGBuffer1, outGBuffer2);
    #if defined(SHADOWS_SHADOWMASK) && (UNITY_ALLOWED_MRT_COUNT > 4)
        outShadowMask = UnityGetRawBakedOcclusions (IN.lmap.xy, float3(0, 0, 0));
    #endif
    #ifndef UNITY_HDR_ON
    outEmission.rgb = exp2(-outEmission.rgb);
    #endif
}
 
 
#endif
 
// -------- variant for: INSTANCING_ON
#if defined(INSTANCING_ON)
// Surface shader code generated based on:
// vertex modifier: 'vert'
// writes to per-pixel normal: YES
// writes to emission: no
// writes to occlusion: YES
// needs world space reflection vector: no
// needs world space normal vector: YES
// needs screen space position: no
// needs world space position: no
// needs view direction: no
// needs world space view direction: no
// needs world space position for lighting: YES
// needs world space view direction for lighting: YES
// needs world space view direction for lightmaps: no
// needs vertex color: no
// needs VFACE: no
// passes tangent-to-world matrix to pixel shader: YES
// reads from normal: no
// 1 texcoords actually used
//   float2 _MainTex
#define UNITY_PASS_DEFERRED
#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "UnityPBSLighting.cginc"
 
#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
 
#ifdef DUMMY_PREPROCESSOR_TO_WORK_AROUND_HLSL_COMPILER_LINE_HANDLING
#endif
/* UNITY: Original start of shader */
        // Physically based Standard lighting model, and enable shadows on all light types
        //#pragma surface surf Standard fullforwardshadows vertex:vert
 
        // Use shader model 3.0 target, to get nicer looking lighting
        //#pragma target 5.0
 
        #include "UnityStandardUtils.cginc"
 
        sampler2D _MainTex;
        sampler2D _MetallicGlossMap;
        sampler2D _BumpMap;
        sampler2D _OcclusionMap;
 
        struct Input {
            float2 uv_MainTex;
            float3 worldNormal; INTERNAL_DATA
        };
 
        fixed4 _Color;
        half _GlossMapScale;
        half _BumpScale;
        half _OcclusionStrength;
 
        void surf (Input IN, inout SurfaceOutputStandard o) {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
 
            fixed4 mg = tex2D(_MetallicGlossMap, IN.uv_MainTex);
            o.Metallic = mg.r;
            o.Smoothness = mg.a * _GlossMapScale;
 
            float3 vNormalWsDdx = ddx_fine( IN.worldNormal.xyz );
            float3 vNormalWsDdy = ddy_fine( IN.worldNormal.xyz );
            float flGeometricRoughnessFactor = pow( saturate( max( dot( vNormalWsDdx.xyz, vNormalWsDdx.xyz ), dot( vNormalWsDdy.xyz, vNormalWsDdy.xyz ) ) ), 0.333 );
            o.Smoothness = min( o.Smoothness, 1.0 - flGeometricRoughnessFactor ); // Ensure we don’t double-count roughness if normal map encodes geometric roughness
 
            half occ = tex2D(_OcclusionMap, IN.uv_MainTex).g;
            o.Occlusion = LerpOneTo (occ, _OcclusionStrength);
 
            half3 normal = UnpackScaleNormal(tex2D(_BumpMap, IN.uv_MainTex), _BumpScale);
            o.Normal = normal;
        }
     
 
// vertex-to-fragment interpolation data
struct v2f_surf {
    float4 pos : SV_POSITION;
    float2 pack0 : TEXCOORD0; // _MainTex
    float4 tSpace0 : TEXCOORD1;
    float4 tSpace1 : TEXCOORD2;
    float4 tSpace2 : TEXCOORD3;
    #if defined(_CENTROIDNORMAL)
    float3 centroidNormal : TEXCOORD4_centroid;
    #endif
#ifndef DIRLIGHTMAP_OFF
    half3 viewDir : TEXCOORD5;
#endif
    float4 lmap : TEXCOORD6;
#ifndef LIGHTMAP_ON
    #if UNITY_SHOULD_SAMPLE_SH
        half3 sh : TEXCOORD7; // SH
    #endif
#else
    #ifdef DIRLIGHTMAP_OFF
        float4 lmapFadePos : TEXCOORD7;
    #endif
#endif
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
float4 _MainTex_ST;
 
// vertex shader
v2f_surf vert_surf (appdata_full v) {
    UNITY_SETUP_INSTANCE_ID(v);
    v2f_surf o;
    UNITY_INITIALIZE_OUTPUT(v2f_surf,o);
    UNITY_TRANSFER_INSTANCE_ID(v,o);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    o.pos = UnityObjectToClipPos(v.vertex);
    o.pack0.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
    float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
    float3 worldNormal = UnityObjectToWorldNormal(v.normal);
    float3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
    float tangentSign = v.tangent.w * unity_WorldTransformParams.w;
    float3 worldBinormal = cross(worldNormal, worldTangent) * tangentSign;
    o.tSpace0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
    o.tSpace1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
    o.tSpace2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);
    #if defined(_CENTROIDNORMAL)
    o.centroidNormal = worldNormal;
    #endif
    float3 viewDirForLight = UnityWorldSpaceViewDir(worldPos);
    #ifndef DIRLIGHTMAP_OFF
    o.viewDir.x = dot(viewDirForLight, worldTangent);
    o.viewDir.y = dot(viewDirForLight, worldBinormal);
    o.viewDir.z = dot(viewDirForLight, worldNormal);
    #endif
#ifdef DYNAMICLIGHTMAP_ON
    o.lmap.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
#else
    o.lmap.zw = 0;
#endif
#ifdef LIGHTMAP_ON
    o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
    #ifdef DIRLIGHTMAP_OFF
        o.lmapFadePos.xyz = (mul(unity_ObjectToWorld, v.vertex).xyz - unity_ShadowFadeCenterAndType.xyz) * unity_ShadowFadeCenterAndType.w;
        o.lmapFadePos.w = (-UnityObjectToViewPos(v.vertex).z) * (1.0 - unity_ShadowFadeCenterAndType.w);
    #endif
#else
    o.lmap.xy = 0;
        #if UNITY_SHOULD_SAMPLE_SH
            o.sh = 0;
            o.sh = ShadeSHPerVertex (worldNormal, o.sh);
        #endif
#endif
    return o;
}
#ifdef LIGHTMAP_ON
float4 unity_LightmapFade;
#endif
fixed4 unity_Ambient;
 
// fragment shader
void frag_surf (v2f_surf IN,
        out half4 outGBuffer0 : SV_Target0,
        out half4 outGBuffer1 : SV_Target1,
        out half4 outGBuffer2 : SV_Target2,
        out half4 outEmission : SV_Target3
#if defined(SHADOWS_SHADOWMASK) && (UNITY_ALLOWED_MRT_COUNT > 4)
        , out half4 outShadowMask : SV_Target4
#endif
) {
    UNITY_SETUP_INSTANCE_ID(IN);
    // prepare and unpack data
    Input surfIN;
    UNITY_INITIALIZE_OUTPUT(Input,surfIN);
    surfIN.uv_MainTex.x = 1.0;
    surfIN.uv_MainTex = IN.pack0.xy;
    float3 worldPos = float3(IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w);
    #ifndef USING_DIRECTIONAL_LIGHT
        fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
    #else
        fixed3 lightDir = _WorldSpaceLightPos0.xyz;
    #endif
    fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
    surfIN.worldNormal = float3(IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z);
    surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
    surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
    surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
    #ifdef UNITY_COMPILER_HLSL
    SurfaceOutputStandard o = (SurfaceOutputStandard)0;
    #else
    SurfaceOutputStandard o;
    #endif
    o.Albedo = 0.0;
    o.Emission = 0.0;
    o.Alpha = 0.0;
    o.Occlusion = 1.0;
    fixed3 normalWorldVertex = fixed3(0,0,1);
 
    // call surface function
    surf (surfIN, o);
fixed3 originalNormal = o.Normal;
    fixed3 worldN;
    worldN.x = dot(IN.tSpace0.xyz, o.Normal);
    worldN.y = dot(IN.tSpace1.xyz, o.Normal);
    worldN.z = dot(IN.tSpace2.xyz, o.Normal);
    o.Normal = normalize(worldN);
    half atten = 1;
 
    // Setup lighting environment
    UnityGI gi;
    UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
    gi.indirect.diffuse = 0;
    gi.indirect.specular = 0;
    gi.light.color = 0;
    gi.light.dir = half3(0,1,0);
    // Call GI (lightmaps/SH/reflections) lighting function
    UnityGIInput giInput;
    UNITY_INITIALIZE_OUTPUT(UnityGIInput, giInput);
    giInput.light = gi.light;
    giInput.worldPos = worldPos;
    giInput.worldViewDir = worldViewDir;
    giInput.atten = atten;
    #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
        giInput.lightmapUV = IN.lmap;
    #else
        giInput.lightmapUV = 0.0;
    #endif
    #if UNITY_SHOULD_SAMPLE_SH
        giInput.ambient = IN.sh;
    #else
        giInput.ambient.rgb = 0.0;
    #endif
    giInput.probeHDR[0] = unity_SpecCube0_HDR;
    giInput.probeHDR[1] = unity_SpecCube1_HDR;
    #if defined(UNITY_SPECCUBE_BLENDING) || defined(UNITY_SPECCUBE_BOX_PROJECTION)
        giInput.boxMin[0] = unity_SpecCube0_BoxMin; // .w holds lerp value for blending
    #endif
    #ifdef UNITY_SPECCUBE_BOX_PROJECTION
        giInput.boxMax[0] = unity_SpecCube0_BoxMax;
        giInput.probePosition[0] = unity_SpecCube0_ProbePosition;
        giInput.boxMax[1] = unity_SpecCube1_BoxMax;
        giInput.boxMin[1] = unity_SpecCube1_BoxMin;
        giInput.probePosition[1] = unity_SpecCube1_ProbePosition;
    #endif
    LightingStandard_GI(o, giInput, gi);
 
    // call lighting function to output g-buffer
    outEmission = LightingStandard_Deferred (o, worldViewDir, gi, outGBuffer0, outGBuffer1, outGBuffer2);
    #if defined(SHADOWS_SHADOWMASK) && (UNITY_ALLOWED_MRT_COUNT > 4)
        outShadowMask = UnityGetRawBakedOcclusions (IN.lmap.xy, float3(0, 0, 0));
    #endif
    #ifndef UNITY_HDR_ON
    outEmission.rgb = exp2(-outEmission.rgb);
    #endif
}
 
 
#endif
 
 
ENDCG
 
}
 
    // ---- meta information extraction pass:
    Pass {
        Name "Meta"
        Tags { "LightMode" = "Meta" }
        Cull Off
 
CGPROGRAM
// compile directives
#pragma vertex vert_surf
#pragma fragment frag_surf
#pragma target 5.0
#pragma multi_compile_instancing
#pragma shader_feature _CENTROIDNORMAL
#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
#pragma skip_variants INSTANCING_ON
#pragma shader_feature EDITOR_VISUALIZATION
 
#include "HLSLSupport.cginc"
#include "UnityShaderVariables.cginc"
#include "UnityShaderUtilities.cginc"
// -------- variant for: <when no other keywords are defined>
#if !defined(INSTANCING_ON)
// Surface shader code generated based on:
// vertex modifier: 'vert'
// writes to per-pixel normal: YES
// writes to emission: no
// writes to occlusion: YES
// needs world space reflection vector: no
// needs world space normal vector: no
// needs screen space position: no
// needs world space position: no
// needs view direction: no
// needs world space view direction: no
// needs world space position for lighting: YES
// needs world space view direction for lighting: YES
// needs world space view direction for lightmaps: no
// needs vertex color: no
// needs VFACE: no
// passes tangent-to-world matrix to pixel shader: YES
// reads from normal: no
// 1 texcoords actually used
//   float2 _MainTex
#define UNITY_PASS_META
#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "UnityPBSLighting.cginc"
 
#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
 
#ifdef DUMMY_PREPROCESSOR_TO_WORK_AROUND_HLSL_COMPILER_LINE_HANDLING
#endif
/* UNITY: Original start of shader */
        // Physically based Standard lighting model, and enable shadows on all light types
        //#pragma surface surf Standard fullforwardshadows vertex:vert
 
        // Use shader model 3.0 target, to get nicer looking lighting
        //#pragma target 5.0
 
        #include "UnityStandardUtils.cginc"
 
        sampler2D _MainTex;
        sampler2D _MetallicGlossMap;
        sampler2D _BumpMap;
        sampler2D _OcclusionMap;
 
        struct Input {
            float2 uv_MainTex;
            float3 worldNormal; INTERNAL_DATA
        };
 
        fixed4 _Color;
        half _GlossMapScale;
        half _BumpScale;
        half _OcclusionStrength;
 
        void surf (Input IN, inout SurfaceOutputStandard o) {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
 
            fixed4 mg = tex2D(_MetallicGlossMap, IN.uv_MainTex);
            o.Metallic = mg.r;
            o.Smoothness = mg.a * _GlossMapScale;
 
            float3 vNormalWsDdx = ddx_fine( IN.worldNormal.xyz );
            float3 vNormalWsDdy = ddy_fine( IN.worldNormal.xyz );
            float flGeometricRoughnessFactor = pow( saturate( max( dot( vNormalWsDdx.xyz, vNormalWsDdx.xyz ), dot( vNormalWsDdy.xyz, vNormalWsDdy.xyz ) ) ), 0.333 );
            o.Smoothness = min( o.Smoothness, 1.0 - flGeometricRoughnessFactor ); // Ensure we don’t double-count roughness if normal map encodes geometric roughness
 
            half occ = tex2D(_OcclusionMap, IN.uv_MainTex).g;
            o.Occlusion = LerpOneTo (occ, _OcclusionStrength);
 
            half3 normal = UnpackScaleNormal(tex2D(_BumpMap, IN.uv_MainTex), _BumpScale);
            o.Normal = normal;
        }
     
#include "UnityMetaPass.cginc"
 
// vertex-to-fragment interpolation data
struct v2f_surf {
    float4 pos : SV_POSITION;
    float2 pack0 : TEXCOORD0; // _MainTex
    float4 tSpace0 : TEXCOORD1;
    float4 tSpace1 : TEXCOORD2;
    float4 tSpace2 : TEXCOORD3;
    #if defined(_CENTROIDNORMAL)
    float3 centroidNormal : TEXCOORD4_centroid;
    #endif
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};
float4 _MainTex_ST;
 
// vertex shader
v2f_surf vert_surf (appdata_full v) {
    UNITY_SETUP_INSTANCE_ID(v);
    v2f_surf o;
    UNITY_INITIALIZE_OUTPUT(v2f_surf,o);
    UNITY_TRANSFER_INSTANCE_ID(v,o);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
    o.pack0.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
    float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
    float3 worldNormal = UnityObjectToWorldNormal(v.normal);
    float3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
    float tangentSign = v.tangent.w * unity_WorldTransformParams.w;
    float3 worldBinormal = cross(worldNormal, worldTangent) * tangentSign;
    o.tSpace0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
    o.tSpace1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
    o.tSpace2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);
    #if defined(_CENTROIDNORMAL)
    o.centroidNormal = worldNormal;
    #endif
    return o;
}
 
// fragment shader
fixed4 frag_surf (v2f_surf IN) : SV_Target {
    UNITY_SETUP_INSTANCE_ID(IN);
    // prepare and unpack data
    Input surfIN;
    UNITY_INITIALIZE_OUTPUT(Input,surfIN);
    surfIN.uv_MainTex.x = 1.0;
    surfIN.uv_MainTex = IN.pack0.xy;
    float3 worldPos = float3(IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w);
    #ifndef USING_DIRECTIONAL_LIGHT
        fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
    #else
        fixed3 lightDir = _WorldSpaceLightPos0.xyz;
    #endif
    #ifdef UNITY_COMPILER_HLSL
    SurfaceOutputStandard o = (SurfaceOutputStandard)0;
    #else
    SurfaceOutputStandard o;
    #endif
    o.Albedo = 0.0;
    o.Emission = 0.0;
    o.Alpha = 0.0;
    o.Occlusion = 1.0;
    fixed3 normalWorldVertex = fixed3(0,0,1);
 
    // call surface function
    surf (surfIN, o);
    UnityMetaInput metaIN;
    UNITY_INITIALIZE_OUTPUT(UnityMetaInput, metaIN);
    metaIN.Albedo = o.Albedo;
    metaIN.Emission = o.Emission;
    return UnityMetaFragment(metaIN);
}
 
 
#endif
 
 
ENDCG
 
}
 
    // ---- end of surface shader generated code
 
    }
    FallBack "Diffuse"
}
 