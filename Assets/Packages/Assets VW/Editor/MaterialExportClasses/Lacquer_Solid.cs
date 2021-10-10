using UnityEditor;
using UnityEngine;
using System.Xml;

public class Lacquer_Solid : MaterialExport 
{
	#region implemented abstract members of MaterialExport
	public override Material CreateMaterial (XmlDocument doc, string path)
	{
		_MaterialName = base.GetMaterialName(doc);

		Material mat = new Material(Shader.Find("Carpaint-Unity5"));

		float clearcoatIntensity = GetFloat(doc, "clearCoatIntensity", 0.6f);
		mat.SetFloat ("_ClearcoatIntensity", clearcoatIntensity);

		float clearFresnel = GetFloat(doc, "clearFresnel", 3.5f);
		mat.SetFloat ("_ClearFresnel", clearFresnel);

		float minClearCoat = GetFloat(doc, "minClearCoat", 0.02f);
		mat.SetFloat ("_MinClearCoat", minClearCoat);

		mat.SetColor("_DiffuseColor", GetDiffuseColor(doc));

		mat.SetColor("_ReflectiveColor", GetReflectionColor(doc));

        float reflGloss = GetGlossiness(doc);
        mat.SetFloat("_Glossiness", reflGloss);

		float paintFresnel = GetFloat(doc, "paintFresnel", 0.2f);
		mat.SetFloat ("_PaintFresnel", paintFresnel);

		float flakeIntensity = GetFloat(doc, "flakeIntensity");
		mat.SetFloat ("_FlakeIntensity", flakeIntensity);

		float flakeDentensity = GetFloat(doc, "flakeDentensity");
		mat.SetFloat ("_FlakeDensity", flakeDentensity);

		float flakeViewDistance = GetFloat(doc, "flakeViewDistance");
		mat.SetFloat ("_FlakeViewDistance", flakeViewDistance);

		float microflakeSpecular = GetFloat(doc, "microflakeSpecular", 2f);
		mat.SetFloat ("_MicroflakeSpecular", microflakeSpecular);

		float microflakePerturbation = GetFloat(doc, "microflakePerturbation");
		mat.SetFloat ("_MicroflakePerturbation", microflakePerturbation);

		float normalPerturbation = GetFloat(doc, "normalPerturbation");
		mat.SetFloat ("_NormalPerturbation", normalPerturbation);

		float specularFactor = GetFloat(doc, "specularFactor");
		mat.SetFloat ("_SpecularFactor", specularFactor);

		float diffuseFactor = GetFloat(doc, "diffuseFactor", 1f);
		mat.SetFloat ("_DiffuseFactor", diffuseFactor);

		float lightmapOffset = GetFloat(doc, "lightmapOffset", 1f);
		mat.SetFloat ("_LightmapOffset", lightmapOffset);

		// update shader features (enable features based on properties)
		GridcellShaderGUI.UpdateFeatures(mat);

		return mat;
	}
	#endregion
	
}
