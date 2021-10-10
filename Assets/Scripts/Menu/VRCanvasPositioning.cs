using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCanvasPositioning : MonoBehaviour
{
	Vector3 pos0;
	Camera cam;
	
	void Start ()
	{
		pos0 = transform.position;
		cam = Camera.main;
	}
	
	void Update ()
	{
		transform.position = cam.transform.position + pos0;
	}
}
