using UnityEngine;
using UnityEditor;
using System.Xml;
using System.IO;

public class VWImportProcessor : AssetPostprocessor {

	private int countPRTags = 0;

    public static XmlDocument GeometryConfigXML;

    private static XmlElement _root;

    [MenuItem("VWImporter/Init GeoConfig")]
    public static void Init()
    {
        GeometryConfigXML = new XmlDocument();
        _root = GeometryConfigXML.AppendChild(GeometryConfigXML.CreateElement("NodeItems")) as XmlElement;

        Debug.Log(GeometryConfigXML);
    }

    [MenuItem("VWImporter/Write Test Data")]
    public static void WriteTestData()
    {
        string assetPathXML = "Assets/TempFBX/GeometryConfig.xml";
        GeometryConfigXML.Save(assetPathXML);
        GeometryConfigXML = null;
    }

    void OnPostprocessModel(GameObject g)
    {
        if (GeometryConfigXML == null)
        {
     //       Debug.Log("Skipped Processing, because it is not the structure.fbx: " + g.name);
            return;
        }

        Debug.Log("Start Process: " + g.name);

        CreateGeoConfigXML(g.transform, _root);
    }

    private void CreateGeoConfigXML(Transform t, XmlElement currentNode)
    {
        NodeItem ni = t.GetComponent<NodeItem>();
        XmlElement node = currentNode.AppendChild(GeometryConfigXML.CreateElement("Transform")) as XmlElement;
        if (ni != null)
        {
            XmlElement nodeItem = node.AppendChild(GeometryConfigXML.CreateElement("NodeItem")) as XmlElement;
            nodeItem.AppendChild(GeometryConfigXML.CreateElement("Code")).InnerText = ni.prTags;
            string originalName = ni.originalObjectName;
            nodeItem.AppendChild(GeometryConfigXML.CreateElement("Name")).InnerText = originalName;
            nodeItem.AppendChild(GeometryConfigXML.CreateElement("FileLink")).InnerText = ni.fileLink;
        }

        node.AppendChild(GeometryConfigXML.CreateElement("GameObjectName")).InnerText = t.name;
        node.AppendChild(GeometryConfigXML.CreateElement("TransformLocalPosition")).InnerText = t.transform.localPosition.ToString("0.0000");
        node.AppendChild(GeometryConfigXML.CreateElement("TransformLocalRotation")).InnerText = t.transform.localRotation.ToString("0.0000");
        node.AppendChild(GeometryConfigXML.CreateElement("TransformLocalScale")).InnerText = t.transform.localScale.ToString("0.0000");
        node.AppendChild(GeometryConfigXML.CreateElement("TransformPosition")).InnerText = t.transform.position.ToString("0.0000");
        node.AppendChild(GeometryConfigXML.CreateElement("TransformRotation")).InnerText = t.transform.rotation.ToString("0.0000");
        node.AppendChild(GeometryConfigXML.CreateElement("TransformScale")).InnerText = t.transform.lossyScale.ToString("0.0000");

        foreach(Transform c in t)
        {
            CreateGeoConfigXML(c, node);
        }
    }

    void OnPostprocessGameObjectWithUserProperties(GameObject gameObject, string[] attributeNames, System.Object[] attributeValues)
    {
        if (GeometryConfigXML == null)
        {
            Debug.Log("Skipped Processing, because it is not the structure.fbx: " + gameObject.name);
            return;
        }

        // read meta Data
        string currentPRTag = "";
        NodeItem node = null;
        if (attributeNames.Length > 0)
        {
            node = gameObject.AddComponent<NodeItem>();
        }
        for (int i = 0; i < attributeNames.Length; i++)
        {
            if (attributeNames[i] == "PR_TAGS")
            {
                currentPRTag = (string)attributeValues[i];

                countPRTags++;
            }
            else if (attributeNames[i] == "rttObjectName")
            {
                node.prTags = currentPRTag;
                string originalName = (string)attributeValues[i];
                node.originalObjectName = originalName;

                //Write FileLinks
                if (originalName.Contains("(file)") && gameObject.name.Contains("csb"))
                {

                    string filename = originalName.Replace("(file)", "").Trim();
                    filename = filename.Replace(",", "_").Trim();
                    filename = filename.Replace(" ", "_").Trim();
                    filename = filename.Replace(".csb", "");


//                    string modelName = Path.GetFileNameWithoutExtension(assetPath);
                    node.fileLink = filename;

                }
            }
        }
    }
}


















