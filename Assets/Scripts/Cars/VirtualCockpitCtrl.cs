using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using UnityEngine.Video;

public class VirtualCockpitCtrl : MonoBehaviour
{
	public VideoPlayer[] videoPlayers;
	public GameObject info;

	public Material mat_display;
	public Color color_off;
	public Color color_on;

	public enum States{
		MENU,
		NAVIGATOR,
		CONTACTS,
		MEDIA,
		APPCONNECT,
		IDLE
	}
	public States state;

	InteractionBehaviour m_interactionBehaviour;
	AudioSource m_audio;
	bool isOn = false;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		m_interactionBehaviour = GetComponent<InteractionBehaviour>();
		m_audio = GetComponent<AudioSource>();
		state = States.IDLE;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		m_interactionBehaviour.OnContactBegin += OnContactBegin;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnEnable()
	{
		mat_display.color = color_off;
		isOn = false;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnDestroy()
	{
		m_interactionBehaviour.OnContactBegin -= OnContactBegin;
		mat_display.color = color_off;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	float ContactTime = 0;
	void OnContactBegin()
	{
		if ((Time.time-ContactTime) < 1) return;
		ContactTime = Time.time;        

		bool tisOn = isOn;

		foreach (VideoPlayer player in videoPlayers){                        
			if (!player.isPlaying)
			{
				if(isOn){
					//De momento, lo dejo, pero no se puede apagar el virtual cockpit
					m_audio.Play();
					tisOn = false;
				}else{
					Globals.instance.bCockpitOn = true;
					m_audio.Play();
					StartCoroutine(ShowVideoPlayer(player));
					tisOn = true;
					m_interactionBehaviour.enabled = false;
					StartCoroutine(WaitSomeTime());
				}
			}
		}
		isOn = tisOn;

		if(info){
			Destroy(info);
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	IEnumerator ShowVideoPlayer(VideoPlayer player)
	{
		player.Play();
		yield return new WaitForSeconds(0.5F);
		mat_display.color = color_on;
		yield return new WaitForSeconds(3F);
		Messenger.Broadcast(MessengerEventsEnum.CHANGE_AMBILIGHT);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void PlayVideo(int i=-1)
	{
		if (i == -1 || i == 0) {
			//Cockpit
			videoPlayers [0].time = 0;
			videoPlayers [0].Play ();
			m_audio.Play();
		}
		if (i == -1 || i == 1) {
			//Speedometer
			if (state != States.MENU) {
				videoPlayers [1].time = 0;
				videoPlayers [1].Play ();
			}
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	IEnumerator WaitSomeTime()
	{
		yield return new WaitForSeconds(7F);
		state = States.MENU;
	}
}