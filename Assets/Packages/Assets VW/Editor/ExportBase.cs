using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public static class ExportBase
{
	private static List<AssetBundleBuild> _buildMap;

    public static void AddAssetBuildEntry(string assetBundleName, UnityEngine.Object[] objects)
    {
        if (objects != null)
        {
            AssetBundleBuild abb = new AssetBundleBuild();
            abb.assetBundleName = assetBundleName;
            string[] assets = new string[objects.Length];
            int id = 0;
            foreach (UnityEngine.Object asset in objects)
            {
                if (asset != null)
                {
                    assets[id] = AssetDatabase.GetAssetPath(asset);

                    Debug.Log(assets[id]);
                }

                id++;
            }
            abb.assetNames = assets;
            if (_buildMap == null) 
				_buildMap = new List<AssetBundleBuild>();

			_buildMap.Add(abb);
        }
    }

	public static void ClearBuildMap() {

		_buildMap = new List<AssetBundleBuild>();
	}

    public static void ExportAssetsFromAssetBuild(string savepath)
    {
        if (_buildMap != null)
        {
            BuildPipeline.BuildAssetBundles(savepath, _buildMap.ToArray(), BuildAssetBundleOptions.ForceRebuildAssetBundle, EditorUserBuildSettings.activeBuildTarget);
        }
    }

    public static void ExportFunction(string savepath, UnityEngine.Object[] objects = null)
	{
//		BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
		
		if (objects != null) 
		{
			Selection.objects = objects;
		}
		
		// Build the resource file from the active selection.
		UnityEngine.Object[] selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);

        var buildMap = new AssetBundleBuild[1];
        string assetBundlename = savepath.Substring(savepath.LastIndexOf("/")+1);

        buildMap[0].assetBundleName = assetBundlename;
        int id = 0;
        string [] assets = new string[selection.Length];
        foreach (UnityEngine.Object asset in selection)
        {
            assets[id] = AssetDatabase.GetAssetPath(asset);

            id++;
        }
        buildMap[0].assetNames = assets;
        string outputPath = savepath.Substring(0, savepath.LastIndexOf("/"));

        BuildPipeline.BuildAssetBundles(outputPath, buildMap, BuildAssetBundleOptions.ForceRebuildAssetBundle, EditorUserBuildSettings.activeBuildTarget);
    }
}