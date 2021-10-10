using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using UnityEngine.UI;

public class InteractionButtonDisablerOnMovingMenu : MonoBehaviour
{
	private InteractionButton button;
	private Image[] images;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	private void Awake()
	{
		button = GetComponentInChildren<InteractionButton>();
		images = GetComponentsInChildren<Image>();
		Messenger.AddListener(MessengerEventsEnum.MOVING_MENU, MovingMenu);
		Messenger.AddListener(MessengerEventsEnum.STOP_MENU, StopMenu);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	private void OnDestroy()
	{
		Messenger.RemoveListener(MessengerEventsEnum.MOVING_MENU, MovingMenu);
		Messenger.RemoveListener(MessengerEventsEnum.STOP_MENU, StopMenu);
	}

	private void MovingMenu()
	{
		button.enabled = false;
		foreach(Image img in images)
		{
			img.enabled = false;
		}
	}

	private void StopMenu()
	{
		button.enabled = true;
		foreach(Image img in images)
		{
			img.enabled = true;
		}
	}
}