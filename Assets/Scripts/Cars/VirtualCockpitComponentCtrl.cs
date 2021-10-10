using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using UnityEngine.Video;

public class VirtualCockpitComponentCtrl : MonoBehaviour
{
	public VirtualCockpitCtrl virtualCockpitCtrl;
	public VideoPlayer videoPlayer;
	public VideoPlayer videoPlayerSpeed;
	public VideoClip videoClip;
	public VideoClip videoClipSpeed;
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
	//Go to submenu
	void OnContactBegin()
	{
		if(waiting || virtualCockpitCtrl.state == state || virtualCockpitCtrl.state == VirtualCockpitCtrl.States.IDLE){
			return;
		}
		videoPlayer.clip = videoClip;
		virtualCockpitCtrl.PlayVideo(0);

		StartCoroutine(WaitSomeTimeForPlayerSpeed());

		waiting = true;
		StartCoroutine(WaitSomeTime());
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	IEnumerator WaitSomeTimeForPlayerSpeed()
	{
		yield return new WaitForSeconds(0.75F);
		videoPlayerSpeed.clip = videoClipSpeed;
		virtualCockpitCtrl.state = state;
		virtualCockpitCtrl.PlayVideo(1);
	}

	IEnumerator WaitSomeTime()
	{
		yield return new WaitForSeconds(1F);
		waiting = false;
	}
}