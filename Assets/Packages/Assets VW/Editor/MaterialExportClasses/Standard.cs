using UnityEditor;
using UnityEngine;
using System.Xml;

public class Standard : MaterialExport 
{
	public enum BlendMode
	{
		Opaque,
		Cutout,
		Fade,		// Old school alpha-blending mode, fresnel does not affect amount of transparency
		Transparent // Physically plausible transparency mode, implemented as alpha pre-multiply
	}

	#region implemented abstract members of MaterialExport
	public override Material CreateMaterial (XmlDocument doc, string path)
	{
		_MaterialName = base.GetMaterialName(doc);

		Color transparentColor = GetTransparentColor(doc);

		Color transparentLinear = transparentColor.linear;
		transparentColor.a = (transparentLinear.r + transparentLinear.b + transparentLinear.g) / 3.0f;

		float transparency = transparentColor.a;

		Material mat;

//		float clearFresnel = 0;

//		float minClearCoat = 0;

		BlendMode blendMode = BlendMode.Opaque;

		mat = new Material(Shader.Find("Standard (Specular setup)"));

		// transparent material?
		if (transparency > 0.0f)
		{
			// use transparent shader
			//
			blendMode = BlendMode.Transparent;
/*
			float refractionIndex = GetFloat(doc, "refractionIndex", 1.0f);

			if (refractionIndex > 1.0f)
			{
				mat.SetFloat ("_RefractionIndex", refractionIndex);
				float refractionAbsorb = GetFloat(doc, "refractionAbsorb", 0.0f);
				mat.SetFloat ("_RefractionAbsorb", refractionAbsorb);
			}

			transparentColor.a = 1.0f - transparentColor.a;	// note: converting from transparency to alpha (1 - value)
			mat.SetColor ("_Transparency", transparentColor);
	
			clearFresnel = GetFloat(doc, "clearFresnel", 1.52f);
			minClearCoat = GetFloat(doc, "minClearCoat", 0.15f);						
*/			
		}

		string blendModeText = GetName(doc, "blendMode");

		if (blendModeText != string.Empty)
		{
			if (blendModeText == "cutout")
			{
				blendMode = BlendMode.Cutout;
			}
			else if (blendModeText == "fade")
			{
				blendMode = BlendMode.Fade;
			}
		}

		mat.SetFloat("_Mode", (float)blendMode);
		
		mat.SetColor("_EmissionColor", GetEmissiveColor(doc));

/*
		mat.SetFloat ("_ClearFresnel", clearFresnel);
		mat.SetFloat ("_MinClearCoat", minClearCoat);

		float clearCoatIntensity = 0.4f;

		if (HasProperty(doc, "clearCoatIntensity"))
		{
			clearCoatIntensity = GetFloat(doc, "clearCoatIntensity", 0.4f);
		}
		else 
		{
			clearCoatIntensity = GetReflectivity(doc);
		}
		mat.SetFloat ("_ClearcoatIntensity", clearCoatIntensity);
*/
		Color diffuseColor = GetDiffuseColor(doc);

		// transparent material?
		if (transparency > 0.0f)
		{
			diffuseColor.a = transparency;
			mat.SetColor("_Color", diffuseColor);
		}

		Color reflectionColor = GetReflectionColor(doc);
		mat.SetColor("_SpecColor", reflectionColor);

//        float reflGloss = GetGlossiness(doc);
//        mat.SetFloat("_Glossiness", reflGloss);

//		float paintFresnel = GetFloat (doc, "paintFresnel", 0.1f);
//		mat.SetFloat ("_PaintFresnel", paintFresnel);

//		float diffuseFactor = GetFloat (doc, "diffuseFactor", 0.3f);
//		mat.SetFloat ("_DiffuseFactor", diffuseFactor);

//		float specularFactor = GetFloat (doc, "specularFactor", 0.1f);
//		mat.SetFloat ("_SpecularFactor", specularFactor);

		float roughness = GetFloat (doc, "roughness", 0.1f);
//		mat.SetFloat ("_Roughness", roughness);
		mat.SetFloat ("_Glossiness", roughness);


		float lightmapOffset = GetFloat (doc, "lightmapOffset", 1f);
		mat.SetFloat ("_LightmapOffset", lightmapOffset);

		string bumpTextureName = GetNormalTextureName(doc);

		if (bumpTextureName == string.Empty)
		{
			bumpTextureName = GetBumpTextureName(doc);
		}

		if (bumpTextureName != string.Empty)
		{
			Texture bumpTexture = ImportTexture(path + bumpTextureName, "normal", GetTextureRotation(doc, "normal"));
			mat.SetTexture("_BumpMap", bumpTexture);

			if (HasPropertyAttribute(doc, "texture", "normal", "repeatUV"))
			{
				mat.SetTextureScale("_BumpMap" , new Vector2(GetTextureTilingU(doc, "normal"), GetTextureTilingV(doc, "normal")));
			}
			else if (HasPropertyAttribute(doc, "texture", "bump", "repeatUV"))
			{
				mat.SetTextureScale("_BumpMap" , new Vector2(GetTextureTilingU(doc, "bump"), GetTextureTilingV(doc, "bump")));
			}

			if (HasPropertyAttribute(doc, "texture", "normal", "offsetUV"))
			{
				mat.SetTextureOffset("_BumpMap" , new Vector2(GetTextureOffsetU(doc, "normal"), GetTextureOffsetV(doc, "normal")));
			}
			else if (HasPropertyAttribute(doc, "texture", "bump", "offsetUV"))
			{
				mat.SetTextureOffset("_BumpMap" , new Vector2(GetTextureOffsetU(doc, "bump"), GetTextureOffsetV(doc, "bump")));
			}
		}

		if (GetDiffuseTextureName(doc) != string.Empty)
		{
			Texture diffuseTexture = ImportTexture(path + GetDiffuseTextureName(doc), "texture", GetTextureRotation(doc, "diffuse"));
			mat.SetTexture("_MainTex", diffuseTexture);
			mat.SetTextureScale("_MainTex", new Vector2(GetTextureTilingU(doc, "diffuse", 1f), GetTextureTilingV(doc, "diffuse", 1f)));
			mat.SetTextureOffset("_MainTex" , new Vector2(GetTextureOffsetU(doc, "diffuse"), GetTextureOffsetV(doc, "diffuse")));

//			if (diffuseColor.r + diffuseColor.g + diffuseColor.b == 0.0f) {
//
//				mat.SetColor("_MainTex", new Color(0.5f, 0.5f, 0.5f, 1f));
//			}
		}

		if (GetSpecularTextureName(doc) != string.Empty)
		{
			Texture specularTexture = ImportTexture(path + GetSpecularTextureName(doc), "texture", GetTextureRotation(doc, "specular"));
			mat.SetTexture("_SpecGlossMap", specularTexture);
			mat.SetTextureScale("_SpecGlossMap", new Vector2(GetTextureTilingU(doc, "specular"), GetTextureTilingV(doc, "specular")));
			mat.SetTextureOffset("_SpecGlossMap" , new Vector2(GetTextureOffsetU(doc, "specular"), GetTextureOffsetV(doc, "specular")));

//			if (reflectionColor.r + reflectionColor.g + reflectionColor.b == 0.0f) {
//
//				mat.SetColor("_SpecGlossMap", new Color(0.5f, 0.5f, 0.5f, 1f));
//			}
		}

		// update shader features (enable features based on properties)
//		StandardShaderGUI-(mat);

		return mat;
	}
	#endregion
	
}
