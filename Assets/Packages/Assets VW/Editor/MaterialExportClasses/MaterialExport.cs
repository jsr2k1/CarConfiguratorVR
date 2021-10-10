
using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Xml;

// Änderungen in die Unity 5 Version übernehmen.

public class MaterialExport 
{
	protected string _MaterialName;

	public virtual Material CreateMaterial(XmlDocument doc, string path)
	{
		return null;
	}

	protected Color GetGammaColor(XmlDocument doc, string nameR, string nameG, string nameB, float defaultR=0, float defaultG=0, float defaultB=0)
	{
		Color c = Color.black;

		if (nameR != null) {
			c.r = GetFloat(doc, nameR, defaultR);
		}

		if (nameG != null) {
			c.g = GetFloat(doc, nameG, defaultG);	
		}

		if (nameB != null) {
			c.b = GetFloat(doc, nameB, defaultB);	
		}

		return c.gamma;
	}

	protected float GetFloat(XmlDocument doc, string name, float defaultValue=0)
	{
		float value = defaultValue;
		XmlNodeList tags = doc.GetElementsByTagName (name);

		try
		{
			if (tags.Count > 0)
			{
				value = System.Convert.ToSingle (tags[0].InnerText);
			}
		}
		catch(System.Exception e)
		{
			Debug.Log(name+": "+e.Message);
		}

//		Debug.Log(name + " = " + value + " mat: " + _MaterialName);

		if(MaterialModificatorManager.Instance.HasGlobalModificator(name))
		{
			MaterialModificatorManager.Instance.ApplyGlobalModificator(name, ref value);
//			Debug.Log("Global parameter changed! AttributeName: " + name + " New value: " + value.ToString());
		}
		
		if(MaterialModificatorManager.Instance.HasMaterialSpecificModificator(_MaterialName, name))
		{
			MaterialModificatorManager.Instance.ApplyMaterialModificator(_MaterialName, name, ref value);
//			Debug.Log("Material specific parameter changed! MaterialName: " + _MaterialName + "AttributeName: " + name + " New value: " + value.ToString());
		}

		return value;
	}
	
	protected string GetName(XmlDocument doc, string name, string type)
	{
		string value = string.Empty;
		XmlNodeList tags = doc.GetElementsByTagName(name);
		try
		{
			if (tags.Count > 0) 
			{	
				foreach (XmlNode node in tags) 
				{
					if (node.Attributes["type"].InnerText == type)
					{
						value = node.Attributes["name"].InnerText;
						break;
					}
				}
			}
		}
		catch (System.Exception e)
		{
			Debug.Log(name + ": " + e.Message);
		}

		if(MaterialModificatorManager.Instance.HasGlobalModificator(name))
		{
			MaterialModificatorManager.Instance.ApplyGlobalModificator(name, ref value);
		}

		if(MaterialModificatorManager.Instance.HasMaterialSpecificModificator(_MaterialName, name, type))
		{
			MaterialModificatorManager.Instance.ApplyMaterialModificator(_MaterialName, name, ref value, type);
		}

		return value;
	}

	protected string GetName(XmlDocument doc, string name)
	{
		string value = string.Empty;
		XmlNodeList tags = doc.GetElementsByTagName(name);
		try
		{
			if (tags.Count > 0) 
			{	
				foreach (XmlNode node in tags) 
				{
					value = node.Attributes["name"].InnerText;
					break;
				}
			}
		}
		catch (System.Exception e)
		{
			Debug.Log(name + ": " + e.Message);
		}
		
		if(MaterialModificatorManager.Instance.HasGlobalModificator(name))
		{
			MaterialModificatorManager.Instance.ApplyGlobalModificator(name, ref value);
		}
		
		if(MaterialModificatorManager.Instance.HasMaterialSpecificModificator(_MaterialName, name))
		{
			MaterialModificatorManager.Instance.ApplyMaterialModificator(_MaterialName, name, ref value);
		}
		
		return value;
	}

	protected float GetFloatFromType(XmlDocument doc, string name, int position, string type, string attribute, float defaultValue=0f)
	{
		float value = defaultValue;
		XmlNodeList tags = doc.GetElementsByTagName (name);
		try
		{
			if (tags.Count > 0) 
			{	
				foreach (XmlNode node in tags) 
				{
					if (node.Attributes["type"].InnerText == type)
					{
						value = System.Convert.ToSingle(node.Attributes[attribute].InnerText.Split(new char[]{' '})[position]);
						break;
					}
				}
			}
		}
		catch (System.Exception e)
		{
			Debug.Log(name+": "+e.Message);
		}

		string modValue = string.Empty;

		if (MaterialModificatorManager.Instance.HasGlobalModificator(name, type, attribute))
		{
			MaterialModificatorManager.Instance.ApplyGlobalModificator(name, ref modValue, type, attribute);
		}	

		if (MaterialModificatorManager.Instance.HasMaterialSpecificModificator(_MaterialName, name, type, attribute))
		{
			MaterialModificatorManager.Instance.ApplyMaterialModificator(_MaterialName, name, ref modValue, type, attribute);
		}

		if (modValue != string.Empty)
		{
			value = System.Convert.ToSingle(modValue.Split(new char[]{' '})[position]);
		}

		return value;
	}

	protected string GetMaterialName(XmlDocument doc, float defaultR=0, float defaultG=0, float defaultB=0)
	{
		return doc.SelectSingleNode("/Material/Name").InnerText;
	}

	protected Color GetDiffuseColor(XmlDocument doc, float defaultR=0, float defaultG=0, float defaultB=0)
	{
		return GetGammaColor(doc, "diffuseR", "diffuseG", "diffuseB", defaultR, defaultG, defaultB);
	}

	protected Color GetReflectionColor(XmlDocument doc, float defaultR=0, float defaultG=0, float defaultB=0)
	{
		return GetGammaColor(doc, "reflectionR", "reflectionG", "reflectionB", defaultR, defaultG, defaultB);
	}

	protected Color GetTransparentColor(XmlDocument doc, float defaultR=0, float defaultG=0, float defaultB=0)
	{
		return GetGammaColor(doc, "transparencyR", "transparencyG", "transparencyB", defaultR, defaultG, defaultB);
	}

	protected float GetGlossiness(XmlDocument doc)
	{
		return GetFloat(doc, "glossiness");
	}

	protected float GetReflectivity(XmlDocument doc)
	{
		return GetFloat(doc, "reflectivity");
	}

	protected float GetTransparency(XmlDocument doc)
	{
		if (HasProperty(doc, "transparency"))
		{
			return GetFloat(doc, "transparency");
		}

		return 0.0f;	// no transparency
	}

	protected Color GetFlakeColor(XmlDocument doc, float defaultR=0, float defaultG=0, float defaultB=0)
	{
		return GetGammaColor(doc, "reflectionR", "reflectionG", "reflectionB", defaultR, defaultG, defaultB);
	}
	
	protected string GetDiffuseTextureName(XmlDocument doc)
	{
//		Debug.Log("GetDiffuseTextureName" + doc);
		return GetName(doc , "texture", "diffuse");
	}

	protected string GetSpecularTextureName(XmlDocument doc)
	{	
//		Debug.Log("GetSpecularTextureName" + doc);
		return GetName(doc , "texture", "specular");
	}

	protected string GetNormalTextureName(XmlDocument doc)
	{		
//		Debug.Log("GetNormalTextureName" + doc);
		return GetName(doc , "texture", "normal");
	}

	protected string GetBumpTextureName(XmlDocument doc)
	{		
		return GetName(doc , "texture", "bump");
	}

	protected float  GetTextureTilingU(XmlDocument doc, string type, float defaultValue=0f)
	{
		return GetFloatFromType(doc , "texture", 0, type, "repeatUV", defaultValue );
	}

	protected float  GetTextureTilingV(XmlDocument doc, string type, float defaultValue=0f)
	{
		return GetFloatFromType(doc , "texture", 1, type, "repeatUV", defaultValue);
	}

	protected float  GetTextureOffsetU(XmlDocument doc, string type, float defaultValue=0f)
	{
		return GetFloatFromType(doc , "texture", 0, type, "offsetUV", defaultValue );
	}
	
	protected float  GetTextureOffsetV(XmlDocument doc, string type, float defaultValue=0f)
	{
		return GetFloatFromType(doc , "texture", 1, type, "offsetUV", defaultValue);
	}

    protected float GetTextureRotation(XmlDocument doc, string type, float defaultValue = 0f)
    {
        return GetFloatFromType(doc, "texture", 0, type, "rotateFrame", defaultValue);
    }

    protected Texture ImportTexture(string path, string type, float rotation = 0.0f)
	{
		if (type == "bump")
		{
			// use normal maps instead of height (=bump) maps as we do not have the bump parameters
			path = path.Replace("_h", "_n");	
		}

		string file = Path.GetFileName(path);

		if (Directory.Exists(ExportMaterials.TextureFolderPath) == false)
        {
            Directory.CreateDirectory(ExportMaterials.TextureFolderPath);
        }

        if (File.Exists(path) == false) {

			Debug.Log("file not found: " + path);
			return null;
		}

		File.Copy(path, ExportMaterials.TextureFolderPath + "/" + file, true);

        AssetDatabase.Refresh();

        //		Debug.Log("file: " + file);
        FileInfo fileInfo = new FileInfo(file);
		if (new FileInfo(file).Extension == ".rgb") return null;

		if (fileInfo.Extension == ".hdr")
		{
			return null;
		}

		bool importTexture = true;

		if (fileInfo.Extension == ".dds")
		{
			importTexture = false;
		}

		if (importTexture == true)
		{
            ImportTextureAssetFromPath(ExportMaterials.TextureFolderPath + "/" + file, type);
		}

        Texture2D texture = (Texture2D)AssetDatabase.LoadAssetAtPath(ExportMaterials.TextureFolderPath + "/" + file, typeof(UnityEngine.Texture));

        if (rotation > 0.0f)
        {
            texture = CreateRotatedTexture(texture, type, rotation, ExportMaterials.TextureFolderPath + "/" + file);
        }

        return texture;
	}

    private Texture2D CreateRotatedTexture(Texture2D texture, string type, float rotation, string filePath)
    {
        bool flipX = false;
        bool flipY = false;
        bool swapXY = false;

        switch ((int)rotation)
        {
            case 90:
            case -270:
                swapXY = true;
                flipY = true;
                break;
            case 180:
            case -180:
                flipX = true;
                break;
            case 270:
            case -90:
                swapXY = true;
                break;
        }

        Texture2D rotatedTexture;

        if (swapXY == true)
        {
            rotatedTexture = new Texture2D(texture.height, texture.width);
        }
        else
        {
            rotatedTexture = new Texture2D(texture.width, texture.height);
        }

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                int destX = x;
                int destY = y;

                if (swapXY == true)
                {
                    destX = y;
                    destY = x;
                }

                if (flipX == true)
                {
                    destX = (rotatedTexture.width - 1) - destX;
                }

                if (flipY == true)
                {
                    destY = (rotatedTexture.height- 1) - destY;
                }

                rotatedTexture.SetPixel(destX, destY, texture.GetPixel(x, y));
            }
        }

        byte[] pngFile = rotatedTexture.EncodeToPNG();

        string rotatedTextureName = filePath;
        rotatedTextureName = rotatedTextureName.Substring(0, rotatedTextureName.LastIndexOf("."));
        rotatedTextureName += "_rot.png";
        File.WriteAllBytes(rotatedTextureName, pngFile);

        AssetDatabase.Refresh();

        ImportTextureAssetFromPath(rotatedTextureName, type);

        texture = (Texture2D)AssetDatabase.LoadAssetAtPath(rotatedTextureName, typeof(UnityEngine.Texture));

        return texture;
    }

    private void ImportTextureAssetFromPath(string filePath, string type)
    {
        AssetDatabase.ImportAsset(filePath);

		TextureImporter textureImporter = AssetImporter.GetAtPath(filePath) as TextureImporter;

        textureImporter.textureType = TextureImporterType.Default;

		if (type == "bump" || type == "normal")
		{
			textureImporter.textureType = TextureImporterType.NormalMap;
			textureImporter.anisoLevel = 9;
		}
		else if (type == "lightmap")
		{
			textureImporter.textureType = TextureImporterType.Lightmap;
		}
		else if (type == "texture")
		{
			textureImporter.anisoLevel = 9;
		}

		textureImporter.maxTextureSize = 4096;

        textureImporter.isReadable = true;

		AssetDatabase.ImportAsset(filePath);
    }

    protected bool HasTexture(XmlDocument doc, string type)
	{
		if (GetName(doc, "texture", type) != string.Empty)
		{
			return true;
		}

		return false;
	}

	protected bool HasProperty(XmlDocument doc, string name)
	{
		XmlNodeList tags = doc.GetElementsByTagName (name);

		try
		{
			if (tags.Count > 0)
			{
				return true;
			}
			else if (MaterialModificatorManager.Instance.HasMaterialSpecificModificator(_MaterialName, name))
			{
				return true;
			}
		}
		catch (System.Exception e)
		{
			Debug.Log(name+": "+e.Message);
		}

		return false;
	}

	protected bool HasPropertyAttribute(XmlDocument doc, string name, string type, string attribute)
	{
//		bool result = false;

		XmlNodeList tags = doc.GetElementsByTagName (name);
		try
		{
			if (tags.Count > 0) 
			{	
				foreach (XmlNode node in tags) 
				{
					if (node.Attributes["type"].InnerText == type)
					{
						if (node.Attributes[attribute] != null)
						{
							return true;
						}
					}
				}
			}
		}
		catch (System.Exception e)
		{
			Debug.Log(name+": "+e.Message);
		}
		
		if (MaterialModificatorManager.Instance.HasGlobalModificator(name, type, attribute))
		{
			return true;
		}	
		
		if (MaterialModificatorManager.Instance.HasMaterialSpecificModificator(_MaterialName, name, type, attribute))
		{
			return true;
		}		

		return false;
	}

	protected Color GetEmissiveColor(XmlDocument doc, float defaultR=0, float defaultG=0, float defaultB=0)
	{
		return GetGammaColor(doc, "emissiveR", "emissiveG", "emissiveB", defaultR, defaultG, defaultB);
	}
	
	protected bool HasDiffuseColorModfifier(XmlDocument doc)
	{
		if (HasModfifierForValue(doc, "diffuseR"))
		{
			return true;
		}
		if (HasModfifierForValue(doc, "diffuseG"))
		{
			return true;
		}
		if (HasModfifierForValue(doc, "diffuseB"))
		{
			return true;
		}

		return false;
	}

	protected bool HasModfifierForValue(XmlDocument doc, string name)
	{
		if (MaterialModificatorManager.Instance.HasGlobalModificator(name))
		{
			return true;
		}
		
		if (MaterialModificatorManager.Instance.HasMaterialSpecificModificator(_MaterialName, name))
		{
			return true;
		}

		return false;
	}
}