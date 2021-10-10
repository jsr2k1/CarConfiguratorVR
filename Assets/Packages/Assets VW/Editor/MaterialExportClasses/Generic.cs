using UnityEditor;
using UnityEngine;
using System.Xml;
using System.IO;

public class generic : MaterialExport 
{
	#region implemented abstract members of MaterialExport
	public override Material CreateMaterial (XmlDocument doc, string path)
	{
		_MaterialName = base.GetMaterialName(doc);

		bool forceTransparent = false;

        string type = "";

        if (MaterialModificatorManager.Instance.HasMaterialSpecificModificator(_MaterialName, "type"))
		{
			MaterialModificatorManager.Instance.ApplyMaterialModificator(_MaterialName, "type", ref type);

			if (type == "Generic_Transparent" || type == "Generic_Transparent_Front" || type == "Generic_Refractive")
			{
				forceTransparent = true;
			}
		}

		Color transparentColor = GetTransparentColor(doc);

		Color transparentLinear = transparentColor.linear;
		transparentColor.a = (transparentLinear.r + transparentLinear.b + transparentLinear.g) / 3.0f;

        if (HasProperty(doc, "transparency"))
        {
            transparentColor.a = GetFloat(doc, "transparency", 0.0f);
        }

        float transparency = transparentColor.a;

        Material mat;

		float clearFresnel = 0;

		float minClearCoat = 0;

		// transparent material?
		if ((transparency > 0.0f) || (forceTransparent == true))
		{
			// use transparent shader
			//

			float refractionIndex = GetFloat(doc, "refractionIndex", 1.0f);

			if ((refractionIndex > 1.0f) || (type == "Generic_Refractive"))
			{
				mat = new Material(Shader.Find("Generic Refractive-Unity5"));				

				mat.SetFloat ("_RefractionIndex", refractionIndex);
				float refractionAbsorb = GetFloat(doc, "refractionAbsorb", 0.0f);
				mat.SetFloat ("_RefractionAbsorb", refractionAbsorb);
			}
			else
			{
                if (type == "Generic_Transparent_Front")
                {
                    mat = new Material(Shader.Find("Generic Transparent Front-Unity5"));
                }
                else
                {
                    mat = new Material(Shader.Find("Generic Transparent-Unity5"));
                }
            }

			mat.SetColor ("_Transparency", transparentColor);
	
			clearFresnel = GetFloat(doc, "clearFresnel", 1.52f);
			minClearCoat = GetFloat(doc, "minClearCoat", 0.15f);
						
			mat.SetColor("_EmissiveColor", GetEmissiveColor(doc));
		}
		else
		{
            if (type == "Generic_Culloff")
            {
                mat = new Material(Shader.Find("Generic-Culloff-Unity5"));
            }
            else
            {
                mat = new Material(Shader.Find("Generic-Unity5"));
            }

            clearFresnel = GetFloat(doc, "clearFresnel", 3.5f);
			minClearCoat = GetFloat(doc, "minClearCoat", 0.05f);
		}
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

		Color diffuseColor = GetDiffuseColor(doc);
		Color reflectionColor = GetReflectionColor(doc);

		mat.SetColor("_DiffuseColor", diffuseColor);

		mat.SetColor("_ReflectiveColor", reflectionColor);

        float reflGloss = GetGlossiness(doc);
        mat.SetFloat("_Glossiness", reflGloss);

		float paintFresnel = GetFloat (doc, "paintFresnel", 0.1f);
		mat.SetFloat ("_PaintFresnel", paintFresnel);

		float diffuseFactor = GetFloat (doc, "diffuseFactor", 0.3f);
		mat.SetFloat ("_DiffuseFactor", diffuseFactor);

		float specularFactor = GetFloat (doc, "specularFactor", 0.1f);
		mat.SetFloat ("_SpecularFactor", specularFactor);

		float roughness = GetFloat (doc, "roughness", 0.1f);
		mat.SetFloat ("_Roughness", roughness);

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
			mat.SetTexture("_BumpTexture", bumpTexture);

			if (HasPropertyAttribute(doc, "texture", "normal", "repeatUV"))
			{
				mat.SetTextureScale("_BumpTexture" , new Vector2(GetTextureTilingU(doc, "normal"), GetTextureTilingV(doc, "normal")));
			}
			else if (HasPropertyAttribute(doc, "texture", "bump", "repeatUV"))
			{
				mat.SetTextureScale("_BumpTexture" , new Vector2(GetTextureTilingU(doc, "bump"), GetTextureTilingV(doc, "bump")));
			}

			if (HasPropertyAttribute(doc, "texture", "normal", "offsetUV"))
			{
				mat.SetTextureOffset("_BumpTexture" , new Vector2(GetTextureOffsetU(doc, "normal"), GetTextureOffsetV(doc, "normal")));
			}
			else if (HasPropertyAttribute(doc, "texture", "bump", "offsetUV"))
			{
				mat.SetTextureOffset("_BumpTexture" , new Vector2(GetTextureOffsetU(doc, "bump"), GetTextureOffsetV(doc, "bump")));
			}
		}

		if (GetDiffuseTextureName(doc) != string.Empty)
		{
			Texture diffuseTexture = ImportTexture(path + GetDiffuseTextureName(doc), "texture", GetTextureRotation(doc, "diffuse"));

            mat.SetTexture("_DiffuseTexture", diffuseTexture);
			mat.SetTextureScale("_DiffuseTexture", new Vector2(GetTextureTilingU(doc, "diffuse", 1f), GetTextureTilingV(doc, "diffuse", 1f)));
			mat.SetTextureOffset("_DiffuseTexture" , new Vector2(GetTextureOffsetU(doc, "diffuse"), GetTextureOffsetV(doc, "diffuse")));

			if (diffuseColor.r + diffuseColor.g + diffuseColor.b == 0.0f)
            {
				mat.SetColor("_DiffuseColor", new Color(0.5f, 0.5f, 0.5f, 1f));
			}
		}

		if (GetSpecularTextureName(doc) != string.Empty)
		{
			Texture specularTexture = ImportTexture(path + GetSpecularTextureName(doc), "texture", GetTextureRotation(doc, "specular"));
			mat.SetTexture("_SpecularTexture", specularTexture);
			mat.SetTextureScale("_SpecularTexture", new Vector2(GetTextureTilingU(doc, "specular"), GetTextureTilingV(doc, "specular")));
			mat.SetTextureOffset("_SpecularTexture" , new Vector2(GetTextureOffsetU(doc, "specular"), GetTextureOffsetV(doc, "specular")));

			if (reflectionColor.r + reflectionColor.g + reflectionColor.b == 0.0f) {

				mat.SetColor("_ReflectiveColor", new Color(0.5f, 0.5f, 0.5f, 1f));
			}
		}

		// update shader features (enable features based on properties)
		GridcellShaderGUI.UpdateFeatures(mat);

		return mat;
	}
	#endregion
	
}
