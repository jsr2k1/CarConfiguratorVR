﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunroofController : MonoBehaviour
{
	public bool isSunroof;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		Messenger.AddListener(MessengerEventsEnum.CHANGE_GLOBAL_SUNROOF, OnChangeSunroof);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		OnChangeSunroof();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnDestroy()
	{
		Messenger.RemoveListener(MessengerEventsEnum.CHANGE_GLOBAL_SUNROOF, OnChangeSunroof);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnChangeSunroof()
	{
		foreach(Transform child in transform){
			child.gameObject.SetActive(Globals.instance.sunroof == isSunroof);
		}
	}
}