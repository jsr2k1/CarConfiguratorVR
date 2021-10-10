#if UNITY_EDITOR
using UnityEditor;

using UnityEngine;
using System;
using System.Reflection;
using CrashReportDLL;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class ReporterSettings : EditorWindow
{

	private const string REPORTER_PLUGIN_EMAIL_ADRESSES = "reportPluginEmailAdresses";
	public const string REPORTER_PLUGIN_USER_NAME = "reportPluginUserName";
	public const string REPORTER_PLUGIN_VALID_EMAIL_ADRESSES = "reportPluginValidEmailAdresses";
	//public const string REPORTER_PLUGIN_MAX_AUTO_REPORT_SEND_COUNT = "reportPluginMaxAutoReportSendCount";

	
	public static string userName = "";
	public static string validEmailAdresses = "";
    //public static int maxAutoReportSendCount = 1;
    
    private static Report reportObject = null;
    

    static ReporterSettings(){
		userName = Environment.UserName;
    }
	
	public string[] emailAddresssesToSendReport = new string[3];
	//public int maxAutoReportSendCountPerSession = 1;
	
	[MenuItem("Tools/Reporter Settings")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(ReporterSettings));
        EditorPrefs.SetString(REPORTER_PLUGIN_USER_NAME, userName);
        validEmailAdresses = EditorPrefs.GetString(REPORTER_PLUGIN_VALID_EMAIL_ADRESSES, "");
        //RefreshReportGameObject ();
        if(reportObject)
            MonoImporter.SetExecutionOrder(MonoScript.FromMonoBehaviour(reportObject), -100);
    }


    //Run in editor mode
    /*[DrawGizmo(GizmoType.NotInSelectionHierarchy)]
    static void RenderCustomGizmo(Transform objectTransform, GizmoType gizmoType)
    {
        if (!EditorApplication.isPlaying){
            reportObject = Reporter.checkReportGameObject();
            reportObject.checkPreviousSession();
        }
    }*/

    /* Draw the user interface elements in the popup.
	 * Validate the data at the same time.
	*/
    void OnGUI()
	{
		if(userName==string.Empty || userName == null)
		{
			GUILayout.Label ("Please login for settings.", EditorStyles.boldLabel);
			return;
		}
		
		string serialisedEmail = EditorPrefs.GetString (REPORTER_PLUGIN_EMAIL_ADRESSES, ",,");
		//maxAutoReportSendCountPerSession = EditorPrefs.GetInt (REPORTER_PLUGIN_MAX_AUTO_REPORT_SEND_COUNT, 1);

		emailAddresssesToSendReport = serialisedEmail.Split(',');
		GUILayout.Label ("Reporter Settings", EditorStyles.boldLabel);
		ScriptableObject target = this;
		SerializedObject serializedObject = new SerializedObject(target);
		SerializedProperty emailsProperty = serializedObject.FindProperty("emailAddresssesToSendReport");
		//SerializedProperty maxAutoReportSendCountProperty = serializedObject.FindProperty("maxAutoReportSendCountPerSession");

		EditorGUILayout.PropertyField(emailsProperty, true); // True means show children
		serializedObject.ApplyModifiedProperties();
		serialisedEmail = "";
		validEmailAdresses = "";
		string emailCheck = "";
		for (int i=0; i<3 ; i++)
		{
			string email = emailAddresssesToSendReport[i];
			if(email!=null && email!="" && !StringUtil.validateEmail(email))
			{
				emailCheck += (i)+", ";
			}
			else if(email!=null && email!="")
			{
				validEmailAdresses += email + ",";
			}
			serialisedEmail += email + ",";
		}
		if(emailCheck != "")
		{
			GUILayout.Label ("Invalid email address, please check Element: "+emailCheck.Remove(emailCheck.Length-2,2));
		}

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		EditorGUIUtility.labelWidth =230f;
		//EditorGUILayout.PropertyField(maxAutoReportSendCountProperty);
		
		serializedObject.ApplyModifiedProperties();
        //remove the commas at the end
        if (serialisedEmail.Length > 0)
            serialisedEmail = serialisedEmail.Remove (serialisedEmail.Length - 1, 1);
        if (validEmailAdresses.Length > 0)
            validEmailAdresses = validEmailAdresses.Remove (validEmailAdresses.Length - 1, 1);
		//maxAutoReportSendCount = maxAutoReportSendCountPerSession;

		EditorPrefs.SetString (REPORTER_PLUGIN_EMAIL_ADRESSES, serialisedEmail);
		EditorPrefs.SetString (REPORTER_PLUGIN_VALID_EMAIL_ADRESSES, validEmailAdresses);
		//EditorPrefs.SetInt (REPORTER_PLUGIN_MAX_AUTO_REPORT_SEND_COUNT, maxAutoReportSendCountPerSession);

		RefreshReportGameObject ();

        if (!EditorUserBuildSettings.development)
        {
            EditorGUILayout.Space();
            GUILayout.TextArea("Don't forget to enable 'Development Build' on Build Settings to be able to catch the error logs.");
        }
        EditorGUILayout.Space();
        GUILayout.TextArea("To be able to log in a release build, please call Log.l, Log.w and Log.e for log, warning and error respectively, from CrashReportDLL.");

    }

    /* Update the game object properties to save them in to a prefab.
	 * So that these properties are saved across sessions.
	*/
    static void RefreshReportGameObject()
    {
        reportObject = Reporter.checkReportGameObject (reportObject);
		if(reportObject.ReportSendEmails != validEmailAdresses 
		   //|| reportObject.MaxAutoReportSendCount != maxAutoReportSendCount
		   || reportObject.userName != userName)
		{
			reportObject.ReportSendEmails = validEmailAdresses;
			//reportObject.MaxAutoReportSendCount = maxAutoReportSendCount;
			Assembly assembly = Assembly.GetEntryAssembly();
			if (assembly != null) {
				AssemblyName assemblyName = assembly.GetName();
				reportObject.appName = assemblyName.Name;
				reportObject.version = assemblyName.Version.ToString();
			}
			reportObject.userName = userName;
            if(!EditorApplication.isPlaying)
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

            if (!AssetDatabase.IsValidFolder(Reporter.REPORTER_PREFAB_DIRECTORY))
            {
                AssetDatabase.CreateFolder(Reporter.REPORTER_DIRECTORY, "Resources");
            }
            UnityEngine.Object emptyPrefab = PrefabUtility.CreateEmptyPrefab(Reporter.REPORTER_PREFAB_PATH_WRITE);
			PrefabUtility.ReplacePrefab(reportObject.gameObject, emptyPrefab);
		}
    }

}
#endif
