using UnityEditor;
using System.Xml;
using UnityEngine;

public class Seat_Cover_Cloth : MaterialExport {
	
	#region implemented abstract members of MaterialExport
	public override Material CreateMaterial (XmlDocument doc, string path)
	{
		_MaterialName = base.GetMaterialName(doc);

		Material mat = new Material(Shader.Find("Cloth-Unity5"));
		
		mat.SetColor("_DiffuseColor", GetDiffuseColor(doc));

		mat.SetColor("_ReflectiveColor", GetReflectionColor(doc));
		
		float glossiness = GetGlossiness(doc);
		mat.SetFloat ("_Glossiness", glossiness);
						
//		bool hasBump = false;
		if (HasTexture(doc, "bump") == true)
		{
			Texture bumpTexture = ImportTexture(path + GetBumpTextureName(doc), "bump", GetTextureRotation(doc, "bump"));
			mat.SetTexture("_BumpTexture", bumpTexture);
			mat.SetTextureScale("_BumpTexture" , new Vector2(GetTextureTilingU(doc, "bump"), GetTextureTilingV(doc, "bump")));
//			hasBump = true;
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

		float diffuseFactor = GetFloat (doc, "diffuseFactor", 0.2f);
		mat.SetFloat ("_DiffuseFactor", diffuseFactor);

		float specularFactor = GetFloat(doc, "specularFactor", 0.1f);
		mat.SetFloat ("_SpecularFactor", specularFactor);
		
		// update shader features (enable features based on properties)
		GridcellShaderGUI.UpdateFeatures(mat);

		return mat;
	}
	#endregion
	
}
