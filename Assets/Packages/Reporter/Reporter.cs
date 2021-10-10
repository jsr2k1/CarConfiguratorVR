using UnityEngine;
using System;

public class Reporter {

	public const String REPORTER_GAMEOBJECT_NAME = "ReporterGameObject";

    public const string REPORTER_DIRECTORY = "Assets/Plugins/Reporter";
    public const string REPORTER_PREFAB_DIRECTORY = REPORTER_DIRECTORY+"/Resources";
    public const string REPORTER_PREFAB_PATH = /*REPORTER_PREFAB_DIRECTORY+"/"+*/ REPORTER_GAMEOBJECT_NAME;
    public const string REPORTER_PREFAB_PATH_WRITE = REPORTER_PREFAB_DIRECTORY + "/" + REPORTER_PREFAB_PATH + ".prefab";

    /* Capture a screenshot from the current scene.
	 * This method is seperated from the report sending process, to be able to capture a screenshot before a screen triggered to 
	*/
    public static void CaptureScreenShot(Action onScreenShotCaptured = null)
    {
        checkReportGameObject();
        if (onScreenShotCaptured != null)
        {
            Report.report.OnScreenShotCaptured += onScreenShotCaptured;
        }
        Report.report.takeScreenShot();
    }

    /* Send a custom report on demand with a title and message.
	 * Take a screenshot if not taken already.
	*/
    public static void SendUserReport(string title = "", string message = "", string userId = "", Action<bool> onComplete = null)
    {
        checkReportGameObject();
        Report.report.Send(title, message, userId, onComplete);
    }

    /* A GameObject is needed in the scene to be able to take a screenshot.
	  * Check if the Report GameObject exits, if not create one and put it in the scene.
	  * Return the Report instance.
	*/
    [ExecuteInEditMode]
    public static Report checkReportGameObject(Report _reportObject = null)
	{
        if (_reportObject != null)
        {
            Report.report = _reportObject;
            return Report.report;
        }
        
        if (Report.report == null)
		{
            GameObject go = GameObject.Find(REPORTER_GAMEOBJECT_NAME);

            if (!go) {
                go = Resources.Load<GameObject>(REPORTER_PREFAB_PATH);
                if (go)
                {
                    go = UnityEngine.Object.Instantiate(go);
                    go.name = REPORTER_GAMEOBJECT_NAME;

#if UNITY_EDITOR
                    if (!UnityEditor.EditorApplication.isPlaying)
                        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
#endif
                }
                else
                {
                    if (Report.report != null)
                    {
                        UnityEngine.Object.DestroyImmediate(Report.report.gameObject);
                    }
                    go = new GameObject(REPORTER_GAMEOBJECT_NAME, typeof(Report));
                }
            }
            Report.report = go.GetComponent<Report>();
        }
		return Report.report;
	}



}
