using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightManager : MonoBehaviour
{
	public GameObject dayNightOn;
	public GameObject dayNightOff;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		dayNightOn.SetActive(Globals.instance.hasDayNight);
		dayNightOff.SetActive(!Globals.instance.hasDayNight);
	}
}