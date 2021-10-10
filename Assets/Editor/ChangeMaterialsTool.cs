using UnityEngine;
using UnityEditor;

public class ChangeMaterialsTool : EditorWindow
{
	Material mat_source;
	Material mat_target;
	GameObject obj;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem("Custom/Change Material Tool")]
	static void Init()
	{
		ChangeMaterialsTool window = (ChangeMaterialsTool)EditorWindow.GetWindow(typeof(ChangeMaterialsTool));
		window.Show();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnGUI()
	{
		obj = EditorGUILayout.ObjectField("Obj source: ", obj, typeof(GameObject), true) as GameObject;
		mat_source = EditorGUILayout.ObjectField("Material before: ", mat_source, typeof(Material), true) as Material;
		mat_target = EditorGUILayout.ObjectField("Material after: ", mat_target, typeof(Material), true) as Material;

		if(GUILayout.Button("Change")){
			DoChangeMaterial();
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void DoChangeMaterial()
	{
		MeshRenderer[] renderers = obj.GetComponentsInChildren<MeshRenderer>();

		foreach(MeshRenderer renderer in renderers)
		{
			if(renderer.sharedMaterial != null){
				if(renderer.sharedMaterial.name.StartsWith(mat_source.name)){
					renderer.sharedMaterial = mat_target;
				}
			}
		}
	}
}