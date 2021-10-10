using UnityEditor;
using UnityEngine;
using System.Xml;

public class Trims_Decorative_Aluminium : MaterialExport
{
	#region implemented abstract members of MaterialExport
	public override Material CreateMaterial (XmlDocument doc, string path)
	{
		_MaterialName = base.GetMaterialName(doc);

		Material mat = new Material (Shader.Find("Alu-Unity5"));

        float reflGloss = GetGlossiness(doc);
        mat.SetFloat("_Glossiness", reflGloss);

		float clearcoatIntensity = GetFloat(doc, "clearCoatIntensity", 0.4f);
		mat.SetFloat ("_ClearcoatIntensity", clearcoatIntensity);

		float clearFresnel = GetFloat(doc, "clearFresnel", 3.5f);
		mat.SetFloat ("_ClearFresnel", clearFresnel);

		float minClearCoat = GetFloat(doc, "minClearCoat", 0.02f);
		mat.SetFloat ("_MinClearCoat", minClearCoat);

		mat.SetColor("_DiffuseColor", GetDiffuseColor(doc));
		
		mat.SetColor("_ReflectiveColor", GetReflectionColor(doc));

		float paintFresnel = GetFloat(doc, "paintFresnel", 0.2f);
		mat.SetFloat ("_PaintFresnel", paintFresnel);

		float specularFactor = GetFloat(doc, "specularFactor", 0.9f);
		mat.SetFloat ("_SpecularFactor", specularFactor);

		float diffuseFactor = GetFloat(doc, "diffuseFactor", 0.1f);
		mat.SetFloat ("_DiffuseFactor", diffuseFactor);

		if (HasTexture(doc, "bump") == true)
		{
			Texture bumpTexture = ImportTexture(path + GetBumpTextureName(doc), "bump", GetTextureRotation(doc, "bump"));
			mat.SetTexture("_BumpTex", bumpTexture);
			mat.SetTextureScale("_BumpTex" , new Vector2(GetTextureTilingU(doc, "bump"), GetTextureTilingV(doc, "bump")));
		}

		// update shader features (enable features based on properties)
		GridcellShaderGUI.UpdateFeatures(mat);

		return mat;
	}
	#endregion
}