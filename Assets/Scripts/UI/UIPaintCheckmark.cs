﻿using UnityEngine;
using UnityEngine.UI;

public class UIPaintCheckmark : MonoBehaviour
{
	private Globals.Paint roof;
	private Globals.Paint body;
	private Image thisImage;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		PaintButtonController pbc = GetComponentInParent<PaintButtonController>();
		roof = pbc.paint_roof;
		body = pbc.paint_body;
		thisImage = GetComponent<Image>();
		Messenger.AddListener(MessengerEventsEnum.CHANGE_EXT_BODY_COLOR, OnChange);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		OnChange();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnDestroy()
	{
		Messenger.RemoveListener(MessengerEventsEnum.CHANGE_EXT_BODY_COLOR, OnChange);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnChange()
	{
		thisImage.enabled = (Globals.instance.currentPaintRoof == roof && Globals.instance.currentPaintBody == body) ? true : false;
	}
}
