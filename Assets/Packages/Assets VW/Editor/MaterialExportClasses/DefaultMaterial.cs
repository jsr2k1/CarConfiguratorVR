using UnityEngine;
using System.Xml;

#pragma warning disable 0436

public class DefaultMaterial : MaterialExport 
{
	#region implemented abstract members of MaterialExport
	public override Material CreateMaterial (XmlDocument doc, string path)
	{
		return new Material(Shader.Find("Standard"));
	}
	#endregion
}

#pragma warning restore 0436
