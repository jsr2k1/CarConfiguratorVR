﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

[RequireComponent(typeof(InteractionBehaviour))]
public class AnimTriggerCtrl : MonoBehaviour
{
	public AnimationCtrl[] target_anim;
	public GameObject info;

	bool ready = true;
	InteractionBehaviour m_interactionBehaviour;

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

	void OnDisable()
	{
		StopAllCoroutines();
		ready = true;
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnContactBegin()
	{
		if(ready){
			foreach(AnimationCtrl target in target_anim){
				target.PlayAnim();
			}
			Destroy(info);
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

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		if(info && Camera.main){
			float dist = Vector3.Distance(Camera.main.transform.position, transform.position);
			info.SetActive(dist < Globals.instance.info_distance);
		}
	}
}