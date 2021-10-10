using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScreenshotCapture : MonoBehaviour {
	[Header("Controls")]
	//public bool simulate = false;
	public bool startRec = false;
	private  int currentPosition = 0;

	[Header("Config")]
	public string path;	//D:\Users\Gravient\Documents\T-Roc Captures\1
	public int fps = 30;
	public int duration;
	public Transform start;
	public Transform end;
	[Tooltip("1x1")]
	public AnimationCurve curve; //1x1

	int totalFrames;
	List<Vector3> positions;
	//float time;

	void Awake ()
	{
		positions = new List<Vector3> ();
		totalFrames = fps * duration;

		for (int i = 0; i < totalFrames; i++)
		{
			positions.Add(Lerp (start.position, end.position, curve.Evaluate (i / (float)totalFrames)));
		}
	}
		

	void Update ()
	{
		/*if (simulate)
		{
			transform.position = Lerp (start.position, end.position, curve.Evaluate (Time.timeSinceLevelLoad - time / (float)duration));
		}
		else
		{
			time = Time.timeSinceLevelLoad;
		}*/

		if (Input.GetKeyDown (KeyCode.Space))
		{
			startRec = true;
		}

		if (startRec)
		{
			startRec = false;
			StartCoroutine (Record());
		}
	}

	IEnumerator Record()
	{
		if (currentPosition == positions.Count)
		{
			StopAllCoroutines ();
		}

		transform.position = positions [currentPosition++];
		string name = String.Format("{0}/{1:D04} shot.png", path, currentPosition );
		ScreenCapture.CaptureScreenshot(name, 2);
		yield return new WaitForSeconds(0.1f);
		StartCoroutine (Record());
	}

	static Vector3 Lerp(Vector3 a, Vector3 b, float t)
	{
		return new Vector3(Mathf.Lerp (a.x, b.x, t),Mathf.Lerp (a.y, b.y, t),Mathf.Lerp (a.z, b.z, t));
	}
}
