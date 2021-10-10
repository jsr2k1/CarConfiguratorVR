using UnityEngine;

using System;
using System.Xml;

public class Modificator 
{
	private string _name;
	public string Name
	{
		get
		{
			return _name;
		}
	}
	
	private string _subType;
	public string SubType
	{
		get
		{
			return _subType;
		}
	}

	private string _attributeName;
	public string AttributeName
	{
		get
		{
			return _attributeName;
		}
	}

	private string _functionName;
	public string FunctionName
	{
		get
		{
			return _functionName;
		}
	}

	private object _value;
	public float FloatValue
	{
		get
		{
			return  Convert.ToSingle(_value);
		}
	}

	public string StringValue
	{
		get
		{
			return Convert.ToString(_value);
		}
	}

	public Modificator(string name, string functionName, object value, string subType, string attributeName)
	{
		_name = name;
		_attributeName = attributeName;
		_subType = subType;
		_functionName = functionName;
		_value = value;
	}

	public static Modificator CreateModificatorContainer(XmlNode modificatorXmlNode)
	{
		string name = modificatorXmlNode.Attributes["name"].InnerText;
		string subtype = "";
		if (modificatorXmlNode.Attributes["subtype"] != null)
		{
			subtype = modificatorXmlNode.Attributes["subtype"].InnerText;
		}
		string attribute = "";
		if (modificatorXmlNode.Attributes["attribute"] != null)
		{
			attribute = modificatorXmlNode.Attributes["attribute"].InnerText;
		}
		string function = modificatorXmlNode.Attributes["function"].InnerText;
		string value = modificatorXmlNode.InnerText;

//		Debug.Log("reading: " + name + subtype + attribute);

		return new Modificator(name, function, value, subtype, attribute);
	}
	
	public void ApplyModificatorToValue(ref float value)
	{
		switch(_functionName)
		{
			case "set":
				value = FloatValue;
				break;
				
			case "multiply":
				value *= FloatValue;
				break;
		}
	}

	public void ApplyModificatorToValue(ref string value)
	{
		switch(_functionName)
		{
			case "set":
				value = StringValue;
				break;
		}
	}
}