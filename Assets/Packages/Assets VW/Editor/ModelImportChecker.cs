using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class ModelImportChecker : AssetPostprocessor 
{
	public static bool s_createLOD = false;
    public static bool s_createMobileAsset = false;
    public static float s_mobileReduction = 0.1f;

//    static float[] s_lodDefines = { 1.0f, 0.5f, 0.3f, 0.1f };
//	static float[] s_lodDefines = { 0.1f, 0.075f, 0.05f, 0.025f };
	const float c_minDensity = 0.15f;
	/*
	void OnPreprocessModel() 
	{
		ModelImporter mi = assetImporter as ModelImporter;
		if (mi)
		{
			mi.optimizeMesh = true;
			mi.importAnimation = false;
			mi.importBlendShapes = false;
			mi.swapUVChannels = false;
			mi.importMaterials = true;
			mi.materialSearch = ModelImporterMaterialSearch.Local;
			mi.materialSearch = ModelImporterMaterialSearch.RecursiveUp;
			mi.materialName = ModelImporterMaterialName.BasedOnMaterialName;
		
			mi.normalImportMode = ModelImporterTangentSpaceMode.Import;
			mi.tangentImportMode = ModelImporterTangentSpaceMode.Calculate;
			float filesize = (new FileInfo(assetPath).Length / (1024 * 1024));
			if (filesize > 150)
			{
				UnityEngine.Debug.Log("TOO BIG "+assetPath+" :"+filesize + "-"+new FileInfo(assetPath).Length);
				mi.isReadable = false;
			}
			else
			{
				mi.isReadable = true;
			}
		}

	}

	void OnPostprocessModel(GameObject obj) 
	{
		Renderer[] allRenderer = obj.GetComponentsInChildren<Renderer>(true);

		// create and assign simple dummy material with original names to all renderers
		foreach (Renderer renderer in allRenderer) {

			string materialPath = ExportVehicle.VehicleGeometryPath + "/" + renderer.sharedMaterial.name + ".mat";
			Shader dummyShader = Shader.Find("Legacy Shaders/Diffuse");
			Material dummyMaterial = new Material(dummyShader);
			if (System.IO.File.Exists(materialPath) == false) {

				AssetDatabase.CreateAsset(dummyMaterial, materialPath);
			}
			dummyMaterial = (Material)AssetDatabase.LoadAssetAtPath(materialPath, typeof(Material));

//			Material dummyMaterial = new Material(renderer.sharedMaterial);
//			dummyMaterial.shader = dummyShader;
			renderer.sharedMaterial = dummyMaterial;
		}

		if ((s_createLOD == false) && (s_createMobileAsset == false))
        {
            return; // no post processing
        }

		foreach (MeshFilter meshObj in obj.GetComponentsInChildren<MeshFilter>())
		{
//		    Debug.Log("Postprocessing Mesh: " + meshObj.name + " Vertices: " + meshObj.sharedMesh.vertexCount);

			Mesh unityMesh = meshObj.sharedMesh;
			string meshName = unityMesh.name;

			int faceCount = unityMesh.vertexCount;

			Bounds bounds = meshObj.GetComponent<MeshRenderer>().bounds;
			float meshVolume = (bounds.size.x * bounds.size.y * bounds.size.z) * 1000.0f;
			float meshDensity = 1 / ((meshVolume / faceCount) * 1000.0f);
			int minFaces = (int)(((meshVolume * c_minDensity) * 1000.0f) + 0.5f);
			if (minFaces < faceCount * 0.1f)
			{
				minFaces = (int)(faceCount * 0.1f);
			}

            if (s_createLOD == true)
            {
			    for (int lodIndex = 1; lodIndex < s_lodDefines.Length; lodIndex++)
			    {
				    int reducedFaceCount = (int)(faceCount * s_lodDefines[lodIndex]);

				    if (reducedFaceCount < minFaces)
				    {
    //				    Debug.LogWarning("REDUCTION BELOW MIN: " + reducedFaceCount + " USING: + " + minFaces);
						
					    reducedFaceCount = minFaces;
				    }

                    Debug.Log("Reducing: " + meshObj.name);
                    Mesh resultMesh = SimplifyMesh(unityMesh, reducedFaceCount);

				    resultMesh.name = meshName + "_lod" + lodIndex;

				    GameObject lodObject = new GameObject(meshObj.name + "_lod" + lodIndex);

                    lodObject.transform.parent = meshObj.transform.parent;
                    lodObject.transform.position = meshObj.transform.position;
                    lodObject.transform.rotation = meshObj.transform.rotation;
                    lodObject.transform.localScale = meshObj.transform.localScale;

                    MeshFilter meshFilter = lodObject.AddComponent<MeshFilter>();
		            meshFilter.mesh = resultMesh;
				    MeshRenderer meshRenderer = lodObject.AddComponent<MeshRenderer>();
				    meshRenderer.material = meshObj.GetComponent<MeshRenderer>().sharedMaterial;

				    AssetDatabase.CreateAsset(resultMesh, ExportVehicle.VehicleGeometryPath + "/" + resultMesh.name + ".asset");

				    lodObject.SetActive(false);

    //					Debug.Log("Result Mesh: " + resultMesh.name + " Vertices: " + resultMesh.vertexCount);
			    }

			    // process original mesh
			    //
			    float reductionFactor = s_lodDefines[0];

			    if (reductionFactor < 1.0f)
			    {
				    int reducedFaceCount = (int)(faceCount * reductionFactor);
				    meshObj.sharedMesh = SimplifyMesh(unityMesh, reducedFaceCount, true, false);
			    }
            }

            if (s_createMobileAsset == true)
            {
                int reducedFaceCount = (int)(faceCount * s_mobileReduction);

                if (reducedFaceCount < minFaces)
                {
                    reducedFaceCount = minFaces;
                }

//                Debug.Log("Reducing: " + meshObj.name);
                Mesh resultMesh = SimplifyMesh(unityMesh, reducedFaceCount);

                resultMesh.name = meshName + "_mobile";

                AssetDatabase.CreateAsset(resultMesh, ExportVehicle.VehicleGeometryPath + "/" + resultMesh.name + ".asset");

                // just mesh asset (no gameobject and link to imported hierarchy)

//                GameObject mobileObject = new GameObject(meshObj.name + "_mobile");
//
//                mobileObject.transform.parent = meshObj.transform.parent;
//                mobileObject.transform.position = meshObj.transform.position;
//                mobileObject.transform.rotation = meshObj.transform.rotation;
//                mobileObject.transform.localScale = meshObj.transform.localScale;
//
//                MeshFilter meshFilter = mobileObject.AddComponent<MeshFilter>();
//                meshFilter.mesh = resultMesh;
//                MeshRenderer meshRenderer = mobileObject.AddComponent<MeshRenderer>();
//                meshRenderer.material = meshObj.GetComponent<MeshRenderer>().sharedMaterial;
//
//                mobileObject.SetActive(true);


            }
        }
    }

	public static Mesh SimplifyMesh(Mesh unityMesh, int targetFaceCount, bool highQuality = true, bool createNew = true) 
	{
		KrablMesh.MeshEdges kmesh = new KrablMesh.MeshEdges();
		KrablMesh.Simplify sim = new KrablMesh.Simplify();
		KrablMesh.SimplifyParameters simpars = new KrablMesh.SimplifyParameters();
		
		KrablMesh.ImportExport.UnityMeshToMeshEdges(unityMesh, kmesh);	
		simpars.targetFaceCount = targetFaceCount;
		simpars.recalculateVertexPositions = highQuality;
		simpars.checkTopology = !highQuality;
		simpars.maxEdgesPerVertex = highQuality ? 18 : 0;

		if (highQuality == false) {
			simpars.preventNonManifoldEdges = false;
			simpars.boneWeightProtection = 0.0f;
			simpars.vertexColorProtection = 0.0f;
		}
		
		sim.Execute(ref kmesh, simpars);		

		if (createNew == true)
		{
			Mesh resultMesh = new Mesh();
			KrablMesh.ImportExport.MeshEdgesToUnityMesh(kmesh, resultMesh);

			return resultMesh;
		}
		else 
		{
			string meshName = unityMesh.name;
        	KrablMesh.ImportExport.MeshEdgesToUnityMesh(kmesh, unityMesh);
			unityMesh.name = meshName;

        	return unityMesh;
		}
	}*/
}
