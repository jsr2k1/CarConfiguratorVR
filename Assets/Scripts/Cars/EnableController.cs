﻿using UnityEngine;
using System.Collections;

public class EnableController : MonoBehaviour
{
	public MessengerEventsEnum messengerEvent;
	public bool IsDefault;
	bool bActiveChildren;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		Messenger.AddListener(messengerEvent, MsgSetChildren);
		bActiveChildren = IsDefault;

		SetChildren();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnDestroy()
	{
		Messenger.RemoveListener(messengerEvent, MsgSetChildren);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void MsgSetChildren()
	{
		bActiveChildren = !bActiveChildren;
		SetChildren();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void SetChildren()
	{		
		foreach(Transform child in transform){
			child.gameObject.SetActive(bActiveChildren);
		}
	}
}
