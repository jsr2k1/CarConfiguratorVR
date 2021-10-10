
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Xml;
using System;

public static class ExportVehicle
{
    public static string VehicleGeometryPath { get; private set; }

    //	public static List<string> flagList = new List<string>() {
    //		"materials_maya",
    //		"shapes_maya",
    //		"lightmaps_maya",
    //		"materials_unity",
    //		"shapes_unity",
    //		"lightmaps_unity",
    //		"main_unity",
    //		"package_unity"
    //	};
    //

    private static string _vehicleFolderPath;
    private static string _unityPackagePath;

    private static Dictionary<string, float> _reductionFactors;
    private static float _default_Factor = 0.25f;
    private static float _ext_Prio1 = 0.5f;
    private static float _ext_Prio2 = 0.2f;
    private static float _int_Prio1 = 0.5f;
    private static float _int_Prio2 = 0.2f;
    private static float _int_Prio3 = 0.1f;

    [MenuItem("Assets/Vehicle Export")]
    static public void ExportVehicleEditor()
    {
        ExportBase.ClearBuildMap();

        // set true if we want to create LODs
//        ModelImportChecker.s_createLOD = true;
        // set true if we want to create a mobile asset
//        ModelImportChecker.s_createMobileAsset = true;

        _reductionFactors = new Dictionary<string, float>();

        // Exterieur
        // Prio1
        _reductionFactors.Add("FRONT_END_trID_2635", _ext_Prio1);
        _reductionFactors.Add("DOORS_EXT_trID_2065", _ext_Prio1);
        _reductionFactors.Add("ROOF_trID_825", _ext_Prio1);
        _reductionFactors.Add("REAR_END_trID_790", _ext_Prio1);
        _reductionFactors.Add("AXLE_REAR_INSERT_XZ_trID_2726", _ext_Prio1); // ->Rad hinten rechts
        _reductionFactors.Add("AXLE_FRONT_INSERT_XZ_trID_2907", _ext_Prio1); // ->Rad vorne rechts
        _reductionFactors.Add("AXLE_REAR_INSERT_XZ_trID_2814", _ext_Prio1); // ->Rad hinten links
        _reductionFactors.Add("AXLE_FRONT_INSERT_XZ_trID_2998", _ext_Prio1); // ->Rad vorne links

        // Prio2
        // Prio3
        _reductionFactors.Add("WHEEL_AXLE_trID_164", _ext_Prio2);
        _reductionFactors.Add("NON_ROTATING_PARTS_trID_2947", _ext_Prio2);
        _reductionFactors.Add("NON_ROTATING_PARTS_trID_2856", _ext_Prio2);
        _reductionFactors.Add("NON_ROTATING_PARTS_trID_2678", _ext_Prio2);
        _reductionFactors.Add("NON_ROTATING_PARTS_trID_2766", _ext_Prio2);

        // Interieur
        // Prio1
        _reductionFactors.Add("LIGHT_CONTROL_UNIT_CP_trID_4185", _int_Prio1);
        _reductionFactors.Add("ANIM_UPPER_STORAGE_BOX_trID_4703", _int_Prio1);
        _reductionFactors.Add("DECORATIV_PANELS_CP_trID_4403", _int_Prio1);
        _reductionFactors.Add("DOORS_INT_trID_5782", _int_Prio1); // ->Türen innen
        _reductionFactors.Add("CENTRE_CONSOLE_trID_5122", _int_Prio1); // ->Mittelkonsole
        _reductionFactors.Add("STEERING_WHEELS_CP_trID_4009", _int_Prio1); // ->Lenkrad
        _reductionFactors.Add("BUTTONS_CP_trID_4542", _int_Prio1); // ->Autoradio inkl.Text
        _reductionFactors.Add("INFOTAINMENT_CP_trID_4289", _int_Prio1); // ->Navi inkl.Text
        _reductionFactors.Add("STEERING_COLUMN_SWITCH_CP_trID_4125", _int_Prio1); // ->Steuerhebel inkl.Text

        // Prio2
        _reductionFactors.Add("COMMON_PARTS_CP_trID_4505", _int_Prio2);
        _reductionFactors.Add("AIRCONDITION_CONTROL_CP_trID_4651", _int_Prio2);
        _reductionFactors.Add("INSTRUMENT_CLUSTER_CP_trID_4246", _int_Prio2);
        _reductionFactors.Add("AIR_VENTS_CP_trID_4681", _int_Prio2);
        _reductionFactors.Add("ANIM_GLOVE_BOX_CP_trID_4719", _int_Prio2); // ->Handschuhfach
        _reductionFactors.Add("GREENHOUSE_UPPER_trID_8073", _int_Prio2); // ->Dach innen
        _reductionFactors.Add("GREENHOUSE_LOWER_trID_7674", _int_Prio2); // ->Innenverkleidung
        _reductionFactors.Add("DASHBOARD_CP_trID_4440", _int_Prio2); // ->Verkleidung Armaturenbrett
        _reductionFactors.Add("SEATS_trID_7475", _int_Prio2); // ->Sitze

        // Prio3
        _reductionFactors.Add("LUGGAGE_COMPARTMENT_trID_8360", _int_Prio3);
        _reductionFactors.Add("ANIM_LOWER_STORAGE_BOX_trID_4693", _int_Prio3);
        _reductionFactors.Add("PEDALS_CP_trID_4142", _int_Prio3);


		//        string path = Path.Combine(Environment.CurrentDirectory, "Golf_7_lowpoly_KK_csb");
		//        string path = Path.Combine(Environment.CurrentDirectory, "Porsche991");
		//		  string path = Path.Combine(Environment.CurrentDirectory, "ExportedCar_q5_gc_0001");
		//      string path = Path.Combine(Environment.CurrentDirectory, "Sportsvan");
		//				string path = Path.Combine(Environment.CurrentDirectory, "DummyCar");

		// DEBUG settings

		string path = "C:\\Projekte\\Erlebnisfahrzeug\\Pipeline_V3\\_temp\\2_converted\\Cars\\Maya\\Passat_Highline_csb";

		bool shapes = true;
		bool materials = true;
		bool lightmaps = true;
		bool main = true;
		bool shadow = true;
		bool unityPackage = true;

		ExportVehiclePrepareFunction(path, "Car", shapes, materials, lightmaps, main, shadow, unityPackage, string.Empty);
	}

	static public void ExportVehicleCmd()
    {
        string[] parameters = System.Environment.GetCommandLineArgs().Where(x => x.StartsWith("+")).ToArray();

        if (parameters == null || parameters.Length < 2)
        {
            return;
        }

//		string input = "";

		string modelDirectory = string.Empty;
		string savePath = string.Empty;

		bool exportShapes = false;
		bool exportMaterials = false;
		bool exportLightmaps = false;
		bool exportMain = false;
		bool exportUnityPackage = false;
		bool exportShadow = false;

		if (parameters.Length >= 1) modelDirectory = parameters[0].Substring(1);
		if (parameters.Length >= 2) savePath = parameters[1].Substring(1);

		if (parameters.Length >= 3)
		{
			for (int i = 2; i < parameters.Length; i++)
			{
				string param = parameters[i].Substring(1);

				switch (param)
				{
					case "materials_unity":
						exportMaterials = true;
						break;
					case "shapes_unity":
						exportShapes = true;
						break;
					case "lightmaps_unity":
						exportLightmaps = true;
						break;
					case "main_unity":
						exportMain = true;
						break;
					case "package_unity":
						exportUnityPackage = true;
						break;
					case "shadow_unity":
						exportShadow = true;
						break;
					default:
						break;
				}
			}
		}

//        string modelDirectory = parameters[0].Substring(1);
//        string savePath = parameters[1].Substring(1);
//        bool exportShapes = Convert.ToBoolean(parameters[2].Substring(1));
//        bool exportMaterials = Convert.ToBoolean(parameters[3].Substring(1));
//        bool exportLightmaps = Convert.ToBoolean(parameters[4].Substring(1));
//        bool exportMain = Convert.ToBoolean(parameters[5].Substring(1));
//
//        //optinal parameter: export vehicle as untiypackage
//        bool exportUnityPackage = false;
//        if (parameters.Length >= 7)
//        {
//            exportUnityPackage = Convert.ToBoolean(parameters[6].Substring(1));
//
//        }
//
//        //optional parameter: unitypackage path
//        string unitypackagePath = string.Empty;
//        if (parameters.Length >= 8)
//        {
//            unitypackagePath = parameters[7].Substring(1);
//        }
        Debug.Log("modelDirectory: " + modelDirectory + "savePath: " + savePath);

		ExportVehiclePrepareFunction(modelDirectory, savePath, exportShapes, exportMaterials, exportLightmaps, exportMain, exportShadow, exportUnityPackage);
    }

    static void ExportVehiclePrepareFunction(string modelDirectory, string savePath, bool exportShapes, bool exportMaterials, bool exportLightmaps, bool exportMain, bool exportShadow, bool exportUnityPackage = false, string unityPackagePath = "")
    {

        savePath = savePath.Replace("\\", "/");
        if (savePath.EndsWith("/") == false)
        {
            savePath += "/";
        }
		/*		
                if (Directory.Exists(savePath) == true)
                {
                    Directory.Delete(savePath, true);
                }

                Directory.CreateDirectory(savePath);
        */
		if (exportShapes && exportMaterials && exportLightmaps) { // nur löschen falls alles exportiert werden soll

            if (Directory.Exists(savePath) == true)
            {
                DeleteFolderContent(savePath);
            }

            Directory.CreateDirectory(savePath);
        }

        string[] carPaths = Directory.GetFiles(modelDirectory, "Hierarchy.xml", SearchOption.AllDirectories);

        if (carPaths == null || carPaths.Length == 0) {

            carPaths = Directory.GetFiles(modelDirectory, "vehicle.xml", SearchOption.AllDirectories);
        }

        foreach (string carpath in carPaths)
        {
            string newModelpath = carpath.Replace("\\", "/").Replace("/Hierarchy.xml", "");
            newModelpath = newModelpath.Replace("\\", "/").Replace("/vehicle.xml", "");
            string newSavepath = savePath + newModelpath.Substring(newModelpath.LastIndexOf("/") + 1) + "/";
            Directory.CreateDirectory(newSavepath);

			ExportVehicleFunction(newModelpath, newSavepath, exportShapes, exportMaterials, exportLightmaps, exportMain, exportUnityPackage, unityPackagePath, exportShadow);
        }
    }

    static List<UnityEngine.Object> _selectedObjects;

    static public string ModelType = "NONE";

    static void ExportVehicleFunction(string modelDirectory, string savePath, bool exportShapes, bool exportMaterials, bool exportLightmaps, bool exportMain, bool exportUnityPackage, string unityPackagePath = "", bool exportShadow = true)
    {
        string vehicleFolderPath = CreateVehicleFolderPath(modelDirectory);

        string vehicleDataPath = vehicleFolderPath + "/Data";
        string vehicleMaterialPath = vehicleFolderPath + "/Materials";
        string vehicleTexturePath = vehicleFolderPath + "/Textures";
        VehicleGeometryPath = vehicleFolderPath + "/Geometry";
//        string vehicleShadowGeometryPath = vehicleFolderPath + "/Shadow";
        string vehicleOcclusionmapsPath = vehicleFolderPath + "/Occlusionmaps";

        if (Directory.Exists(vehicleFolderPath) == true) //delete vehicle folder if existing from recent export.
        {
            DeleteFolderContent(vehicleFolderPath);
        }
        else
        {
            Directory.CreateDirectory(vehicleFolderPath);
        }

        Debug.Log("modelDirectory: " + modelDirectory);
        _selectedObjects = new List<UnityEngine.Object>();

        //
        // build main assetbundle
        //		
        if (exportMain)
        {
            CreateMain(modelDirectory, vehicleDataPath, savePath);

            ExportBase.ExportAssetsFromAssetBuild(savePath);
            ExportBase.ClearBuildMap();
        }

        //
        // export vehicle specific materials
        //
        if (exportMaterials) {

            ExportMaterials.ExportMaterialsFunction(modelDirectory + "/Materials", modelDirectory.Substring(modelDirectory.LastIndexOf("/") + 1) + ".materials", vehicleMaterialPath, vehicleTexturePath);

            ExportBase.ExportAssetsFromAssetBuild(savePath);
            ExportBase.ClearBuildMap();
        }

        //
        // export FBX
        //		
        if (exportShapes || exportShadow)
        {
            CreateFBX(modelDirectory, VehicleGeometryPath, savePath, exportShapes, exportShadow);

			if (ModelImportChecker.s_createMobileAsset == false)
            {
                ExportBase.ExportAssetsFromAssetBuild(savePath);
                ExportBase.ClearBuildMap();
            }
		}

		//		
		// build lightmap asset bundle
		//		
		if (exportLightmaps) {

            CreateLightmaps(modelDirectory, vehicleOcclusionmapsPath, savePath);

            ExportBase.ExportAssetsFromAssetBuild(savePath);
            ExportBase.ClearBuildMap();
        }

        //
        // create unity package for exported vehicle
        //
        if (exportUnityPackage == true)
        {
            string unitypackageSavePath = savePath;

            if (!string.IsNullOrEmpty(unityPackagePath))
            {
                unitypackageSavePath = unityPackagePath;
            }


            //CreateVehicleUnityPackage(unitypackageSavePath, vehicleFolderPath);

            _unityPackagePath = unitypackageSavePath;
            _vehicleFolderPath = vehicleFolderPath;

            CreateVehiclePrefab(vehicleFolderPath);
        }

        VehicleGeometryPath = string.Empty;
    }

    private static void CreateMain(string modelDirectory, string assetFolderPath, string savePath)
    {
        if (Directory.Exists(assetFolderPath) == false)
        {
            Directory.CreateDirectory(assetFolderPath);
        }

        _selectedObjects.Clear();
        // include all xml files
        string[] xmlPaths = Directory.GetFiles(modelDirectory, "*.xml", SearchOption.AllDirectories);
        if (xmlPaths != null)
        {
            foreach (string xmlPath in xmlPaths)
            {
                string xmlFile = Path.GetFileName(xmlPath);

                if (xmlFile.EndsWith("vehicle.xml"))
                {
                    XmlDocument vehicledoc = new XmlDocument();
                    vehicledoc.Load(xmlPath);
                    XmlNode modelTypeNode = vehicledoc.SelectSingleNode("vehicle/ModelType");
                    if (modelTypeNode != null)
                    {
                        ModelType = modelTypeNode.InnerText;
                        if (ModelType == "130")
                            CreateConfigXML(modelDirectory, assetFolderPath);
                    }
                    else
                    {
                        ModelType = "NONE";
                    }
                }

                File.Copy(xmlPath, assetFolderPath + "/" + xmlFile, true);
                AssetDatabase.ImportAsset(assetFolderPath + "/" + xmlFile);
                _selectedObjects.Add(AssetDatabase.LoadAssetAtPath(assetFolderPath + "/" + xmlFile, typeof(UnityEngine.Object)));
            }
        }

        // include all json files
        string[] jsonPaths = Directory.GetFiles(modelDirectory, "*.json", SearchOption.AllDirectories);
        if (jsonPaths != null)
        {
            foreach (string jsonPath in jsonPaths)
            {
                string jsonFile = Path.GetFileName(jsonPath);

                File.Copy(jsonPath, assetFolderPath + "/" + jsonFile, true);
                AssetDatabase.ImportAsset(assetFolderPath + "/" + jsonFile);
                _selectedObjects.Add(AssetDatabase.LoadAssetAtPath(assetFolderPath + "/" + jsonFile, typeof(UnityEngine.Object)));
            }
        }

        Debug.Log(_selectedObjects.Count);

        ExportBase.AddAssetBuildEntry(modelDirectory.Substring(modelDirectory.LastIndexOf("/") + 1) + ".main", _selectedObjects.ToArray());
    }

    private static void CreateConfigXML(string modelDirectory, string assetFolderPath)
    {
        Debug.Log("try to create xml");

        string structureFbxName = modelDirectory.Substring(modelDirectory.LastIndexOf("/") + 1);
        structureFbxName += ".fbx";
        string[] filePaths = Directory.GetFiles(modelDirectory, structureFbxName, SearchOption.AllDirectories);
        if (filePaths.Length < 0)
        {
            Debug.LogError("NO structure.fbx available: " + structureFbxName);
            return;
        }

        Debug.Log(structureFbxName + " found.");
        string assetPath = assetFolderPath + "/" + structureFbxName;
        VWImportProcessor.Init();
        File.Copy(filePaths[0], assetPath, true);
        AssetDatabase.ImportAsset(assetPath);
        Debug.Log("Try Write configxml");
        XmlDocument configDoc = VWImportProcessor.GeometryConfigXML;
        string assetPathXML = assetFolderPath + "/GeometryConfig.xml";
        configDoc.Save(assetPathXML);

        AssetDatabase.ImportAsset(assetPathXML);
        UnityEngine.Object xmlFileObj = AssetDatabase.LoadAssetAtPath(assetPathXML, typeof(UnityEngine.Object));
        _selectedObjects.Add(xmlFileObj);
        VWImportProcessor.GeometryConfigXML = null;
    }

	private static void CreateFBX(string modelDirectory, string assetFolderPath, string savePath, bool geometry = true, bool shadow = false) {
		if (Directory.Exists(assetFolderPath) == false) {
			Directory.CreateDirectory(assetFolderPath);
		}

		GameObject root = new GameObject("root");

		string[] allfbx = Directory.GetFiles(modelDirectory, "*.fbx", SearchOption.AllDirectories);

		foreach (string s in allfbx) {
			string filepath = s.Replace("\\", "/");

			string fbxName = filepath.Substring(filepath.LastIndexOf("/") + 1);
/*						
			if (fbxName != "Cockpit_trID_4923.fbx") {

				continue;
			}
*/	
			// skip Structure FBX
			string fbxWithoutExtension = fbxName.Substring(0, fbxName.LastIndexOf("."));
			if (modelDirectory.EndsWith(fbxWithoutExtension))// && ModelType == "130")
			{
				Debug.Log("skipped structure fbx export");
				continue;
			}

			// get mobile reduction factor from table
			if (ModelImportChecker.s_createMobileAsset == true) {
				float reductionFactor = _default_Factor;

				if (_reductionFactors.ContainsKey(fbxWithoutExtension)) {
					reductionFactor = _reductionFactors[fbxWithoutExtension];

					Debug.Log("using reduction: " + reductionFactor + " for FBX: " + fbxWithoutExtension);
				}
				else {
					Debug.Log("using DEFAULT reduction: " + reductionFactor + " for FBX: " + fbxWithoutExtension);
				}

				ModelImportChecker.s_mobileReduction = reductionFactor;
			}

			string assetPath = assetFolderPath + "/" + fbxName;

			File.Copy(filepath, assetPath, true);

			AssetDatabase.Refresh();

			AssetDatabase.ImportAsset(assetPath);

			UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityEngine.Object));
			UnityEngine.Object[] objects = new UnityEngine.Object[1];
			objects[0] = obj;

			//set dummyshader on vehicle fbx.
			//			SetDummyMaterialOnGameObjectRenderers((objects[0] as GameObject), dummyMaterial);

			if (geometry == true) {
				string bundleName = obj.name + ".geometry";
				ExportBase.AddAssetBuildEntry(bundleName, objects);
			}

			if (shadow == true) {
				GameObject geometryObj = GameObject.Instantiate(obj) as GameObject;
				geometryObj.transform.parent = root.transform;
			}
		}

		if (shadow == true) {
			GameObject shadowObj = ShadowMeshCreator.CreateShadowMeshObject(root);

			// material
			string materialPath = assetFolderPath + "/" + "ShadowMesh.mat";
			Shader dummyShader = Shader.Find("Legacy Shaders/Diffuse");
			Material shadowMaterial = new Material(dummyShader);
			AssetDatabase.CreateAsset(shadowMaterial, materialPath);
			shadowMaterial = (Material)AssetDatabase.LoadAssetAtPath(materialPath, typeof(Material));

			MeshRenderer renderer = shadowObj.GetComponent<MeshRenderer>();
			renderer.material = shadowMaterial;

			// create mesh
			Mesh shadowMesh = shadowObj.GetComponent<MeshFilter>().sharedMesh;
			string meshPath = assetFolderPath + "/" + shadowMesh.name + ".asset";
			AssetDatabase.CreateAsset(shadowMesh, meshPath);

			// create prefab
			string prefabPath = assetFolderPath + "/" + "ShadowMesh.prefab";
//			GameObject shadowPrefab = PrefabUtility.CreatePrefab(prefabPath, shadowObj);

			UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(UnityEngine.Object));
			UnityEngine.Object[] objects = new UnityEngine.Object[1];
			objects[0] = obj;

			string bundleName = modelDirectory.Substring(modelDirectory.LastIndexOf("/") + 1) + ".shadow";

			ExportBase.AddAssetBuildEntry(bundleName, objects);
		}
	}

	private static void CreateLightmaps(string modelDirectory, string assetFolderPath, string savePath)
	{
        if (Directory.Exists(assetFolderPath) == false)
        {
            Directory.CreateDirectory(assetFolderPath);
        }

        _selectedObjects.Clear();
		
		if (Directory.Exists(modelDirectory+"/Lightmaps/") == false)
			return;
		
		string[] bmpPaths = Directory.GetFiles (modelDirectory+"/Lightmaps/" , "*.bmp", SearchOption.AllDirectories);
		
		if(bmpPaths != null)
		{
			if(Directory.Exists(modelDirectory+"/Lightmaps"))
			{
				string saveName = modelDirectory.Substring(modelDirectory.LastIndexOf("/")+1) + ".occlusionmaps";
				string newSaveName = saveName;
				int packageCount = 0;
				for(int i=0; i < bmpPaths.Length; i++)
				{
					string bmpFile = Path.GetFileName(bmpPaths[i]);
					File.Copy(bmpPaths[i] , assetFolderPath + "/" + bmpFile , true);

                    AssetDatabase.ImportAsset(assetFolderPath + "/" + bmpFile);
					
					TextureImporter textureImporter = AssetImporter.GetAtPath(assetFolderPath + "/" + bmpFile) as TextureImporter;
					textureImporter.textureType = TextureImporterType.Default;
//					textureImporter.lightmap = false;
					textureImporter.maxTextureSize = 4096;
					textureImporter.mipmapEnabled = false;
					textureImporter.npotScale = TextureImporterNPOTScale.None;
//					textureImporter.textureFormat = TextureImporterFormat.RGB24;

					AssetDatabase.ImportAsset(assetFolderPath + "/" + bmpFile);

					_selectedObjects.Add(AssetDatabase.LoadAssetAtPath(assetFolderPath + "/" + bmpFile, typeof(UnityEngine.Object)));
					
					if (_selectedObjects.Count == 500 || i == bmpPaths.Length-1)
					{
						if (packageCount > 0)
                        {
                            newSaveName = saveName.Replace(".unity3d", "_" + packageCount + ".occlusionmap");
                        }
                        ExportBase.AddAssetBuildEntry(newSaveName, _selectedObjects.ToArray());
						_selectedObjects.Clear();
						packageCount++;
					}
				}
			}
		}
	}

	private static void SetDummyMaterialOnGameObjectRenderers(GameObject gameObject, Material dummyMaterial)
	{
/*		Shader dummyShader = Shader.Find("Legacy Shaders/Diffuse");
		if(dummyShader == null)
		{
			return; // early out when there isn't the legacy diffuse shader in the project. (should never happen)
		}
*/
		Renderer[] allRenderer = gameObject.GetComponentsInChildren<Renderer>(true);
		foreach(Renderer renderer in allRenderer)
		{
			//			Material dummyMaterial = new Material(renderer.sharedMaterial);
			//			Material dummyMaterial = new Material(dummyShader);
			//			dummyMaterial.shader = dummyShader;

			renderer.sharedMaterial = dummyMaterial;
		}
	}

    private static string CreateVehicleFolderPath(string modelDirectory)
    {
        return "Assets/Vehicles/" + CreateVehicleName(modelDirectory);
    }

    private static string CreateVehicleName(string modelDirectory)
    {
        string[] modelDirectorySplitted = modelDirectory.Split('/');
        string modelName = modelDirectorySplitted[modelDirectorySplitted.Length - 1];

        return modelName;
    }

    private static void CreateVehicleUnityPackage(string unityPackagePath, string vehicleFolderPath)
    {
        CreateExportDirectory(unityPackagePath);

        string unitypackageName = CreateVehicleName(vehicleFolderPath) + ".unitypackage";

        string completeUnityPackagePath = unityPackagePath + unitypackageName;

		string shaderFolderPath = GetShaderDirectory();

		if(string.IsNullOrEmpty(shaderFolderPath) == true)
		{
			AssetDatabase.ExportPackage(vehicleFolderPath, completeUnityPackagePath, ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Recurse);
		}
		else
		{
			string[] unitypackageContent = new string[3];
			unitypackageContent[0] = vehicleFolderPath;
			unitypackageContent[1] = shaderFolderPath;
			unitypackageContent[2] = "Assets/Resources/CarShadow";

            AssetDatabase.ExportPackage(unitypackageContent, completeUnityPackagePath, ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Recurse);
        }

        _vehicleFolderPath = string.Empty;
        _unityPackagePath = string.Empty;
    }

    private static void CreateVehiclePrefab(string vehicleFolderPath)
    {
        VehicleLoaderEditor.VehicleLoaded += VehicleLoaded;

//        GameObject go = VehicleLoaderEditor.LoadVehicleAsync(vehicleFolderPath);
    }

    private static void VehicleLoaded(GCContentLibrary.VehicleBehaviour vehicleBehaviour)
    {
        CreateVehicleUnityPackage(_unityPackagePath, _vehicleFolderPath);

        VehicleLoaderEditor.VehicleLoaded -= VehicleLoaded;
    }

	public static string GetShaderDirectory()
	{
		string shaderDirectory = string.Empty;

		Shader genericShader = Shader.Find("Generic-Unity5");

		if(genericShader != null)
		{
			string genericShaderPath = AssetDatabase.GetAssetPath(genericShader);
			shaderDirectory = genericShaderPath.Remove(genericShaderPath.LastIndexOf("/"));
		}
		else if(Directory.Exists("Resources/Shader") == true)
		{
			shaderDirectory = "Resources/Shader";
		}

		return shaderDirectory;
	}

    private static void CreateExportDirectory(string exportFolderPath)
    {
        if (exportFolderPath.Contains("\\"))
        {
            exportFolderPath = exportFolderPath.Replace("\\", "//");
        }

        if (exportFolderPath.Contains("/"))
        {
            exportFolderPath = exportFolderPath.Remove(exportFolderPath.LastIndexOf("/"));

            if (!Directory.Exists(exportFolderPath))
            {
                Directory.CreateDirectory(exportFolderPath);
            }
        }
    }

	private static void DeleteFolderContent(string folderPath) { // von Oliver

		if (Directory.Exists(folderPath)) {

			try {

				Directory.Delete(folderPath, true);
			}
			catch (Exception e) {

				Debug.LogError(e.Message);
			}
		}
		return;
	}

/*	unser alter Code.
	private static void DeleteFolderContent(string folderPath)
    {
        if (Directory.Exists(folderPath) == false)
            return;
        string[] allFiles = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);

        string[] allDirs = Directory.GetDirectories(folderPath, "*", SearchOption.AllDirectories);

        foreach (string file in allFiles)
        {
            File.Delete(file);
        }

        foreach (string d in allDirs)
        {
            DeleteFolderContent(d);
            if (Directory.Exists(d) == false)
                continue;
            Directory.Delete(d);
        }
    }
*/
}