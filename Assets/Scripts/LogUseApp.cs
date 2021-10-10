using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public static class LogUseApp 
{
    [Serializable]
    public class Config
    {
        public string url = "myurl";
        public string name = "myname";

        public Config(bool Load=false)
        {
            if (!Load) return;
            Config c = JsonUtility.FromJson<Config>(File.ReadAllText(getConfigFilePath()));
            this.url = c.url;
            this.name = c.name;
        }
    }
    static Config config = new Config(true);
    [Serializable]
    class PostStats
    {
        public string name;
        public int openApp;
        public int NumUsers;
        public int updatetime;
    }
    [Serializable]
    class Stats
    {
        public int openApp = 0;
        public int NumUsers = 0;
        public int updatetime = 0;
    }
    [Serializable]
    class RegUse
    {
        public string name;
        public int time;
        public String ACTION;

        public RegUse()
        {
            this.name = config.name;
            System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            time = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        }
    }
    public static String Action_NewUser = "NEW_USER";
    public static String Action_OpenApp = "OPEN_APP";

    [Serializable]
    class newUserParams:RegUse
    {        
        public String Model;
        public newUserParams(String Model)
        {
            this.Model = Model;
            this.ACTION = Action_NewUser;
        }
    }

    static IEnumerator SendStats(Stats s)
    {
        PostStats ps = new PostStats();
        ps.name = config.name;
        ps.openApp = s.openApp;
        ps.NumUsers = s.NumUsers;
        ps.updatetime = s.updatetime;
        string sjson = JsonUtility.ToJson(ps);

        var request = new UnityWebRequest(config.url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(sjson);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.Send();        
    }
    public enum LOGKEY:int { OpenApp = 0, NumUsers=1 };
    static string[] keys = {"OpenApp", "NumUsers"};
    private static string getFilePath()
    {
        return Globals.AppPath + "t-roc_log.txt";
    }
    private static string getConfigFilePath()
    {
        return Globals.AppPath + "config.txt";
    }
    public static void SaveNewReg(String reg)
    {
//        Debug.Log("Save reg " + reg);
        using (StreamWriter sw = File.AppendText(getFilePath()))
        {
            sw.WriteLine(reg);
        }
    }
    public static void SaveNewUser()
    {
        String Model = "X";
        if (PlayerPrefs.GetInt("model") == 0) Model = "T-Roc";
        else if (PlayerPrefs.GetInt("model") == 1) Model = "Touareg";

        SaveNewReg(JsonUtility.ToJson(new newUserParams(Model)));
    }
    public static void SaveOpenApp()
    {
        RegUse ru = new RegUse();
        ru.ACTION = Action_OpenApp;

        SaveNewReg(JsonUtility.ToJson(ru));
    }
    public static void WriteIncKey(MonoBehaviour CallObj, LOGKEY key) //RACE CONDITION?
    {
        Stats s = JsonUtility.FromJson<Stats>(File.ReadAllText(getFilePath()));
        if (key == LOGKEY.NumUsers) s.NumUsers++;
        if (key == LOGKEY.OpenApp) s.openApp++;

        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        s.updatetime = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;

        File.WriteAllText(getFilePath(), JsonUtility.ToJson(s));
        CallObj.StartCoroutine(SendStats(s));
        return;

//joel    List<string> lines = ReadFile();
//        int i = getkeyline(lines, key);
//        if (i == -1)
//        {
//            lines.Add(newkeyline(key, 1));
//        }
//        else
//        {            
//            string val = Regex.Split(lines[i], "(\\w+(\\s)*)+=(\\s)*").Last();
//            int v = 0;
//            Int32.TryParse(val, out v);
//            lines[i] = newkeyline(key, v + 1);
//        }
//        writefile(lines);
    }
    private static void writefile(List<string> lines)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(getFilePath(), false))
            {
                foreach (string l in lines) writer.WriteLine(l);
                writer.Close();
            }
        }
        catch (IOException ioex) {Debug.Log(ioex); }
    }
    private static string newkeyline(LOGKEY key, int val)
    {
            return keys[(int)key] + " = "+val;
    }
    private static int getkeyline(List<string> lines, LOGKEY key)
    {
        for(int i = 0;i<lines.Count;i++)
        {
            if (lines[i].StartsWith(keys[(int)key])) return i;
        }
        return -1;
    }
    private static List<string> ReadFile()
    {       
        List<string> lines = new List<string>();
        try {
            using (StreamReader reader = new StreamReader(getFilePath())) {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
                reader.Close();
            }
        } catch (IOException ioex){Debug.Log(ioex); }
        return lines;
    }
}
