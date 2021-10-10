using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

public class AssignLightmapIndex : EditorWindow
{
	GameObject prefab;
	GameObject target;

	[MenuItem ("Custom/AssignLightmap")]
	public static void AssignLightmap()
	{
		EditorWindow.GetWindow (typeof(AssignLightmapIndex));
	}

	void OnGUI()
	{
		GUILayout.Label("Apply prefabs");

		prefab = (GameObject)EditorGUILayout.ObjectField("Prefab:", prefab, typeof(Object), true);
		target = (GameObject)EditorGUILayout.ObjectField("Target:", target, typeof(Object), true);

		if(GUILayout.Button("Apply")){
			Debug.Log("Apply");
			CopyLightmapParams();
		}
	}

	void CopyLightmapParams()
	{
		var prefab_renderers = prefab.GetComponentsInChildren<Renderer>();
		foreach(Renderer prefab_renderer in prefab_renderers){
			Renderer[] target_renderers = target.GetComponentsInChildren<Renderer>(true);
			foreach(Renderer target_renderer in target_renderers){
				if(target_renderer.name == prefab_renderer.name){
					target_renderer.lightmapIndex = prefab_renderer.lightmapIndex;
					target_renderer.lightmapScaleOffset = prefab_renderer.lightmapScaleOffset;
					break;
				}
			}
		}
	}

		//Debug.Log(Selection.activeGameObject.GetComponent<Renderer>().lightmapIndex);

//		Selection.activeGameObject.GetComponent<Renderer>().lightmapIndex = 1;
//		Selection.activeGameObject.GetComponent<Renderer> ().lightmapScaleOffset = new Vector4(0.6349672F, 0.6349672F, 0F, 0F);


//		int i=1;
//		foreach(Transform child in Selection.activeGameObject.transform)
//		{
//			child.name = "Button_" + Selection.activeGameObject.transform.name + "_" + i.ToString("00");
//			child.GetChild(0).GetComponent<Text>().text = Selection.activeGameObject.transform.name + "_" + i.ToString("00");
//			i++;
//		}
//	}
}
