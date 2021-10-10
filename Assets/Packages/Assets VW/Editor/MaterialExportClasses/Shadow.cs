using UnityEditor;
using UnityEngine;
using System.Xml;

public class shadow : MaterialExport 
{
	#region implemented abstract members of MaterialExport
	public override Material CreateMaterial (XmlDocument doc, string path)
	{
		_MaterialName = base.GetMaterialName(doc);

		Material mat;
		mat = new Material(Shader.Find("Shadow-Unity5"));				

		float strength = GetFloat(doc, "strength", 1.0f);
		mat.SetFloat ("_Strength", strength);

		// update shader features (enable features based on properties)
		GridcellShaderGUI.UpdateFeatures(mat);

		return mat;
	}
	#endregion
	
}
