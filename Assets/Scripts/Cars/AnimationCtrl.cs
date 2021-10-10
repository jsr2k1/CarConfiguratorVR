using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCtrl : MonoBehaviour
{
	public bool soundAnim;
	public float speed_mult = 1F;

	Animation m_animation;
	AudioSource audioSource;

	bool opened = false;
	bool soundPlayed = true;
	bool moving = false;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		m_animation = GetComponent<Animation>();
		
		if(soundAnim){
			audioSource = GetComponent<AudioSource>();
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnEnable()
	{
		opened = false;
		if(m_animation.clip){
			m_animation.clip.SampleAnimation(gameObject, 0);
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void PlayAnim()
	{
		if(!moving)
		{
			m_animation["Take 001"].speed = opened ? -3F*speed_mult : 2F*speed_mult;
			m_animation["Take 001"].time = opened ? m_animation["Take 001"].length : 0;
			m_animation.Play();
			opened = !opened;
			soundPlayed = false;
			Invoke("MovingTrue", 0.3f);
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void MovingTrue()
	{
		moving = true;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		if(m_animation["Take 001"] == null){
			Debug.LogWarning("ERROR: No animation clips in: " + gameObject.name);
		}
		else{
			if(m_animation["Take 001"].time == 0 || m_animation["Take 001"].time == m_animation["Take 001"].length){
				moving = false;
			}
			if(soundAnim && !soundPlayed && !opened && m_animation["Take 001"].time < 0.3f){
				soundPlayed = true;
				audioSource.Play();
			}
		}
	}
}
