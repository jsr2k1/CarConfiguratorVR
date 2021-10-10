using UnityEngine;
using System.Collections;

public class ScriptFPS : MonoBehaviour
{
	public float updateInterval = 1.0f;
	public GUIStyle style;
	public bool isEnable = false;

	float accum = 0; // FPS accumulated over the interval
	float timeleft; // Left time for current interval
	int frames = 0; // Frames drawn over the interval
	string sFPS;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		timeleft = updateInterval;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		++frames;

		if (timeleft <= 0.0)
		{
			float fps = accum / frames;
			sFPS = System.String.Format("{0:F2} FPS", fps);

			timeleft = updateInterval;
			accum = 0.0F;
			frames = 0;
		}

		if(Input.GetKeyDown(KeyCode.F)){
			isEnable = !isEnable;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnGUI()
	{
		if(isEnable){
			GUI.Label(new Rect(Screen.width / 2, 20, 200, 50), sFPS, style);
		}
	}
}
