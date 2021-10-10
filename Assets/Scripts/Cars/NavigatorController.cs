using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

[RequireComponent(typeof(InteractionBehaviour))]
public class NavigatorController : MonoBehaviour
{
	public Texture tex_music_on;
	public Texture tex_music_off;
	public Material mat_nav;

	InteractionBehaviour m_interactionBehaviour;
	AudioSource m_audioSource;
	bool ready = true;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		m_interactionBehaviour = GetComponent<InteractionBehaviour>();
		m_audioSource = GetComponent<AudioSource>();
		m_interactionBehaviour.OnContactBegin += OnContactBegin;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnEnable()
	{
		m_audioSource.Stop();
		mat_nav.mainTexture = tex_music_off;
		mat_nav.SetTexture("_EmissionMap", tex_music_off);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnDestroy()
	{
		m_interactionBehaviour.OnContactBegin -= OnContactBegin;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnContactBegin()
	{
		if(ready){
			if(m_audioSource.isPlaying){
				m_audioSource.Stop();
				mat_nav.mainTexture = tex_music_off;
				mat_nav.SetTexture("_EmissionMap", tex_music_off);
			} else{
				m_audioSource.Play();
				mat_nav.mainTexture = tex_music_on;
				mat_nav.SetTexture("_EmissionMap", tex_music_on);
			}
			StartCoroutine(Wait());
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	IEnumerator Wait()
	{
		ready = false;
		yield return new WaitForSeconds(2F);
		ready = true;
	}
}