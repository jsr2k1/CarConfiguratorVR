using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuVideoController : MonoBehaviour
{
	public GameObject menuCanvas;
	public GameObject videoCamera;
	float countdown;
	void Start ()
	{
		Init();
	}
	
	// Update is called once per frame
	void Update ()
	{
		countdown -= Time.deltaTime;
		if(countdown < 0)
		{
			PlayVideo();
		}

		if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 || Input.anyKey || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
		{
			Init();
		}
	}

	void Init()
	{
		countdown = 60;
		menuCanvas.SetActive(true);
		videoCamera.SetActive(false);
	}

	void PlayVideo()
	{
		menuCanvas.SetActive(false);
		videoCamera.SetActive(true);
	}
}
