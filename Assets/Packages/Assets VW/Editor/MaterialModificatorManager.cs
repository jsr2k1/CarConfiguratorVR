using UnityEngine;

using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

using System.Text.RegularExpressions;

public class MaterialModificatorManager
{
	#pragma warning disable 0436
	private static MaterialModificatorManager _instance;
	#pragma warning restore 0436
	public static MaterialModificatorManager Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = new MaterialModificatorManager();
			}
			return _instance;
		}
	}

	private List<Modificator> _globalModificatorList;
	#pragma warning disable 0436
	private Dictionary<string, MaterialModificator> _materialModificatorDictionary;
	#pragma warning restore 0436

	private MaterialModificatorManager()
	{
		_globalModificatorList = new List<Modificator>();
		_materialModificatorDictionary = new Dictionary<string, MaterialModificator>();
	}

	public void Init(string path)
	{
		LoadMaterialsXml(path);
	}

	private void LoadMaterialsXml(string path)
	{
		string fileName = string.Concat(path, "/materials.xml");

		StringBuilder sb = new StringBuilder();
		using (StreamReader sr = new StreamReader(fileName)) {

			string line;
			int lineNumber = 1;
			while ((line = sr.ReadLine()) != null) {

				string xmlCommentStartString = "<!--";
				string xmlCommentEndString = "-->";

				while (line.Contains(xmlCommentStartString)) {

//					Debug.Log("line " + lineNumber + " contains comment: " + line);
					int startIndex = line.IndexOf(xmlCommentStartString);
					int endIndex = line.IndexOf(xmlCommentEndString);
					int length = endIndex + xmlCommentEndString.Length - startIndex;
					line = line.Remove(startIndex, length);

//					Debug.Log("xml comment removed. resulting line: " + line);
				}
				// debug.log auskommentiert scheint zu passen. mit debug.log gibt's den fehler mit E_gRU003_Tire_Rubber_BlackSidewall
				// race condition?
				// was passiert evtl. schon während loadmaterialsxml?
				sb.AppendLine(line);
				lineNumber++;
			}
		}

		string documentString = sb.ToString();

		XmlDocument document = new XmlDocument();

		document.LoadXml(documentString);
//		document.Load(string.Concat(path, "/materials.xml"));

		LoadGlobalMaterialModificators(document);
		LoadMaterialSpecificModificators(document);
	}

	private void LoadGlobalMaterialModificators(XmlDocument document)
	{
		_globalModificatorList.Clear();

		XmlNodeList globalModifiersXmlNodeList = document.SelectNodes("Materials/Global/Attribute");
		foreach(XmlNode globalModifierXmlNode in globalModifiersXmlNodeList)
		{
			_globalModificatorList.Add(CreateGlobalModificator(globalModifierXmlNode));
		}
	}

	private void LoadMaterialSpecificModificators(XmlDocument document)
	{
		_materialModificatorDictionary.Clear();

		XmlNodeList materialXmlNodeList = document.SelectNodes("Materials/Material");
		foreach(XmlNode materialXmlNode in materialXmlNodeList)
		{
			MaterialModificator materialModificator = new MaterialModificator(materialXmlNode);

			if (_materialModificatorDictionary.ContainsKey(materialModificator.MaterialName)) {

				Debug.LogError("Material already exists in Dictionary: " + materialModificator.MaterialName);
				continue;
			}
			_materialModificatorDictionary.Add(materialModificator.MaterialName, materialModificator);
		}
	}

	private Modificator CreateGlobalModificator(XmlNode globalModifierNode)
	{
		return Modificator.CreateModificatorContainer(globalModifierNode);
	}

	private Modificator FindGlobalModificator(string name, string subType = "", string attributeName = "")
	{
		Modificator returnValue = null;

//		if (!string.IsNullOrEmpty(attributeName))
//		{
			foreach (Modificator modificator in _globalModificatorList)
			{
				if ((modificator.Name == name) && (modificator.SubType == subType) && (modificator.AttributeName == attributeName))
				{
					returnValue = modificator;
					break;
				}
			}
//		}

		return returnValue;
	}

	private MaterialModificator FindMaterialModificator(string materialName)
	{
		MaterialModificator returnValue = null;

		if(_materialModificatorDictionary.ContainsKey(materialName))
		{
//			Debug.Log("got modificator for material: " + materialName);

			returnValue = _materialModificatorDictionary[materialName];
		}
		else
		{
//			Debug.Log("looking for wildcard");

			returnValue = FindWildcardModificator(materialName);
		}

		return returnValue;
	}

	private MaterialModificator FindWildcardModificator(string materialName)
	{
		foreach (KeyValuePair<string, MaterialModificator> pair in _materialModificatorDictionary)
		{
			if (pair.Key.Contains("*") == false)
			{
				// skip if no wildcard modifier
				continue;
			}

			string pattern = pair.Key.Replace("*", ".*?");

			if (Regex.IsMatch(materialName, pattern) == true)
			{
//				Debug.Log ("regular expression match: " + materialName + " pattern " + pattern);
//				Debug.Log ("regular expression match: " + pair.Value.MaterialName);

				return pair.Value;
			}
		}

		return null;
	}

	public bool HasGlobalModificator(string name, string subType = "", string attributeName = "")
	{
		bool returnValue = false;

		Modificator modificator = FindGlobalModificator(name, subType, attributeName);

		if (modificator != null)
		{
			returnValue = true;
		}

		return returnValue;
	}

	public bool HasMaterial(string materialName)
	{
		return _materialModificatorDictionary.ContainsKey(materialName);
	}

	public bool HasMaterialSpecificModificator(string materialName, string name, string subType = "", string attributeName = "")
	{
		bool returnValue = false;

		if (_materialModificatorDictionary.ContainsKey(materialName))
		{
			if (_materialModificatorDictionary[materialName].HasModificator(name, subType, attributeName))
			{
				returnValue = true;
			}
		}
		else
		{
			returnValue = HasMaterialSpecificWildcardModificator(materialName, name, subType, attributeName);

//			Debug.Log ("HAS MODIFICATOR: " + materialName + " | " + name + " | " + returnValue);
		}

		return returnValue;
	}

	private bool HasMaterialSpecificWildcardModificator(string materialName, string name, string subType = "", string attributeName = "")
	{
		bool returnValue = false;

		MaterialModificator modifier = FindWildcardModificator(materialName);

		if (modifier != null)
		{
			if (modifier.HasModificator(name, subType, attributeName))
			{
				returnValue = true;
			}
		}

		return returnValue;
	}

	private bool HasMaterialSpecificWildcardModificator(string materialName)
	{
		bool returnValue = false;
		
		if(FindWildcardModificator(materialName) != null)
		{
			returnValue = true;
		}
		
		return returnValue;
	}

	public void ApplyGlobalModificator(string name, ref float value, string subType = "", string attributeName = "")
	{
		Modificator attributeModificator = FindGlobalModificator(name, subType, attributeName);

		if (attributeModificator == null)
		{
			return;
		}

		attributeModificator.ApplyModificatorToValue(ref value);
	}

	public void ApplyGlobalModificator(string name, ref string value, string subType = "", string attributeName = "")
	{
		Modificator attributeModificator = FindGlobalModificator(name, subType, attributeName);

		if (attributeModificator == null)
		{
			return;
		}

		attributeModificator.ApplyModificatorToValue(ref value);
	}

	public void ApplyMaterialModificator(string materialName, string name, ref float value, string subType = "", string attributeName = "")
	{
		MaterialModificator materialModificator = FindMaterialModificator(materialName);

		if (materialModificator == null)
		{
			return;
		}

		materialModificator.ApplyModificator(name, ref value, subType, attributeName);
	}

	public void ApplyMaterialModificator(string materialName, string name, ref string value, string subType = "", string attributeName = "")
	{
		MaterialModificator materialModificator = FindMaterialModificator(materialName);

		if (materialModificator == null)
		{
			return;
		}

		materialModificator.ApplyModificator(name, ref value, subType, attributeName);
	}
}
