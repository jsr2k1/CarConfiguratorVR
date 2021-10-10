using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Xml;
using System;

public static class ExportMaterials
{
    public static string MaterialFolderPath;
    public static string TextureFolderPath;

    private const string _virmaFolderPath = "Assets/VirMa";

	[MenuItem("Assets/MaterialExport")]
	static public void ExportMaterialsEditor()
	{
        ExportBase.ClearBuildMap();

		string path = Path.Combine(Environment.CurrentDirectory, "ExportedMaterials");

        if (Directory.Exists(_virmaFolderPath) == true)
        {
            DeleteRecentVirmaFolderContent(_virmaFolderPath);
        }
        else
        {
            Directory.CreateDirectory(_virmaFolderPath);
        }

        string virmaMaterialFolderPath = _virmaFolderPath + "/Materials";
        string virmaTextureFolderPath = _virmaFolderPath + "/Textures";

		ExportMaterialsFunction(path, "Materials.unity3d", virmaMaterialFolderPath, virmaTextureFolderPath, true, true, string.Empty);
	}
	
	static public void ExportMaterialsCmd()
	{
		string[] parameters = System.Environment.GetCommandLineArgs ().Where (x => x.StartsWith ("+")).ToArray ();

		foreach(string paremeter in parameters)
		{
			Debug.Log("Parameter: " + paremeter);
		}
		
		if (parameters == null || parameters.Length < 2)
		{
			return;
		}

		string materialSourceDirectory = parameters[0].Substring(1);
		string assetBundleSavePath = parameters[1].Substring(1);

        //optinal parameter: export virma as unitypackage
        bool exportUnityPackage = false;
        if (parameters.Length >= 3)
        {
            exportUnityPackage = Convert.ToBoolean(parameters[2].Substring(1));
        }

        //optinal parameter: unitypackage path
        string unityPackageSavePath = string.Empty;
        if (parameters.Length >= 4)
		{
            unityPackageSavePath = parameters[3].Substring(1);
		}

        if (Directory.Exists(_virmaFolderPath) == true)
        {
            DeleteRecentVirmaFolderContent(_virmaFolderPath);
        }
        else
        {
            Directory.CreateDirectory(_virmaFolderPath);
        }

        string virmaMaterialFolderPath = _virmaFolderPath + "/Materials";
        string virmaTextureFolderPath = _virmaFolderPath + "/Textures";

        ExportMaterialsFunction(materialSourceDirectory, assetBundleSavePath, virmaMaterialFolderPath, virmaTextureFolderPath, true, exportUnityPackage, unityPackageSavePath);
    }
	
	public static void ExportMaterialsFunction(string materialDirectory, string assetBundleSavePath, string materialFolderPath, string textureFolderPath, bool virmaExport = false, bool exportUnityPackage = false, string unityPackageSavePath = "")
	{
		if (Directory.Exists(materialDirectory) == false)
        {
            return;
        }

        TextureFolderPath = textureFolderPath;

        if (File.Exists(materialDirectory+"/materials.xml"))
        {
			MaterialModificatorManager.Instance.Init(materialDirectory);
        }

        string[] allxmd  = Directory.GetFiles(materialDirectory, "*.xmd", SearchOption.AllDirectories);
		UnityEngine.Object[] createdMaterials = new UnityEngine.Object[allxmd.Length];
		string[] createdMaterialsAssetPaths = new string[allxmd.Length];
		int id = 0;
		
		Dictionary<string, MaterialExport> _matCreators = new Dictionary<string, MaterialExport>();
		
		foreach (string s in allxmd)
		{
/*			string fileName = Path.GetFileName(s);
	
			// test export for single material (DEBUG)
			if (fileName != "L__A3X.xmd")
			{
				continue;
			}
*/
//			Debug.Log ("EXPORTING: " + s);

			XmlDocument doc = new XmlDocument();
			doc.Load(s);
			XmlNode nameNode = doc.DocumentElement.SelectSingleNode("/Material/Name");
			XmlNode typeNode = doc.DocumentElement.SelectSingleNode("/Material/MaterialType");
			string filepath = s.Replace("\\", "/");
			if (typeNode == null) 
			{
				continue;
			}

			string name = nameNode.InnerText;
			string type = typeNode.InnerText;

			if (type == "phong" || type == "blinn" || type == "lambert")
			{
				type = "generic";
			}

			if (type == "Seat_Cover_Alcantara")
			{
				type = "Seat_Cover_Cloth";
			}

			// modify material type?
			//
			if (MaterialModificatorManager.Instance.HasMaterialSpecificModificator(name, "type"))
			{
				MaterialModificatorManager.Instance.ApplyMaterialModificator(name, "type", ref type);
//				Debug.Log("MaterialType changed! MaterialName: " + name + " new Type: " + type);
			}

			if (type == "Generic")
			{
				type = "generic";
			}

			if (type == "Generic_Transparent" || type == "Generic_Culloff" || type == "Generic_Transparent_Front" || type == "Generic_Refractive")
			{
				type = "generic";
			}

            if (type == "Carpaint_UV")
            {
                type = "Lacquer_Metallic";
            }

            Material mat = null;

			if (_matCreators.ContainsKey(type) == false)
			{
				MaterialExport me = (MaterialExport)Assembly.GetExecutingAssembly().CreateInstance(type);

				if (me == null)
				{
                    Debug.Log("Redirection of type: " + type + " to generic type.");

                    type = "generic";

                    me = (MaterialExport)Assembly.GetExecutingAssembly().CreateInstance(type);
				}

				_matCreators[type] = me;
			}
			
			string matFolder = filepath.Substring(0, filepath.LastIndexOf("/") + 1);

			int retryCount = 0;
			bool materialCreated = false;

			while ((materialCreated == false) && (retryCount < 10))
			{
				try 
				{
					mat = _matCreators[type].CreateMaterial(doc, matFolder);

					// create asset from material
					//
					if (Directory.Exists(materialFolderPath) == false)
					{
						Directory.CreateDirectory(materialFolderPath);
					}
					
					string assetPath = materialFolderPath + "/" + filepath.Substring(filepath.LastIndexOf("/")+1, filepath.LastIndexOf(".") - (filepath.LastIndexOf("/")+1)) + ".mat";
					AssetDatabase.CreateAsset(mat, assetPath);
					createdMaterials[id] = AssetDatabase.LoadMainAssetAtPath(assetPath);
					
					createdMaterialsAssetPaths[id] = assetPath;	// add asset to package

					materialCreated = true;

					id++;
				}
				catch (MissingReferenceException e) 
				{
					e.GetBaseException();
					Debug.LogError ("EXCEPTION FOR MATERIAL: " + name + " TYPE: " + type);
//					Debug.LogError (e.GetType());
//					Debug.LogError (e.InnerException);

					retryCount++;
					Debug.LogError ("RETRY: " + retryCount);
				}
			}            
		}

		if (createdMaterials.Length > 0)
		{
			if (string.IsNullOrEmpty(assetBundleSavePath))
			{
				assetBundleSavePath = Environment.CurrentDirectory + "/Materials.unity3d";
			}
			string assetBundleName = CreateAssetBundleName(assetBundleSavePath);			

//			Debug.Log("AssetBundleName: " + assetBundleName);
			ExportBase.AddAssetBuildEntry(assetBundleName, createdMaterials);

			if (virmaExport)
			{
				if(string.IsNullOrEmpty(unityPackageSavePath))
				{
					unityPackageSavePath = assetBundleSavePath.Remove(assetBundleSavePath.LastIndexOf("."), assetBundleSavePath.Substring(assetBundleSavePath.LastIndexOf(".")).Length) + ".unitypackage";
				}

      			CreateMaterialUnityPackage(_virmaFolderPath, unityPackageSavePath);

                CreateMaterialAssetBundle(assetBundleSavePath);
			}
		}

        //MaterialFolderPath = string.Empty;
        //TextureFolderPath = string.Empty;
    }

	private static string CreateAssetBundleName(string assetBundleSavePath)
	{
		if (assetBundleSavePath.Contains("\\"))
		{
			assetBundleSavePath = assetBundleSavePath.Replace("\\", "/");
		}
		
		string assetBundleName = assetBundleSavePath;
		
		if (assetBundleSavePath.Contains("/"))
		{
			assetBundleName = assetBundleSavePath.Substring(assetBundleSavePath.LastIndexOf("/") + 1);
		}

		return assetBundleName;
	}
	
	private static void CreateMaterialAssetBundle(string assetBundleSavePath)
	{
		string exportFolderPath = CreateExportDirectory(assetBundleSavePath);

		if (File.Exists (assetBundleSavePath) == true) 
		{
			File.Delete(assetBundleSavePath);
		}

		ExportBase.ExportAssetsFromAssetBuild(exportFolderPath);
	}

	private static string CreateExportDirectory(string exportFolderPath)
	{
		Debug.Log ("ExportFodlerPath:" + exportFolderPath);

		if (exportFolderPath.Contains("\\"))
		{
			exportFolderPath = exportFolderPath.Replace("\\", "/");
		}

        if (exportFolderPath.Contains("/"))
        {
            exportFolderPath = exportFolderPath.Remove(exportFolderPath.LastIndexOf("/"));
            Debug.Log("ExportFodlerPath:" + exportFolderPath);

            if (!Directory.Exists(exportFolderPath))
            {
                Directory.CreateDirectory(exportFolderPath);
            }
        }
        else
        {
            exportFolderPath = Environment.CurrentDirectory;
        }

		return exportFolderPath;
	}
	
	private static void CreateMaterialUnityPackage(string virmaFolder, string unityPackagePath)
	{
		CreateExportDirectory(unityPackagePath);

        string shaderFolderPath = ExportVehicle.GetShaderDirectory();

        if (string.IsNullOrEmpty(shaderFolderPath) == true)
        {
            AssetDatabase.ExportPackage(virmaFolder, unityPackagePath, ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Recurse);
        }
        else
        {
            string[] unitypackageContent = new string[2];
            unitypackageContent[0] = virmaFolder;
            unitypackageContent[1] = shaderFolderPath;

            AssetDatabase.ExportPackage(unitypackageContent, unityPackagePath, ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Recurse);
        }

	}

    private static void DeleteRecentVirmaFolderContent(string virmaFolderPath)
    {
        string[] allFiles = Directory.GetFiles(virmaFolderPath, "*", SearchOption.AllDirectories);

        string[] allDirs = Directory.GetDirectories(virmaFolderPath, "*", SearchOption.AllDirectories);

        foreach (string file in allFiles)
        {
            File.Delete(file);
        }

        foreach (string d in allDirs)
        {
            Directory.Delete(d);
        }
    }
}