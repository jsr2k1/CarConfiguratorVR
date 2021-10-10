using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using GCContentLibrary;
using GCContentLibrary.HierarchyHandling;

public static class VehicleLoaderEditor
{
	private const string _dataFolderName = "Data";
    private const string _geometryFolderName = "Geometry";
    private const string _materialsFolderName = "Materials";
    private const string _occlusionmapsFolderName = "Occlusionmaps";
    private const string _texturesFolderName = "Textures";


    private const string _progressBarTitle = "Generating vehicle from folder";
    private const float _progressStep = 0.15f;

    private const string _errorDialogTitle = "Error: Wrong Folder selected";
    private const string _errorDialogMessage = "The selected folder isn't a vehicle folder. Please select a valid vehicle folder.";
    private const string _errorDialogOK = "OK";

    private static float _loadingProgress;

	private static string _vehicleFolderPath;
	private static string _relativeVehicleFolderPath;
    private static string _vehicleName;

    public static event Action<VehicleBehaviour> VehicleLoaded;
    public static event Action<VehicleBehaviour, string> VehicleError;
    public static event Action<VehicleBehaviour, string> VehicleWarning;

    public static void LoadVehicleCmd()
    {
    }

    [MenuItem("Assets/Generate vehicle from Folder")]
    public static void LoadVehicle()
    {
        string vehicleFolderPath = EditorUtility.OpenFolderPanel("Select VehicleFolder", Application.dataPath, string.Empty);

        LoadVehicleAsync(vehicleFolderPath);
    }

    public static GameObject LoadVehicleAsync(string vehicleFolderPath)
    {
        GameObject go = null;
        if (!string.IsNullOrEmpty(vehicleFolderPath))
        {
            if (IsSelectedFolderAVehicleFolder(vehicleFolderPath))
            {
                go = CreateVehicleGameObjectFromFolder(vehicleFolderPath);
            }
            else
            {
                EditorUtility.DisplayDialog(_errorDialogTitle, _errorDialogMessage, _errorDialogOK);
            }
        }

        return go;
    }

    private static GameObject CreateVehicleGameObjectFromFolder(string vehicleFolderPath)
    {
		_vehicleFolderPath = vehicleFolderPath;
		_relativeVehicleFolderPath = string.Empty;
        _vehicleName = string.Empty;

        _loadingProgress = 0.0f;
        EditorUtility.DisplayProgressBar(_progressBarTitle, "Generating vehicle...", _loadingProgress);

        _relativeVehicleFolderPath = CreateAssetRelativeFolderPath(vehicleFolderPath);
        _vehicleName = CreateVehicleNameFromFolderPath(_relativeVehicleFolderPath);

        GameObject vehicleGameObject = new GameObject(_vehicleName);
        vehicleGameObject.SetActive(false);

        //Datafiles (Hierarchy.xml, lookLibrary.json, materials.xml, vehicle.xml)
        EditorUtility.DisplayProgressBar(_progressBarTitle, "Loading vehicle data files...", Mathf.Clamp01(_loadingProgress));
        Dictionary<string,TextAsset> dataTextAssetDictionary = CreateDataTextAssetDictionary(_relativeVehicleFolderPath);
        _loadingProgress += _progressStep;

        //Textures
        EditorUtility.DisplayProgressBar(_progressBarTitle, "Loading vehicle textures...", Mathf.Clamp01(_loadingProgress));
        Texture2D[] vehicleTextures = CreateVehicleTextureArray(_relativeVehicleFolderPath);
        _loadingProgress += _progressStep;

        //Occlusionmaps
        EditorUtility.DisplayProgressBar(_progressBarTitle, "Loading vehicle occlusionmaps...", Mathf.Clamp01(_loadingProgress));
        Texture2D[] vehicleOcclusionmaps = CreateVehicleOcclusionmapArray(_relativeVehicleFolderPath);
        _loadingProgress += _progressStep;

        //Materials
        EditorUtility.DisplayProgressBar(_progressBarTitle, "Loading vehicle materials...", Mathf.Clamp01(_loadingProgress));
        Material[] vehicleMaterials = CreateVehicleMaterialArray(_relativeVehicleFolderPath);
		_loadingProgress += _progressStep;

		//Geometry
		EditorUtility.DisplayProgressBar(_progressBarTitle, "Loading vehicle geometry...", Mathf.Clamp01(_loadingProgress));
        GameObject[] vehicleGeometrys = CreateVehicleGeometryArray(_relativeVehicleFolderPath);
        _loadingProgress += _progressStep;

        //VehicleBehaviour
        EditorUtility.DisplayProgressBar(_progressBarTitle, "Initializing vehicle...", Mathf.Clamp01(_loadingProgress));
        InitializeVehicleBehaviour(vehicleFolderPath, vehicleGameObject, dataTextAssetDictionary, vehicleTextures, vehicleOcclusionmaps, vehicleMaterials, vehicleGeometrys);
        _loadingProgress += _progressStep;

        //DEBUG!!! Create vehicle geometry via editor script
        //DEBUG_CreateVehicleGeometry(vehicleGameObject, vehicleGeometrys);

        return vehicleGameObject;
    }

    private static Dictionary<string, TextAsset> CreateDataTextAssetDictionary(string relativeVehiclefolderPath)
    {
        Dictionary<string, TextAsset> dataFileDictionary = null;

        string relativeDataFolderPath = relativeVehiclefolderPath + "/" + _dataFolderName;

        var dataFilenames = Directory.GetFiles(relativeDataFolderPath).Where(name => !name.EndsWith(".meta"));

        foreach (string dataFilename in dataFilenames)
        {
            string assetPath = CreateAssetRelativeFolderPath(dataFilename);

            TextAsset dataTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);

            if (dataTextAsset != null)
            {
                if (dataFileDictionary == null)
                {
                    dataFileDictionary = new Dictionary<string, TextAsset>();
                }

                dataFileDictionary.Add(dataTextAsset.name, dataTextAsset);
            }
        }

        return dataFileDictionary;
    }

    private static Texture2D[] CreateVehicleTextureArray(string relativeVehicleFolderPath)
    {
        string relativeTextruesFolderPath = relativeVehicleFolderPath + "/" + _texturesFolderName;

        return CreateTextureAssetArray(relativeTextruesFolderPath);
    }

    private static Texture2D[] CreateVehicleOcclusionmapArray(string relativeVehicleFolderPath)
    {
        string relativeOcclusionmapsFolderPath = relativeVehicleFolderPath + "/" + _occlusionmapsFolderName;

        return CreateTextureAssetArray(relativeOcclusionmapsFolderPath);
    }

    private static Material[] CreateVehicleMaterialArray(string relativeVehicleFolderPath)
    {
        string relativeMaterialFolderPath = relativeVehicleFolderPath + "/" + _materialsFolderName;

        return CreateMaterialArray(relativeMaterialFolderPath);
    }

    private static GameObject[] CreateVehicleGeometryArray(string relativeVehicleFolderPath)
    {
        string relativeGeometryFolderPath = relativeVehicleFolderPath + "/" + _geometryFolderName;

        return CreateGameObjectArray(relativeGeometryFolderPath);
    }

    private static Texture2D[] CreateTextureAssetArray(string relativeAssetFolderPath)
    {
        List<Texture2D> textureAssetList = null;

        var assetFilenames = Directory.GetFiles(relativeAssetFolderPath).Where(name => !name.EndsWith(".meta"));

        foreach (string assetFilename in assetFilenames)
        {
            string assetPath = CreateAssetRelativeFolderPath(assetFilename);

            Texture2D occlusionmap = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);

            if (occlusionmap != null)
            {
                if (textureAssetList == null)
                {
                    textureAssetList = new List<Texture2D>();
                }

                textureAssetList.Add(occlusionmap);
            }
        }

        if (textureAssetList == null)
        {
            return null;
        }
        else
        {
            return textureAssetList.ToArray();
        }
    }

    private static Material[] CreateMaterialArray(string relativeAssetFolderPath)
    {
        List<Material> materialAssetList = null;

        var assetFilenames = Directory.GetFiles(relativeAssetFolderPath).Where(name => !name.EndsWith(".meta"));

        foreach (string assetFilename in assetFilenames)
        {
            string assetPath = CreateAssetRelativeFolderPath(assetFilename);

            Material material = AssetDatabase.LoadAssetAtPath<Material>(assetPath);

            if (material != null)
            {
                if (materialAssetList == null)
                {
                    materialAssetList = new List<Material>();
                }

                materialAssetList.Add(material);
            }
        }

        if (materialAssetList == null)
        {
            return null;
        }
        else
        {
            return materialAssetList.ToArray();
        }
    }

    private static GameObject[] CreateGameObjectArray(string relativeAssetFolderPath)
    {
        List<GameObject> gameObjectAssetList = null;

        //var assetFilenames = Directory.GetFiles(relativeAssetFolderPath).Where(name => !name.EndsWith(".meta"));
        var assetFilenames = Directory.GetFiles(relativeAssetFolderPath).Where(name => name.EndsWith(".fbx"));

        foreach (string assetFilename in assetFilenames)
        {
            string assetPath = CreateAssetRelativeFolderPath(assetFilename);

            GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            if (gameObject != null)
            {
                if (gameObjectAssetList == null)
                {
                    gameObjectAssetList = new List<GameObject>();
                }

                gameObjectAssetList.Add(gameObject);
            }
        }

        if (gameObjectAssetList == null)
        {
            return null;
        }
        else
        {
            return gameObjectAssetList.ToArray();
        }
    }

    private static VehicleBehaviour InitializeVehicleBehaviour(string vehicleFolderPath, GameObject vehicleGameObject, Dictionary<string, TextAsset> dateFileDictionary, 
        Texture2D[] vehicleTextures, Texture2D[] vehicleOcclusionmaps, Material[] vehicleMaterials, GameObject[] vehicleGeometrys)
    {
        int ignoredLayerNumber = GetVehicleLayerNumber();
        Material blobShadowMaterial = FindBlobShadowMaterial();

        VehicleBehaviour vehicleBehaviour = vehicleGameObject.AddComponent<VehicleBehaviour>();
        vehicleBehaviour.Editor_Initialize(vehicleFolderPath, dateFileDictionary, vehicleOcclusionmaps, vehicleMaterials, vehicleGeometrys, blobShadowMaterial, ignoredLayerNumber,
            OnVehicleBehaviourReady, OnVehicleBehaviourError, OnVehicleBehaviourWarning);

        return vehicleBehaviour;
    }

    private static void OnVehicleBehaviourReady(VehicleBehaviour vehicleBehaviour)
    {
        int layerNumber = LayerMask.NameToLayer("Car");
        vehicleBehaviour.SetLayer(layerNumber, true);

        vehicleBehaviour.ShowShadow(true, false);

        CreateOcclusionMaterials(vehicleBehaviour);

        if (ModelImportChecker.s_createMobileAsset == true)
        {
            // mobile asset creation => exchange meshes
            //
            MeshFilter[] filters = vehicleBehaviour.GetGeometryMeshFilter();

            foreach (MeshFilter filter in filters)
            {
                string meshName = filter.sharedMesh.name;

                string assetPath = _relativeVehicleFolderPath + "/" + _geometryFolderName + "/" + meshName + "_mobile.asset";

                Mesh mesh = AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);
                filter.sharedMesh = mesh;
            }
        }

        _loadingProgress = 0.95f;

		// DummyMaterialien löschen
		string[] materialsPaths = Directory.GetFiles(Path.Combine(_vehicleFolderPath, _geometryFolderName), "*.mat", SearchOption.AllDirectories);

		if (materialsPaths != null) {

			foreach (string materialPath in materialsPaths) {

				if (materialPath.EndsWith("ShadowMesh.mat")) {

					continue;
				}

//				Debug.Log("Deleting DummyMaterial: " + materialPath);
				AssetDatabase.DeleteAsset(materialPath);
			}
		}


		EditorUtility.DisplayProgressBar(_progressBarTitle, "Saving vehicle as Prefab...", Mathf.Clamp01(_loadingProgress));
        CreateAndSaveVehiclePrefab(_relativeVehicleFolderPath, vehicleBehaviour.gameObject);

        EditorUtility.ClearProgressBar();

        if (VehicleLoaded != null)
        {
            VehicleLoaded.Invoke(vehicleBehaviour);
        }
    }

    private static void OnVehicleBehaviourWarning(VehicleBehaviour vehicleBehaviour, string warningMessage)
    {
        Debug.LogWarning(warningMessage);
        EditorUtility.DisplayDialog("Warning", warningMessage, _errorDialogOK);

        if (VehicleWarning != null)
        {
            VehicleWarning.Invoke(vehicleBehaviour, warningMessage);
        }
    }

    private static void OnVehicleBehaviourError(VehicleBehaviour vehicleBehaviour, string errorMessage, System.Exception exception)
    {
        EditorUtility.ClearProgressBar();

        string errMsg = string.Empty;

        if (exception != null)
        {
            Debug.Log(exception);
            EditorUtility.DisplayDialog("Error", exception.Message, _errorDialogOK);

            errMsg = exception.Message;
        }
        else
        {
            Debug.Log(errorMessage);
            EditorUtility.DisplayDialog("Error", errorMessage, _errorDialogOK);

            errMsg = errorMessage;
        }

        if(VehicleError != null)
        {
            VehicleError.Invoke(vehicleBehaviour, errMsg);
        }
    }

    private static void CreateOcclusionMaterials(VehicleBehaviour vehicleBehaviour)
    {
        Dictionary<string, Shape> shapeDictionary = vehicleBehaviour.GetVehicleShapeDictionary(); ;

        MeshFilter[] vehicleGeometryMeshFilter = vehicleBehaviour.GetGeometryMeshFilter();
//        MeshFilter[] wheelGeometryMeshFilter = vehicleBehaviour.GetWheelsGeometryMeshFilter(); ;

        Texture2D[] vehicleOcclusionMaps = vehicleBehaviour.GetVehicleOcclusionMaps();

        if (shapeDictionary != null && vehicleGeometryMeshFilter != null && vehicleOcclusionMaps != null)
        {
            CreateOcclusionMaterialsByMeshFilter(vehicleGeometryMeshFilter, shapeDictionary, vehicleOcclusionMaps);
        }
/*
        if (shapeDictionary != null && wheelGeometryMeshFilter != null && vehicleOcclusionMaps != null)
        {
            CreateOcclusionMaterialsByMeshFilter(wheelGeometryMeshFilter, shapeDictionary, vehicleOcclusionMaps);
        }
*/
    }

    private static void CreateOcclusionMaterialsByMeshFilter(MeshFilter[] geometryMeshFilter, Dictionary<string, Shape> shapes, Texture2D[] occlusionmaps)
    {
        foreach (MeshFilter meshFilter in geometryMeshFilter)
        {
            if (meshFilter == null)
            {
                continue;
            }

            Texture2D occlusionmap = FindOcclusionmapForMeshfilter(meshFilter, shapes, occlusionmaps);

            if (occlusionmap != null)
            {
                Renderer renderer = meshFilter.gameObject.GetComponent<Renderer>();

                if (renderer != null && (renderer.sharedMaterial.HasProperty("_Lightmap") || renderer.sharedMaterial.HasProperty("_Occlusionmap")))
                {
                    string realtiveMasterMaterialAssetPath = AssetDatabase.GetAssetPath(renderer.sharedMaterial);
                    string relativeMasterMaterialFolderPath = realtiveMasterMaterialAssetPath.Remove(realtiveMasterMaterialAssetPath.LastIndexOf("/"));

                    string relativeSubMaterialFolderPath = relativeMasterMaterialFolderPath + "/" + renderer.sharedMaterial.name;
                    string absoulteSubMaterialFolderPath = Application.dataPath.Remove(Application.dataPath.LastIndexOf("/")) + "/" + relativeSubMaterialFolderPath;

                    DirectoryInfo absoluteSubMaterialFolderDirectoryInfo = new DirectoryInfo(absoulteSubMaterialFolderPath);
                    if (absoluteSubMaterialFolderDirectoryInfo.Exists == false)
                    {
                        absoluteSubMaterialFolderDirectoryInfo.Create();
                    }

                    string occMaterialName = renderer.sharedMaterial.name += "_" + occlusionmap.name + ".mat";

                    bool alreadyExisting = false;
                    foreach (FileInfo fileInfo in absoluteSubMaterialFolderDirectoryInfo.GetFiles())
                    {
                        if (fileInfo.Extension != ".mat")
                        {
                            continue;
                        }

                        if (fileInfo.Name == occMaterialName)
                        {
                            alreadyExisting = true;
                            break;
                        }
                    }

                    string relativeOccMaterialAssetPath = relativeSubMaterialFolderPath + "/" + occMaterialName;

                    if (alreadyExisting == false)
                    {
                        AssetDatabase.CopyAsset(realtiveMasterMaterialAssetPath, relativeOccMaterialAssetPath);
                        AssetDatabase.SaveAssets();
                    }

                    Material occMaterial = AssetDatabase.LoadAssetAtPath<Material>(relativeOccMaterialAssetPath);
                    renderer.sharedMaterial = occMaterial;

                    if (renderer.sharedMaterial.HasProperty("_Lightmap"))
                    {
                        renderer.sharedMaterial.SetTexture("_Lightmap", occlusionmap);
                        renderer.sharedMaterial.SetFloat("_LightmapOffset", 0.0f);
                    }
                    else if (renderer.sharedMaterial.HasProperty("_Occlusionmap"))
                    {
                        renderer.sharedMaterial.SetTexture("_Occlusionmap", occlusionmap);
                        //renderer.material.SetFloat("_OcclusionmapOffset", 0.0f);
                    }

                    renderer.sharedMaterial.EnableKeyword("OCCLUSIONMAP_ON");
                    AssetDatabase.SaveAssets();
                }
            }
        }
    }

    private static Texture2D FindOcclusionmapForMeshfilter(MeshFilter meshFilter, Dictionary<string, Shape> shapes, Texture2D[] occMaps)
    {
        Texture2D occlusionmap = null;

        string meshName = meshFilter.transform.name.Replace(" Instance", string.Empty);

//        bool isMeshPart = CheckIsMeshPart(ref meshName);

        string occlusionmapName = (GetOcclusionmapName(meshName, shapes)).Replace(".bmp", string.Empty);

        occlusionmap = GetOcclusionmapByName(occlusionmapName, occMaps);

        return occlusionmap;
    }

    private static bool CheckIsMeshPart(ref string meshName)
    {
		if (meshName.Contains("_MeshPart")) {

            return true;
        }

        return false;
    }

    private static string GetOcclusionmapName(string meshName, Dictionary<string, Shape> shapes)
    {
		if (meshName.Contains("_MeshPart")) {

			int meshPartIndex = meshName.LastIndexOf("_MeshPart");
			meshName = meshName.Remove(meshPartIndex);
		}

		if (meshName.Contains("|")) {

			int index = meshName.LastIndexOf("|");
			meshName = meshName.Remove(0, index + 1);
		}

		if (shapes.ContainsKey(meshName))
        {
            return shapes[meshName].OcclusionmapName;
        }

        return string.Empty;
    }

    private static Texture2D GetOcclusionmapByName(string occlusionmapName, Texture2D[] occMaps)
    {
        Texture2D occlusionmap = null;

        foreach (Texture2D texture2D in occMaps)
        {
            if (texture2D.name == occlusionmapName)
            {
                occlusionmap = texture2D;
                break;
            }
        }
        return occlusionmap;
    }

    private static bool IsSelectedFolderAVehicleFolder(string folderPath)
    {
        bool returnValue = false;

        bool hasDataFolder = false;
        bool hasGeometryFolder = false;
        bool hasMaterialsFolder = false;
        bool hasOcclusionmapsFolder = false;
        bool hasTexturesFolder = false;

        string[] subDirectoryNames = Directory.GetDirectories(folderPath);

        foreach (string subDirectoryName in subDirectoryNames)
        {
            if (subDirectoryName.EndsWith(_dataFolderName))
            {
                hasDataFolder = true;
            }
            else if (subDirectoryName.EndsWith(_geometryFolderName))
            {
                hasGeometryFolder = true;
            }
            else if (subDirectoryName.EndsWith(_materialsFolderName))
            {
                hasMaterialsFolder = true;
            }
            else if (subDirectoryName.EndsWith(_occlusionmapsFolderName))
            {
                hasOcclusionmapsFolder = true;
            }
            else if (subDirectoryName.EndsWith(_texturesFolderName))
            {
                hasTexturesFolder = true;
            }
        }

        if (hasDataFolder == true && hasGeometryFolder == true && hasMaterialsFolder == true && hasOcclusionmapsFolder == true && hasTexturesFolder == true)
        {
            returnValue = true;
        }

        return returnValue;
    }

    private static string CreateAssetRelativeFolderPath(string absoluteFolderPath)
    {
        string relativeVehicleFolderPath = ConvertPathToURL(absoluteFolderPath);

        relativeVehicleFolderPath = relativeVehicleFolderPath.Substring(relativeVehicleFolderPath.IndexOf("Assets/"));

        return relativeVehicleFolderPath;
    }

    private static string CreateVehicleNameFromFolderPath(string vehicleFolderPath)
    {
        string vehicleName = string.Empty;

        try
        {
            string[] splittedVehicleFolderPath = vehicleFolderPath.Split('\\', '/');

            vehicleName = splittedVehicleFolderPath[splittedVehicleFolderPath.Length - 1];
        }
        catch
        {
            vehicleName = vehicleFolderPath;
        }

        return vehicleName;
    }

    private static string ConvertPathToURL(string toConvert)
    {
        if (toConvert.Contains("\\"))
        {
            toConvert = toConvert.Replace("\\", "/");
        }

        return toConvert;
    }

    private static GameObject CreateAndSaveVehiclePrefab(string relativeVehicleFolderPath, GameObject vehicleGameObject)
    {
        string prefabAssetPath = relativeVehicleFolderPath + "/"+_vehicleName+ ".prefab";

        vehicleGameObject.SetActive(true);
        GameObject vehiclePrefab = PrefabUtility.CreatePrefab(prefabAssetPath, vehicleGameObject, ReplacePrefabOptions.ReplaceNameBased);

        GameObject.DestroyImmediate(vehicleGameObject);    

        PrefabUtility.InstantiatePrefab(vehiclePrefab);

        AssetDatabase.SaveAssets();        

        return vehiclePrefab;
    }

    private static int GetVehicleLayerNumber()
    {
        int vehicleLayerNumber = 0;

        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset"));
        SerializedProperty layers = tagManager.FindProperty("layers");

        if (layers != null && layers.isArray)
        {
            bool hasCarLayer = false;

            //Find car layer if existing
            for (int i = 0; i < layers.arraySize; i++)
            {
                SerializedProperty layerSP = layers.GetArrayElementAtIndex(i);
                if (layerSP.stringValue == "Car")
                {
                    vehicleLayerNumber = i;
                    hasCarLayer = true;
                }
            }

            //Create car layer if not existing
            if (hasCarLayer == false)
            {
                for (int i = 8; i < layers.arraySize; i++)
                {
                    SerializedProperty layerSP = layers.GetArrayElementAtIndex(i);
                    if (string.IsNullOrEmpty(layerSP.stringValue))
                    {
                        layerSP.stringValue = "Car";
                        vehicleLayerNumber = i;

                        tagManager.ApplyModifiedProperties();
                        break;
                    }
                }
            }
        }

        return vehicleLayerNumber;
    }

    private static Material FindBlobShadowMaterial()
    {
        return Resources.Load<Material>("CarShadow/Materials/ShadowMaterial");
    }

    #region DEBUG
    private static void DEBUG_CreateVehicleGeometry(GameObject vehicleGameObject, GameObject[] vehicleGeometrys)
    {
        GameObject geometryGameObject = new GameObject("Geometry");
        geometryGameObject.transform.SetParent(vehicleGameObject.transform);
        foreach (GameObject vehicleGeometry in vehicleGeometrys)
        {
            GameObject gameObject = GameObject.Instantiate(vehicleGeometry) as GameObject;
            gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);

            gameObject.transform.SetParent(geometryGameObject.transform);
        }
    }
    #endregion
}