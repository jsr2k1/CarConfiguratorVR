using UnityEditor;
using System.Xml;
using UnityEngine;

public class Lacquer_Metallic : MaterialExport 
{

	#region implemented abstract members of MaterialExport
	public override Material CreateMaterial (XmlDocument doc, string path)
	{
		_MaterialName = base.GetMaterialName(doc);

        bool forceUV = false;

        string type = "";

        if (MaterialModificatorManager.Instance.HasMaterialSpecificModificator(_MaterialName, "type"))
        {
            MaterialModificatorManager.Instance.ApplyMaterialModificator(_MaterialName, "type", ref type);

            if (type == "Carpaint_UV")
            {
                forceUV = true;
            }
        }

        Material mat;

        if (forceUV == true)
        {
            mat = new Material(Shader.Find("Carpaint UV-Unity5"));
        }
        else
        {
            mat = new Material(Shader.Find("Carpaint-Unity5"));
        }

        float reflectionFallOff = GetFloat(doc, "reflectionFalloff", 0.05f);
		mat.SetFloat ("_ReflectionFalloff", reflectionFallOff);

        float clearCoatIntensity = GetFloat (doc, "clearCoatIntensity", 0.7f);
		mat.SetFloat ("_ClearcoatIntensity", clearCoatIntensity);

		float clearFresnel = GetFloat (doc, "clearFresnel", 3.5f);
		mat.SetFloat ("_ClearFresnel", clearFresnel);

		float minClearCoat = GetFloat (doc, "minClearCoat", 0.02f);
		mat.SetFloat ("_MinClearCoat", minClearCoat);
		
		mat.SetColor("_DiffuseColor", GetDiffuseColor(doc));
		Color reflectiveColor = GetReflectionColor(doc);
		mat.SetColor("_ReflectiveColor", reflectiveColor);
		
		float glossiness = GetGlossiness(doc);
		mat.SetFloat ("_Glossiness", glossiness);

		float paintFresnel = GetFloat (doc, "paintFresnel", 0.2f);
		mat.SetFloat ("_PaintFresnel", paintFresnel);

//		Color flakeColor = GetFlakeColor (doc, 1, 1, 1);
//		flakeColor = new Color(1,1,1);
		Color flakeColor = reflectiveColor + ((Color.white - reflectiveColor) * new Color(0.5f, 0.5f, 0.5f));
		mat.SetColor("_FlakeColor", flakeColor);
		
		Texture flakes = (Texture)AssetDatabase.LoadMainAssetAtPath("Assets/GFX/Materials/Carpaint_Metallic/flakes.tif");
		mat.SetTexture("_FlakesTex", flakes);

        float flakeSkale = GetFloat(doc, "flakeScale", 0.4f); 
        mat.SetTextureScale("_FlakesTex", new Vector2(flakeSkale, flakeSkale));

		float flakeIntensity = GetFloat (doc, "flakeIntensity", 0.35f);
		mat.SetFloat ("_FlakeIntensity", flakeIntensity);

		float flakeDensity = GetFloat (doc, "flakeDensity", 1f);
		mat.SetFloat ("_FlakeDensity", flakeDensity);

		float flakeViewDistance = GetFloat (doc, "flakeViewDistance", 1.5f);
		mat.SetFloat ("_FlakeViewDistance", flakeViewDistance);

	 	mat.SetFloat ("_MicroflakeSpecular", glossiness);

		float microflakePerturbation = GetFloat (doc, "microflakePerturbation", 10f);
		mat.SetFloat ("_MicroflakePerturbation", microflakePerturbation);

		float normalPerturbation = GetFloat (doc, "normalPerturbation", 5f);
		mat.SetFloat ("_NormalPerturbation", normalPerturbation);

		float diffuseFactor = GetFloat (doc, "diffuseFactor", 0.5f);
		mat.SetFloat ("_DiffuseFactor", diffuseFactor);		// 0.6f

		float specularFactor = GetFloat (doc, "specularFactor", 1.3f);
		mat.SetFloat ("_SpecularFactor", specularFactor);	// 1.5f

		float lightmapOffset = GetFloat (doc, "lightmapOffset", 1f);
		mat.SetFloat ("_LightmapOffset", lightmapOffset);
		
		// update shader features (enable features based on properties)
		GridcellShaderGUI.UpdateFeatures(mat);

		return mat;
	}
	#endregion
		
}
