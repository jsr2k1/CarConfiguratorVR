using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using UnityEngine.Video;

public class VirtualCockpitMenuCtrl : MonoBehaviour
{
	public VirtualCockpitCtrl virtualCockpitCtrl;
	public VideoPlayer videoPlayer;
	public VideoClip[] videoClips;
	public VirtualCockpitCtrl.States state;

	InteractionBehaviour m_interactionBehaviour;
	bool waiting = false;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		m_interactionBehaviour = GetComponent<InteractionBehaviour>();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		m_interactionBehaviour.OnContactBegin += OnContactBegin;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnDestroy()
	{
		m_interactionBehaviour.OnContactBegin -= OnContactBegin;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Go to menu
	void OnContactBegin()
	{
		if(waiting || virtualCockpitCtrl.state == VirtualCockpitCtrl.States.MENU || virtualCockpitCtrl.state == VirtualCockpitCtrl.States.IDLE){
			return;
		}
		if(virtualCockpitCtrl.state == VirtualCockpitCtrl.States.APPCONNECT){
			videoPlayer.clip = videoClips[0];
		}
		else if(virtualCockpitCtrl.state == VirtualCockpitCtrl.States.CONTACTS){
			videoPlayer.clip = videoClips[1];
		}
		else if(virtualCockpitCtrl.state == VirtualCockpitCtrl.States.MEDIA){
			videoPlayer.clip = videoClips[2];
		}
		else if(virtualCockpitCtrl.state == VirtualCockpitCtrl.States.NAVIGATOR){
			videoPlayer.clip = videoClips[3];
		}
		virtualCockpitCtrl.state = state;
		virtualCockpitCtrl.PlayVideo();

		waiting = true;
		StartCoroutine(WaitSomeTime());
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	IEnumerator WaitSomeTime()
	{
		yield return new WaitForSeconds(1F);
		waiting = false;
	}
}