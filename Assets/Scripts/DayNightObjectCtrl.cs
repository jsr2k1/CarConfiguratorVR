using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightObjectCtrl : MonoBehaviour
{
	public bool bNight;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		foreach(Transform child in transform){
			child.gameObject.SetActive(Globals.instance.isNight == bNight);
		}
	}
}