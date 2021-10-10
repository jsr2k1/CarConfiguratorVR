using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;

public class CarTools : EditorWindow
{
	[MenuItem ("Custom/Create Colliders")]
	public static void CreateColliders()
	{
		int counter = 1;
		var children = Selection.activeGameObject.GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer child in children){
			Debug.Log(counter + " / " + children.Length + " : " + child.name);
			counter++;
			if(child.gameObject.GetComponent<MeshCollider>() == null) {
				child.gameObject.AddComponent<MeshCollider>();
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem ("Custom/Destroy Colliders")]
	public static void DestroyColliders()
	{
		int counter = 1;
		var children = Selection.activeGameObject.GetComponentsInChildren<Transform>();
		foreach(var child in children)
		{
			Debug.Log(counter + " / " + children.Length + " : " + child.name);
			counter++;
			Collider collider = child.gameObject.GetComponent<Collider>();
			if(collider != null) {	
				DestroyImmediate(collider);
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem ("Custom/Enable Shadows")]
	public static void EnableShadows()
	{
		var children = Selection.activeGameObject.GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer child in children){
			child.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
			child.receiveShadows = true;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem ("Custom/Disable Shadows")]
	public static void DisableShadows()
	{
		var children = Selection.activeGameObject.GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer child in children){
			child.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			child.receiveShadows = false;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem ("Custom/Enable CastShadows")]
	public static void EnableCastShadows()
	{
		var children = Selection.activeGameObject.GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer child in children){
			child.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem ("Custom/Disable CastShadows")]
	public static void DisableCastShadows()
	{
		var children = Selection.activeGameObject.GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer child in children){
			child.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem ("Custom/Enable ReceiveShadows")]
	public static void EnableReceiveShadows()
	{
		var children = Selection.activeGameObject.GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer child in children){
			child.receiveShadows = true;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem ("Custom/Disable ReceiveShadows")]
	public static void DisableReceiveShadows()
	{
		var children = Selection.activeGameObject.GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer child in children){
			child.receiveShadows = false;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem ("Custom/Disable Shadows GLASS")]
	public static void DisableShadowsGlass()
	{
		MeshRenderer[] renderers = Selection.activeGameObject.GetComponentsInChildren<MeshRenderer>();

		foreach(MeshRenderer renderer in renderers){
			if(renderer.sharedMaterial.name.StartsWith("EXT_GLASS")){
				renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem ("Custom/Find Missing Meshes")]
	public static void FindMissingMeshes()
	{
		int counter = 1;
		var children = Selection.activeGameObject.GetComponentsInChildren<MeshFilter>();
		foreach(MeshFilter child in children){
			if(child.sharedMesh == null) {
				Debug.Log(counter + " / " + children.Length + " : " + child.name);
				counter++;
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem ("Custom/Find Missing Materials")]
	public static void FindMissingMaterials()
	{
		var renderers = Selection.activeGameObject.GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer renderer in renderers) {
			if(renderer.sharedMaterial == null) {
				Debug.Log(renderer.gameObject.name);
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem ("Custom/Count Triangles")]
	public static void CountTriangles()
	{
		int total = 0;
		var children = Selection.activeGameObject.GetComponentsInChildren<MeshFilter>();
		foreach(MeshFilter child in children){
			if(child.sharedMesh != null){
				int numTriangles = child.sharedMesh.triangles.Length / 3;
				total += numTriangles;
			}
		}
		string value = total.ToString("#,#", CultureInfo.InvariantCulture);
		Debug.Log(Selection.activeGameObject.name + ": " + value + " triangles");
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem ("Custom/List Materials")]
	public static void ListMaterials()
	{
		List<string> list_materials;
		list_materials = new List<string>();

		var renderers = Selection.activeGameObject.GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer renderer in renderers) {
			if(!list_materials.Contains(renderer.sharedMaterial.name)) {
				list_materials.Add(renderer.sharedMaterial.name);
			}
		}
		foreach(string mat in list_materials) {
			Debug.Log(mat);
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem ("Custom/Assign BodyMaterialCtrl")]
	public static void AssignBodyMaterialCtrl()
	{
		Material white_mat = AssetDatabase.LoadAssetAtPath("Assets/Materials/MAT_EXT/BODYPAINT/EXT_BODY_METAL_AZUL_AGUAMARINA.mat", typeof(Material)) as Material;
		MeshRenderer[] renderers = Selection.activeGameObject.GetComponentsInChildren<MeshRenderer>();

		foreach(MeshRenderer renderer in renderers) {
			if(renderer.sharedMaterial.name.StartsWith("EXT_BODY")){
				var bodyMatCtrl = renderer.gameObject.GetComponent<BodyMaterialCtrl>();
				if(bodyMatCtrl == null){
					renderer.gameObject.AddComponent<BodyMaterialCtrl>();
				}
				renderer.sharedMaterial = white_mat;
			}
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem ("Custom/Find references of Material")]
	public static void FindMaterialReferences()
	{
		int count = 0;
		var renderers = Selection.activeGameObject.GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer renderer in renderers) {
			if(renderer.sharedMaterial.name == "EXT_GLASS_DARK"){
				Debug.Log(renderer.gameObject.name);
				count++;
			}
		}
		Debug.Log("TOTAL: " + count);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem ("Custom/Select T-Roc")]
	public static void SelectTRoc()
	{
		PlayerPrefs.SetInt("model", 0); //0: T-Roc, 1: Touareg
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem ("Custom/Select Touareg")]
	public static void SelectTouareg()
	{
		PlayerPrefs.SetInt("model", 1); //0: T-Roc, 1: Touareg
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem ("Custom/Select MeshRenderers")]
	public static void SelectMeshRenderers()
	{
		GameObject parent = Selection.activeGameObject;
		Selection.activeGameObject = null;
		List<GameObject> objs_target = new List<GameObject>();
		MeshRenderer[] renderers = parent.GetComponentsInChildren<MeshRenderer>();

		foreach(MeshRenderer meshRenderer in renderers){
			if(meshRenderer.transform.position.z > 0.4F){
				objs_target.Add(meshRenderer.transform.gameObject);
			}
		}
		Selection.objects = objs_target.ToArray();
	}
}
