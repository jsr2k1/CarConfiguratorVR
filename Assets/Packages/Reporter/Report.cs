using UnityEngine;
using System.Collections;
using System;
using CrashReportDLL;
using System.IO;


public class Report:MonoBehaviour{
    private static string NO_UNITY_ACCOUNT_ERROR = "Report plugin could not send the report because no unity account is loged in. Please go to Window/Report Settings.";
    private static string NO_EMAIL_ADRESS_ERROR = "Report plugin could not send the report because no email adress is entered. Please enter at least one email adress at Window/Report Settings.";

    private static string SERVER_PATH="http://pawio.com/crashreport";
	private static string UPLOAD_IMG_URL = SERVER_PATH+"/image.php";
	private static string UPLOAD_IMG_PATH = "temp/";
    private static int MaxAutoReportSendCount = 1;

    public static Report report = null;
    private static ReportStatus _reportSendStatus = ReportStatus.Idle;
    private static ReportStatus reportSendStatus
    {
        set {
            _reportSendStatus = value;
            PlayerPrefs.SetInt("REPORTER_reportSendStatus", (int)value);
        }
        get { return _reportSendStatus; }
    }

    enum ReportStatus { Start, SavingSS, SendingSS, SentSS, SendingLog, Done, Idle };

    private byte[] screenshotBytes=null;
	private string imagePath = "";
	private string imageName = "";
    public event Action<bool> OnComplete = null;
    public event Action OnScreenShotCaptured = null;

    private string userId="", title="", message="";
	public string ReportSendEmails = "";
	public Texture2D ScreenShotTexture = null;
	public string appName="", version="",userName="";

    float deltaTime = 0.0f, lastSavedTime = 0f;
    float targetfps = 0;

    /* Register to unhandled exceptions.
	*/
    static Report(){
        AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;
    }

    /* On unhandled exception, send an automated report if auto report send count isn't exceeded.
	*/
    static void HandleUnhandledException (object sender, UnhandledExceptionEventArgs e)
	{
		checkReportObject ();
		if(MaxAutoReportSendCount>0){
			MaxAutoReportSendCount--;
			Debug.LogException((Exception) e.ExceptionObject,(UnityEngine.Object) sender);
			SendReport(e.IsTerminating, "AUTO REPORT ON EXCEPTION");
		}
        Debug.Log("HandleUnhandledException:" + e.IsTerminating);
	}

    /* Send report with user message and email title. Triggered by user.
	*/
    public static void SendReport(string title = "", string message = "", string userId = "", Action<bool> onComplete = null)
    {
        checkReportObject();
        report.Send(title, message, userId, onComplete);
    }

    /* This can be triggered by the developer or by exception handling .
	*/
    public static void SendReport(bool auto, string title){
		checkReportObject ();
		if (MaxAutoReportSendCount > 0) {
            MaxAutoReportSendCount--;
            if (auto){
				title = "AUTO SENT "+ title;
			}
			report.Send(title);
		}
	}
	
	private static void checkReportObject()
	{
		report = Reporter.checkReportGameObject (report);
	}

    private bool checkIfReportCanBeSent()
    {
        if (userName != "" && ReportSendEmails != "")
        {
            return true;
        }
        else
        {
            #if UNITY_EDITOR
                if (userName == "")
                {
                    StartCoroutine(delayedLog(NO_UNITY_ACCOUNT_ERROR));
                }
                else
                {
                    StartCoroutine(delayedLog(NO_EMAIL_ADRESS_ERROR));
                }
            #endif
            return false;
        }
    }

    private IEnumerator delayedLog(string log)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log(log);
    }

    /* Check exit code from the previous session
     * Register to unhandled exceptions.
	 * Log class handles the logs from the editor, which includes the error logs.
	 * ReportHelper creates a secure web form and gets triggered by Log, if error log captured.
	*/
	public static Report instance;
    void Start()
	{
		if(instance == null){
			instance = this;
			DontDestroyOnLoad(gameObject);
		}else if(instance != this){
			Destroy(gameObject);
		}
        report = this;
        UPLOAD_IMG_PATH = userName + "/";
        checkPreviousSession();

        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(HandleUnhandledException);
        #if UNITY_5_0_OR_NEWER
            Application.logMessageReceived += Log.handleLog;
//        #else
//            Application.RegisterLogCallback(Log.handleLog);
        #endif
        ReportHelper.sendReportDelegate += SendReport;
        if (Application.targetFrameRate > 0)
            targetfps = Application.targetFrameRate;
    }

    [ExecuteInEditMode]
    public void checkPreviousSession()
    {
        if (getExitCode() != 0)
        {
            //Send Crash report regarding the previous session
            string crashLog = LogFile.load();
            if (crashLog != "")
            {
                ReportStatus status = (ReportStatus)PlayerPrefs.GetInt("REPORTER_reportSendStatus", (int)ReportStatus.SentSS);
                imagePath = PlayerPrefs.GetString("REPORTER_imagePath", "");
                imageName = PlayerPrefs.GetString("REPORTER_imageName", "");
                Debug.Log("status:" + status + ", " + imagePath + ", " + imageName);
                if (status == ReportStatus.SendingSS && imagePath != "")
                {
                    byte[] bytes = LogFile.loadSS();
                    if (bytes != null)
                    {
                        StartCoroutine(Sequence(UploadScreenShot(bytes), SendReportRequest(crashLog)));
                        LogFile.deleteSS();
                        crashLog = "";
                    }
                }

                if (crashLog != "")//The condition above didn't work
                {
                    StartCoroutine(SendReportRequest(crashLog));
                }
                PlayerPrefs.DeleteKey("REPORTER_imageName");
                PlayerPrefs.DeleteKey("REPORTER_imagePath");
                reportSendStatus = ReportStatus.Idle;
            }
        }
        setExitCode(1);
    }

    /*
     * Save the log to a file at the moment the application is the closest to desired FPS. Meaning most idle state.
     * */
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        if(Application.targetFrameRate < 0)
        {
            targetfps += fps - targetfps;
        }

        if (Mathf.Approximately(fps, targetfps) && Time.fixedTime - lastSavedTime > 3)
        {
            LogFile.save();
            lastSavedTime = Time.fixedTime;
        }
    }

    void OnApplicationQuit()
    {
        if(reportSendStatus < ReportStatus.Done)//the report couldn't be sent
        {
            setExitCode(1);
        }else
        {
            setExitCode(0);
        }
    }
    /*On mobile devices OnApplicationQuit might not be called, instead listen to OnApplicationPause.
    */
    void OnApplicationPause(bool pause)
    {
        if(report != null){//check if Start has called. Because earlier than Unity 5, OnApplicationPause might be called before Start.
            if (pause)
                OnApplicationQuit();
            else
                setExitCode(1);
        }
    }


    void setExitCode(int exitCode)
    {
        PlayerPrefs.SetInt("REPORTER_EXIT_CODE", exitCode);
        PlayerPrefs.Save();
        LogFile.save();
    }
    int getExitCode()
    {
        return PlayerPrefs.GetInt("REPORTER_EXIT_CODE", 0);
    }

    /* Call the sending coroutines in sequence. Each should wait for the other:
		Encode the screenshot
		Upload the screenshot
		Send the screenshot url and logs in web post
	*/
    public void Send(string title = "", string message = "", string userId = "", Action<bool> onComplete = null)
    {
        this.title = title;
        this.message = message;
        this.userId = userId;
        if (onComplete != null)
            this.OnComplete += onComplete;
        if (checkIfReportCanBeSent())
        {
            reportSendStatus = ReportStatus.SendingLog;
            if (ScreenShotTexture)
                StartCoroutine(Sequence(UploadScreenShot(), SendReportRequest()));
            else
                StartCoroutine(Sequence(ScreenshotEncode(), UploadScreenShot(), SendReportRequest()));
        }
        else if (this.OnComplete != null)
        {
            this.OnComplete(false);
            this.OnComplete = null;
        }
    }


    /* Post the bytearray of the screenshot which was saved earlier.
	*/
    private IEnumerator UploadScreenShot(){
        reportSendStatus = ReportStatus.SavingSS;
        DateTime time = DateTime.UtcNow;
        //The image should be in the directory of today
        imagePath = UPLOAD_IMG_PATH + time.Day.ToString () + "-" + time.Month.ToString () + "-" + time.Year.ToString ();

		// Create a Web Form
		WWWForm form = new WWWForm ();
		try{
            LogFile.saveSS(screenshotBytes);
            PlayerPrefs.SetString("REPORTER_imagePath", imagePath);
            reportSendStatus = ReportStatus.SendingSS;
            form.AddBinaryData ("upload_file", screenshotBytes, "tmp_name", "image/jpg");
		}catch(Exception e){Debug.LogWarning("Error on UploadScreenShot: "+e);}
		// Upload to a cgi script
		WWW w = new WWW (UPLOAD_IMG_URL + "?path=" + imagePath, form);

		yield return w;
		if (!String.IsNullOrEmpty (w.error)) {
			Debug.Log (w.error);
		} else{
			imageName = w.text;
            PlayerPrefs.SetString("REPORTER_imageName", imageName);
            reportSendStatus = ReportStatus.SentSS;
            ScreenShotTexture = null;
        }
	}

    /* Post the given bytearray of the screenshot from the previous session.
	*/
    private IEnumerator UploadScreenShot(byte[] bytes)
    {
        // Create a Web Form
        WWWForm form = new WWWForm();
        try
        {
            form.AddBinaryData("upload_file", bytes, "tmp_name", "image/jpg");
        }
        catch (Exception e) { Debug.LogWarning("Error on UploadScreenShot: " + e); }
        // Upload to a cgi script
        WWW w = new WWW(UPLOAD_IMG_URL + "?path=" + imagePath, form);

        yield return w;
        if (!String.IsNullOrEmpty(w.error))
        {
            Debug.Log(w.error);
        }
        else
        {
            imageName = w.text;
            ScreenShotTexture = null;
        }
    }

    /* Create and send a web request .
	*/
    private IEnumerator SendReportRequest(){
        reportSendStatus = ReportStatus.SendingLog;
        WWWForm form = ReportHelper.createForm (title, message, ReportSendEmails, appName, version, userName, userId, imageName, imagePath);

		// Upload to a cgi script
		WWW w = new WWW (SERVER_PATH+"/report.php", form);
		
		yield return w;
        bool success = String.IsNullOrEmpty(w.error);
        if (!success)
        {
            Debug.LogWarning(w.error);
        }
        else
        {
            reportSendStatus = ReportStatus.Done;
            ScreenShotTexture = null;
			screenshotBytes = null;
            imageName = "";
            imagePath = "";
            PlayerPrefs.DeleteKey("REPORTER_imageName");
            PlayerPrefs.DeleteKey("REPORTER_imagePath");
            Debug.Log("Report Sent");
		}
        if (OnComplete != null)
        {
            OnComplete(success);
            OnComplete = null;
        }
    }

    private IEnumerator SendReportRequest(string crashLog)
    {
        string crashMessage = "You receive this report with the 'CRASH!' title because either:\n-The app crashed and was killed by the operating system and the report couldn't be sent in the previous session\n-Or the app user didn't have internet in the previous session and the report is saved to be served later.";
        WWWForm form = ReportHelper.createForm("CRASH!", crashMessage, ReportSendEmails, appName, version, userName, userId, imageName, imagePath, crashLog);

        // Upload to a cgi script
        WWW w = new WWW(SERVER_PATH + "/report.php", form);

        yield return w;
        bool success = String.IsNullOrEmpty(w.error);
        if (!success)
        {
            Debug.LogWarning(w.error);
        }
        else
        {
            imageName = "";
            imagePath = "";
            Debug.Log("Crash Report Sent");
        }
        if (OnComplete != null)
        {
            OnComplete(success);
            OnComplete = null;
        }
    }


    public void takeScreenShot(){
		StartCoroutine(ScreenshotEncode());
	}


    /* Get screen as a Texture2D and encode it to byte array.
	*/
    IEnumerator ScreenshotEncode(){
		yield return new WaitForEndOfFrame();
		
		// create a texture to pass to encoding
		Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		
		// put buffer into texture
		texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		texture.Apply();
        ScreenShotTexture = texture;

        if (OnScreenShotCaptured != null)
        {
            OnScreenShotCaptured();
            OnScreenShotCaptured = null;
        }

        screenshotBytes = texture.EncodeToJPG ();

	}


    /* Start the given coroutines in sequence. Each coroutine should wait the other to finish for starting.
	*/
    public static IEnumerator Sequence(params IEnumerator[] sequence)
	{
		for(int i = 0 ; i < sequence.Length; ++i)
		{
			while(sequence[i].MoveNext())
				yield return sequence[i].Current;
		}
	}


}