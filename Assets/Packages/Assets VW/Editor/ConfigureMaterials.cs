// C# Example
// Builds an asset bundle from the selected objects in the project view.
// Once compiled go to "Menu" -> "Assets" and select one of the choices
// to build the Asset Bundle

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Xml;
using System;

public static class ConfigureMaterials
{
	[MenuItem("Assets/Configure Materials")]
	static void ConfigureSelectedMaterials() 
	{
		Debug.Log ("Configure all materials...");

		UnityEngine.Object[] objects = Selection.objects;

		for (int count = 0; count < objects.Length; count++)
		{
			UnityEngine.Object obj = objects[count];

			// for game objects check all renderers materials
			if (obj.GetType() == typeof(GameObject))
			{
				ConfigureRenderMaterials(obj as GameObject);
			}

			// update directly selected materials
			if (obj.GetType() == typeof(Material))
			{
				GridcellShaderGUI.UpdateFeatures(obj as Material);
			}
		}

		Debug.Log ("Done");
	}

	public static void ConfigureRenderMaterials(GameObject go)
	{
		Renderer[] allRenderer = go.GetComponentsInChildren<Renderer>();
		
		foreach (Renderer renderer in allRenderer)
		{
			GridcellShaderGUI.UpdateFeatures(renderer.sharedMaterial);
		}
	}
}


