using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
	public GameObject canvas_in;
	public GameObject canvas_out;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		Messenger.AddListener(MessengerEventsEnum.CHANGE_GLOBAL_POS, OnChangeGlobalPos);

		canvas_in.SetActive(false);
		canvas_out.SetActive(true);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnDestroy()
	{
		Messenger.RemoveListener(MessengerEventsEnum.CHANGE_GLOBAL_POS, OnChangeGlobalPos);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnChangeGlobalPos()
	{
		StartCoroutine(RefreshCanvas());
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	IEnumerator RefreshCanvas()
	{
		//Wait one frame to be sure that camPosition has been updated
		yield return new WaitForEndOfFrame();
//		yield return new WaitForSeconds(1F);

		canvas_in.SetActive(Globals.instance.camPosition == Globals.CamPositions.INTERIOR);
		canvas_out.SetActive(Globals.instance.camPosition == Globals.CamPositions.EXTERIOR);
	}
}