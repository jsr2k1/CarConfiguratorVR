﻿using UnityEngine;
using Leap.Unity.Interaction;

public class ButtonController : MonoBehaviour
{
	protected int idx;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	protected virtual void Awake()
	{
		GetComponent<InteractionButton>().OnPress.AddListener(() => OnButtonPress());
		idx = 0;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public virtual void OnButtonPress()
	{
		idx++;
		idx = idx % 2;
	}
}