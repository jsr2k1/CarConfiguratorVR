using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.Sprites;

public class CustomImportSettings : AssetPostprocessor 
{
	//MODELS
	void OnPreprocessModel()
	{
		ModelImporter modelImporter = assetImporter as ModelImporter;
		modelImporter.materialSearch = ModelImporterMaterialSearch.Everywhere;
		modelImporter.materialName = ModelImporterMaterialName.BasedOnMaterialName;
		modelImporter.importTangents = ModelImporterTangents.None;
		modelImporter.animationType = ModelImporterAnimationType.Legacy;

//		if(assetPath.Contains("COTXES")) {
//			modelImporter.materialName = ModelImporterMaterialName.BasedOnMaterialName;
//		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//TEXTURES
	void OnPreprocessTexture()
	{
		TextureImporter textureImporter = assetImporter as TextureImporter;

		if(assetPath.Contains("UI")){
			textureImporter.textureType = TextureImporterType.Sprite;
			textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
			textureImporter.mipmapEnabled = false;
			textureImporter.spriteImportMode = SpriteImportMode.Single;
		}
//
//		textureImporter.textureType = TextureImporterType.Sprite;
//		textureImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
//		textureImporter.mipmapEnabled = false;
//		textureImporter.filterMode = FilterMode.Point;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	//AUDIOS
	void OnPreprocessAudio()
	{
//		AudioImporter audioImporter = assetImporter as AudioImporter;
//		audioImporter.threeD = false;
	}
}