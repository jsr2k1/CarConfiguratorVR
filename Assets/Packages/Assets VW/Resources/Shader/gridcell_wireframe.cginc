#include "UnityCG.cginc"
#include "AutoLight.cginc"

struct VertexOutput {
	float4 pos : SV_POSITION;
	float2 uv0 : TEXCOORD0;
	float2 uv1 : TEXCOORD1;
	float4 posWorld : TEXCOORD2;
	float3 normalDir : TEXCOORD3;
	float3 tangentDir : TEXCOORD4;
	float3 bitangentDir : TEXCOORD5;
	
	LIGHTING_COORDS(6,7)
	#if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
		float4 ambientOrLightmapUV : TEXCOORD8;
	#endif
	
	#ifdef WIREFRAME_ON
		float3 baricentric : TEXCOORD9;
	#endif

	//optional parameter (only use one)
	#ifdef GRIDCELL_PARAM1_VPOS
		float4 vpos : TEXCOORD10;
	#endif
	
	#ifdef GRIDCELL_PARAM1_SCREENPOS
		float4 screenPos : TEXCOORD10;
	#endif

};

// Geometry Shader -----------------------------------------------------
[maxvertexcount(3)]
void geo(triangle VertexOutput p[3], inout TriangleStream<VertexOutput> triStream)
{
#ifdef WIREFRAME_ON
	p[0].baricentric = float3(1, 0, 0);
	p[1].baricentric = float3(0, 1, 0);
	p[2].baricentric = float3(0, 0, 1);
#endif			
	triStream.Append(p[0]);
	triStream.Append(p[1]);
	triStream.Append(p[2]);
}

#ifdef WIREFRAME_ON
float edgeFactor(VertexOutput i)
{
	half3 d = fwidth(i.baricentric);
	half3 a3 = smoothstep(half3(0, 0, 0), d * 1.5, i.baricentric);
	return min(min(a3.x, a3.y), a3.z);
}
#endif

fixed4 wireframe_frag(VertexOutput i, half wireframeBorder)
{
#ifdef WIREFRAME_ON
	float fEdgeIntensity = 1.0 - edgeFactor(i);

	if (fEdgeIntensity > wireframeBorder)
	{
		return fixed4(1, 1, 1, 1);
	}
	else
	{
		return fixed4(0, 0, 0, 0);
	}
#endif

	return fixed4(0, 0, 0, 0);
}