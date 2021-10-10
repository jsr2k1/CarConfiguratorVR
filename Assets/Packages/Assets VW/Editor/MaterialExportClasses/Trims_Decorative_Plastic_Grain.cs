using UnityEditor;
using System.Xml;
using UnityEngine;
using System.Collections;

public class Trims_Decorative_Plastic_Grain : MaterialExport {
	
	#region implemented abstract members of MaterialExport
	public override Material CreateMaterial (XmlDocument doc, string path)
	{
		_MaterialName = base.GetMaterialName(doc);

		Material mat = new Material(Shader.Find("Leather-Unity5"));
		
		mat.SetColor("_DiffuseColor", GetDiffuseColor(doc));
		
		mat.SetColor("_ReflectiveColor", GetReflectionColor(doc));
		
		float glossiness = GetGlossiness(doc);
		mat.SetFloat ("_Glossiness", glossiness);

		if (HasTexture(doc, "bump") == true)
		{
			Texture bumpTexture = ImportTexture(path + GetBumpTextureName(doc), "bump", GetTextureRotation(doc, "bump"));
			mat.SetTexture("_BumpTexture", bumpTexture);
			mat.SetTextureScale("_BumpTexture" , new Vector2(GetTextureTilingU(doc, "bump"), GetTextureTilingV(doc, "bump")));
		}
		
		if (HasTexture(doc, "diffuse") == true)
		{
			Texture diffuseTexture = ImportTexture(path + GetDiffuseTextureName(doc), "texture", GetTextureRotation(doc, "diffuse"));
			mat.SetTexture("_DiffuseTexture", diffuseTexture);
			mat.SetTextureScale("_DiffuseTexture", new Vector2(GetTextureTilingU(doc, "diffuse"), GetTextureTilingV(doc, "diffuse")));

			// workaround for "wrong" diffusecolor values in virma / maya exported materials
			if (HasDiffuseColorModfifier(doc) == false)
			{
				mat.SetColor("_DiffuseColor", Color.white);
			}			    
		}

		float sharpness = GetFloat (doc, "sharpness", 0.6f);
		mat.SetFloat ("_Sharpness", sharpness);

		float diffuseFactor = GetFloat (doc, "diffuseFactor", 0.5f);
		mat.SetFloat ("_DiffuseFactor", diffuseFactor);

		float specularFactor = GetFloat (doc, "specularFactor", 0.1f);
		mat.SetFloat ("_SpecularFactor", specularFactor);

		// update shader features (enable features based on properties)
		GridcellShaderGUI.UpdateFeatures(mat);

		return mat;
	}

	#endregion
	
}
