using UnityEngine;
using UnityEditor;
using System.Xml;
using System.IO;
using System;

//public class VWImportProcessor2 : AssetPostprocessor {

//	private int countPRTags = 0;

//    public static XmlDocument GeometryConfigXML;

//    private static XmlElement _root;

//    private static XmlElement _currentParent;

//    private static string goNAMES;

//    //[MenuItem("VWImporter/Init GeoConfig")]
//    //public static void Init()
//    //{
//    //    GeometryConfigXML = new XmlDocument();
//    //    _root = GeometryConfigXML.AppendChild(GeometryConfigXML.CreateElement("NodeItems")) as XmlElement;
//    //    goNAMES = string.Empty;
//    //}

//    //[MenuItem("VWImporter/Write Test Data")]
//    //public static void WriteTestData()
//    //{
//    //    File.WriteAllText(Application.streamingAssetsPath+"/AllNames.txt", goNAMES);
//    //}

//    void OnPostprocessGameObjectWithUserProperties(GameObject gameObject, string[] attributeNames, System.Object[] attributeValues)
//    {
//        if (GeometryConfigXML == null)
//        {
////            Debug.Log("Skipped Processing, because it is not the structure.fbx: " + gameObject.name);
//            return;
//        }

//        //goNAMES += gameObject.name + "\n";

//        // read meta Data
//        string currentPRTag = "";

//        for (int i = 0; i < attributeNames.Length; i++)
//		{
//            if (attributeNames[i] == "PR_TAGS")
//            {
//                currentPRTag = (string)attributeValues[i];

//                countPRTags++;
//            }
//            else if (attributeNames[i] == "rttObjectName")
//            {
//                //Debug.Log("Length: " + attributeNames.Length);
//                //		Debug.Log("rttObjectName: " + (string)attributeValues[i]);
//                //  configDataDict[currentPRTag].Add((string)attributeValues[i]);
//                if (string.IsNullOrEmpty(currentPRTag) == false)
//                {
//                    string path = GetPath(gameObject);
//                    XmlNode parentX = GeometryConfigXML.SelectSingleNode(path);

//                    if (parentX == null)
//                    {
//                        Debug.LogError("Nodeitem path not found:"+path);
//                        parentX = _root;
//                    }

//                    XmlElement node = parentX.AppendChild(GeometryConfigXML.CreateElement(gameObject.name.Replace(" ", "/"))) as XmlElement;
//                    node.AppendChild(GeometryConfigXML.CreateElement("Code")).InnerText = currentPRTag;
//                    string originalName = (string)attributeValues[i];
//                    node.AppendChild(GeometryConfigXML.CreateElement("Name")).InnerText = originalName;

//                    //Write FileLinks
//                    if (originalName.Contains("(file)") && gameObject.name.Contains("csb"))
//                    {

//                        string filename = originalName.Replace("(file)", "").Trim();
//                        filename = filename.Replace(",", "_").Trim();
//                        filename = filename.Replace(" ", "_").Trim();
//                        filename = filename.Replace(".csb", "");


//                        string modelName = Path.GetFileNameWithoutExtension(assetPath);
//                        node.AppendChild(GeometryConfigXML.CreateElement("FileLink")).InnerText = filename;

//                    }
//                    node.AppendChild(GeometryConfigXML.CreateElement("GameObjectName")).InnerText = gameObject.name;
//                    node.AppendChild(GeometryConfigXML.CreateElement("TransformLocalPosition")).InnerText = gameObject.transform.localPosition.ToString();
//                    node.AppendChild(GeometryConfigXML.CreateElement("TransformLocalRotation")).InnerText = gameObject.transform.localRotation.ToString();
//                    node.AppendChild(GeometryConfigXML.CreateElement("TransformLocalScale")).InnerText = gameObject.transform.localScale.ToString();
//                    node.AppendChild(GeometryConfigXML.CreateElement("TransformPosition")).InnerText = gameObject.transform.position.ToString();
//                    node.AppendChild(GeometryConfigXML.CreateElement("TransformRotation")).InnerText = gameObject.transform.rotation.ToString();
//                    node.AppendChild(GeometryConfigXML.CreateElement("TransformScale")).InnerText = gameObject.transform.lossyScale.ToString();

//                    node.AppendChild(GeometryConfigXML.CreateElement("Path")).InnerText = path;

//                }
//                else
//                {
//                    string path = GetPath(gameObject);
//                    XmlNode parentX;
//                    try
//                    {
//                        parentX = GeometryConfigXML.SelectSingleNode(path);
//                    }
//                    catch(Exception e)
//                    {
//                        parentX = _root;
//             //           Debug.LogError("Transform path not found:" + path);
//                    }

//                    if (parentX == null)
//                        parentX = _root;
//                    //else
//                    //{
//                    //    Debug.Log("Written Transform " + path+"/"+gameObject.name);
//                    //}
//                    XmlElement node = parentX.AppendChild(GeometryConfigXML.CreateElement(gameObject.name.Replace(" ", "_"))) as XmlElement;
//             //       node.SetAttribute("name", gameObject.name);
//                    node.SetAttribute("TransformLocalPosition", gameObject.transform.localPosition.ToString());
//                    node.SetAttribute("TransformLocalRotation", gameObject.transform.localRotation.ToString());
//                    node.SetAttribute("TransformLocalScale", gameObject.transform.localScale.ToString());
//                    node.SetAttribute("TransformPosition", gameObject.transform.position.ToString());
//                    node.SetAttribute("TransformRotation", gameObject.transform.rotation.ToString());
//                    node.SetAttribute("TransformScale", gameObject.transform.lossyScale.ToString());
//                }
//            }
//        }
//	}

//    private static string GetPath(GameObject gameObject)
//    {
//        string path = "";
//        Transform parent = gameObject.transform.parent;

//        while (parent != null)
//        {
//            path = parent.name + "/" + path;
//            parent = parent.parent;
//        }

//        // add root item to path
//        path = _root.Name + "/" + path;

//        // we get an error when path has / at end
//        if (path.EndsWith("/"))
//            path = path.TrimEnd('/');

//        // path must be xml correct
//        path = path.Replace(" ", "_");

//        return path;
//    }
//}















