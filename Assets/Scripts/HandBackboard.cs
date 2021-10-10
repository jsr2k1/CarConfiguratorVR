using Leap.Unity.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBackboard : MonoBehaviour {
    public Leap.Unity.IHandModel HandModelLeftMale = null;
    public Leap.Unity.IHandModel HandModelLeftFemale = null;

    public SimpleFacingCameraCallbacks FacingCallbacks;
    public Transform Pal;
//    private int State;
    bool MoveMenu = false;

	private MenuObserver mObserver;
	private Transform centerEyeAnchor;

	private void Awake()
	{
		mObserver = GetComponentInChildren<MenuObserver> ();
		centerEyeAnchor = GameObject.Find ("G_Camera/LMHeadMountedRig/CenterEyeAnchor").transform;
	}
    // Use this for initialization
    void Start() {
//        State = 0;
    }

    // Update is called once per frame
    void Update()
    {
		//Cerrar menú cuando te alejas
		if (mObserver.show && (centerEyeAnchor.position - transform.position).magnitude > 0.75f)
		{
			CloseMenu ();
		}
        if (Globals.instance.current_gender == Globals.HandGender.MALE) UpdateBackboard(HandModelLeftMale);
        else                                                            UpdateBackboard(HandModelLeftFemale);
    }
    void UpdateBackboard(Leap.Unity.IHandModel HandModel)
    {
        bool AMoveMenu = false;
        if (ActiveByPalm(HandModel, true))
        {
            if (SkyHand(HandModel))
            {
                FacingCallbacks.OnBeginFacingCamera.Invoke();
                FacingCallbacks.transform.parent.GetComponent<SmoothFollow>().rePosition();
                AMoveMenu = true;
            }
        }
        if (AMoveMenu != MoveMenu)
        {
            MoveMenu = AMoveMenu;
            if (MoveMenu) Messenger.Broadcast(MessengerEventsEnum.MOVING_MENU);
            else Messenger.Broadcast(MessengerEventsEnum.STOP_MENU);
        }
    }
    public void CloseMenu()
    {
		Messenger.Broadcast(MessengerEventsEnum.HIDE_MENU);
        FacingCallbacks.OnEndFacingCamera.Invoke();
    }
    bool ActiveByPalm(Leap.Unity.IHandModel HandModel, bool Open)
    {
        bool extend = false;

        Leap.Hand hand = GetHand(HandModel);
        if (hand != null)
        {
            //Debug.Log(hand.Direction);

            extend = true;
            for (int i = 0; i < 5; i++)
            {
                if (Open)
                    extend &= hand.Fingers[i].IsExtended;
                else
                    extend &= !hand.Fingers[i].IsExtended;
            }
        }

        return extend;
    }
    bool SkyHand(Leap.Unity.IHandModel HandModel)
    {
        Leap.Hand hand = GetHand(HandModel);
        if (hand != null)
        {
            //if(Vector3.Dot(new Vector3(0,1,0).normalized, Pal.forward) > 0.8f)
            //if (hand.Direction.y < 0.25f)
            
            if (hand.PalmNormal.y > 0.6f)
            //if (Pal.transform.rotation.eulerAngles.z < 100 && Pal.transform.rotation.eulerAngles.z > 25)
            //if(Pal.transform.rotation.eulerAngles.z<0.25)
            {
                return true;
            }
        }
        return false;
    }
    Leap.Hand GetHand(Leap.Unity.IHandModel HandModel)
    {
        if (HandModel != null) return HandModel.GetLeapHand();            
        return null;
    }
}
