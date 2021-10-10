using UnityEngine;

public class DebugLog : MonoBehaviour {

	public void Log(string _string)
	{
		Debug.Log(_string);
	}

	public void LogWarning(string _string)
	{
		Debug.LogWarning(_string);
	}

	public void LogError(string _string)
	{
		Debug.LogError(_string);
	}
}
