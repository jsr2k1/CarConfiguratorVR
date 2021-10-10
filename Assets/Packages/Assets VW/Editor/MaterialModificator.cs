
using UnityEngine;
using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class MaterialModificator
{
	private string _materialName;
	public string MaterialName
	{
		get
		{
			return _materialName;
		}
		set
		{
			_materialName = value;
		}
	}

	#pragma warning disable 0436

	private List<Modificator> _modificatorList;

	#pragma warning restore 0436

	public MaterialModificator(XmlNode materialXmlNode)
	{
		_materialName = materialXmlNode.Attributes["name"].InnerText;

		_modificatorList = LoadMaterialModificators(materialXmlNode);
	}

	private List<Modificator> LoadMaterialModificators(XmlNode materialXmlNode)
	{
		List<Modificator> modificatorList = new List<Modificator>();

		XmlNodeList modificatorXmlNodeList = materialXmlNode.SelectNodes("Attribute");
		foreach(XmlNode modificatorXmlNode in modificatorXmlNodeList)
		{
			modificatorList.Add(Modificator.CreateModificatorContainer(modificatorXmlNode));
		}

		return modificatorList;
	}

	private Modificator FindMaterialModificator(string name, string subType = "", string attributeName = "")
	{
		Modificator returnValue = null;

		if (!string.IsNullOrEmpty(name))
		{
//			Debug.Log ("Searching for: " + name + " s " + subType + " a " + attributeName);

			foreach(Modificator modificator in _modificatorList)
			{
				if ((modificator.Name == name) && (modificator.SubType == subType) && (modificator.AttributeName == attributeName))
				{
					returnValue = modificator;
					break;
				}
			}
		}
		
		return returnValue;
	}

	public bool HasModificator(string name, string subType = "", string attributeName = "")
	{
		bool returnValue = false;

		foreach(Modificator modificator in _modificatorList)
		{
//			Debug.Log ("mod has: " + modificator.Name + " subtype: " + modificator.SubType + " attrib: " + modificator.AttributeName);

			if ((modificator.Name == name) && (modificator.SubType == subType) && (modificator.AttributeName == attributeName))
			{
				returnValue = true;
				break;
			}
		}

		return returnValue;
	}

	public void ApplyModificator(string name, ref float value, string subType = "", string attributeName = "")
	{
		Modificator attributeModificator = FindMaterialModificator(name, subType, attributeName);

		if (attributeModificator == null)
		{
			return;
		}
		
//		Debug.Log ("GOT mod for: " + name);
//		Debug.Log(attributeModificator.FloatValue);

		attributeModificator.ApplyModificatorToValue(ref value);
	}

	public void ApplyModificator(string name, ref string value, string subType = "", string attributeName = "")
	{
		Modificator attributeModificator = FindMaterialModificator(name, subType, attributeName);

		if (attributeModificator == null)
		{
			return;
		}
		
		attributeModificator.ApplyModificatorToValue(ref value);
	}
}
