using UnityEngine;
using Leap.Unity.Interaction;

public class InteractionManagerController : MonoBehaviour
{
	private InteractionManager interactionManager;

	private InteractionHand leftHand;
	private InteractionHand rightHand;

	private InteractionHand menuHand;
	private InteractionHand otherHand;

	private bool[] rightFingertips;
	private bool[] leftFingertips;

	private bool menuMoving = false;
	private bool panelLock = false;
	private bool menuHidden = true;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	private void Awake()
	{
		interactionManager = GetComponent<InteractionManager>();
		menuHand = leftHand = interactionManager.transform.GetChild(0).GetComponent<InteractionHand>();
		otherHand = rightHand = interactionManager.transform.GetChild(1).GetComponent<InteractionHand>();

		leftFingertips = leftHand.enabledPrimaryHoverFingertips;
		rightFingertips = rightHand.enabledPrimaryHoverFingertips;

		Messenger.AddListener(MessengerEventsEnum.MOVING_MENU, MovingMenu);
		Messenger.AddListener(MessengerEventsEnum.STOP_MENU, StopMenu);
		Messenger.AddListener(MessengerEventsEnum.GO_TO_PANEL, GoToPanel);
		Messenger.AddListener(MessengerEventsEnum.END_GO_TO_PANEL, EndGoToPanel);
		Messenger.AddListener(MessengerEventsEnum.HIDE_MENU, HideMenu);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	private void OnDestroy()
	{
		Messenger.RemoveListener(MessengerEventsEnum.MOVING_MENU, MovingMenu);
		Messenger.RemoveListener(MessengerEventsEnum.STOP_MENU, StopMenu);
		Messenger.RemoveListener(MessengerEventsEnum.GO_TO_PANEL, GoToPanel);
		Messenger.RemoveListener(MessengerEventsEnum.END_GO_TO_PANEL, EndGoToPanel);
		Messenger.RemoveListener(MessengerEventsEnum.HIDE_MENU, HideMenu);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	private void MovingMenu()
	{
		menuHidden = false;
		menuMoving = true;
		ChangeHandsStates();
	}

	private void StopMenu()
	{
		menuMoving = false;
		ChangeHandsStates();
	}

	private void GoToPanel()
	{
		panelLock = true;
		ChangeHandsStates();
	}

	private void EndGoToPanel()
	{
		panelLock = false;
		ChangeHandsStates();
	}

	private void HideMenu()
	{
		menuHidden = true;
		ChangeHandsStates();
	}

	private void ChangeHandsStates()
	{
		if (panelLock || menuHidden)
		{
			HandActivator(menuHand, false);
			HandActivator(otherHand, false);
		}
		else if (menuMoving)
		{
			HandActivator(menuHand, false);
			HandActivator(otherHand, true);
		}
		else
		{
			HandActivator(menuHand, true);
			HandActivator(otherHand, true);
		}
	}

	private void HandActivator(InteractionHand hand, bool active)
	{
		//hand.hoverEnabled = active;
		//hand.contactEnabled = active;
		//hand.graspingEnabled = active;

		if (active)
		{
				if (hand == leftHand)
					hand.enabledPrimaryHoverFingertips = leftFingertips;
				else
					hand.enabledPrimaryHoverFingertips = rightFingertips;

			//hand.enabledPrimaryHoverFingertips = new bool[] { false, true, false, false, false };
		}
		else
		{
			hand.enabledPrimaryHoverFingertips = new bool[] { false, false, false, false, false };
		}
	}
}
