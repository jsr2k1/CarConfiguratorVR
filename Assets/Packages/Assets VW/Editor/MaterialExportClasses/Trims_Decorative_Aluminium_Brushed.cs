using UnityEditor;
using UnityEngine;
using System.Xml;

#pragma warning disable 0436

public class Trims_Decorative_Aluminum_Brushed : Trims_Decorative_Aluminium
{
	#region implemented abstract members of MaterialExport
	public override Material CreateMaterial (XmlDocument doc, string path)
	{
		Material mat = base.CreateMaterial(doc,path);

		Texture bumpTexs = (Texture)AssetDatabase.LoadMainAssetAtPath("Assets/GFX/Materials/Alu/AL_4J4_n.tif");

		mat.SetTexture("_BumpTex", bumpTexs);

		// update shader features (enable features based on properties)
		GridcellShaderGUI.UpdateFeatures(mat);

		return mat;
	}
	#endregion
}

#pragma warning restore 0436