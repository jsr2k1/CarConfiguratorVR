using UnityEngine;

public class Test : MonoBehaviour {

void Start () {
        GameObject reporter = GameObject.Find("ReporterGameObject");
        if(reporter == null){
            Debug.LogWarning("Please go to Window/Report Settings to configure your project for Reporter.");
        }
    }
    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 50), "Soft Error", GUI.skin.GetStyle("Button")))
        {
            softError();
        }
        if (GUI.Button(new Rect(110, 0, 100, 50), "StackOverflow", GUI.skin.GetStyle("Button")))
        {
            stackOverflow();
        }
        if (GUI.Button(new Rect(220, 0, 100, 50), "Crash", GUI.skin.GetStyle("Button")))
        {
            crash();
        }
        if (GUI.Button(new Rect(330, 0, 100, 50), "User Report", GUI.skin.GetStyle("Button")))
        {
            userReport();
        }
    }

    // Update is called once per frame
    int random;
	void Update () {
        random = Mathf.RoundToInt (Random.value * 1000);
        if (random < 3)
            log();
	}

    /*Create a NullReferenceException
     * Reporter will take a screenshot and send the logs when an exception is fired.
     * This is done in an separate Thread, not to harm the app performance.*/
    void softError()
	{
		string s=null;
        Debug.Log(s.Length);
	}
   
    /*Create StackOverflow Exception
     * Mostly, Reporter will be able to send a report in this case.
     * But on a device a StackOverflow might crash the app. If it crashes, Reporter will track the report sending progress and will send it next session if it fails in this session.
     * You might receive two reports in this situation, because the app might die before the response of the report request is received. A crash report will still be sent next session to be sure not to miss any reports.*/
    void stackOverflow()
    {
        GameObject.CreatePrimitive(PrimitiveType.Capsule);
        Application.Quit();
        stackOverflow();
        
    }

    /*Crash the app immediatelly
    * Even though this isn't a real life scenario, Reporter will recognize the crash of the app and send the log next session.
    * In this case a screenshot won't be possible.*/
    void crash()
    {
        #if UNITY_5
            Debug.Log("The crash log will be sent next session if the app crashes. (This might not work in the editor, try it in a build.)");
            Application.ForceCrash(1);
        #else
            Debug.Log("Your unity version doesn't support internal crash. Try StackOverflow button.");
        #endif
    }

    /*Send a user report
    * First capture a screenshot so that the scene reflects the state of the user complain without the report window.
    * After the screen pixels are read, open the user report window and ask the user for a feedback.*/
    void userReport()
    {
        Reporter.CaptureScreenShot(onScreenShotCaptured);
    }

    private void onScreenShotCaptured()
    {
        GameObject userReport = Resources.Load<GameObject>("UserReport");
        userReport = Instantiate<GameObject>(userReport);
    }


    void log()
	{
		Debug.Log ("Test for some logging "+Time.deltaTime);
	}
}
