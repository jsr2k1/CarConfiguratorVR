using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontLightsCtrl : MonoBehaviour
{
	public GameObject left_light;
	public GameObject right_light;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		left_light.SetActive(Globals.instance.isNight);
		right_light.SetActive(Globals.instance.isNight);
	}
}