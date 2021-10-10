using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SelectionTool : EditorWindow
{
	GameObject source;
	GameObject target;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem("Custom/Selection Tool")]
	static void Init()
	{
		SelectionTool window = (SelectionTool)EditorWindow.GetWindow(typeof(SelectionTool));
		window.Show();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnGUI()
	{
		source = EditorGUILayout.ObjectField("Obj source: ", source, typeof(GameObject), true) as GameObject;
		target = EditorGUILayout.ObjectField("Obj target: ", target, typeof(GameObject), true) as GameObject;

		if(GUILayout.Button("Select")){
			DoSelection();
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void DoSelection()
	{
		Selection.activeGameObject = null;

		List<GameObject> objs_target = new List<GameObject>();

		Transform[] source_transforms = source.GetComponentsInChildren<Transform>();
		Transform[] target_transforms = target.GetComponentsInChildren<Transform>();

		foreach(Transform source_transform in source_transforms){
			foreach(Transform target_transform in target_transforms){
				if(source_transform.name == target_transform.name){
					objs_target.Add(target_transform.gameObject);
				}
			}
		}
		Selection.objects = objs_target.ToArray();
	}
}