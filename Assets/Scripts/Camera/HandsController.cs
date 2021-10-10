using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

public class HandsController : MonoBehaviour
{
	HandPool m_handPool;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		m_handPool = GetComponent<HandPool>();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		if(Globals.instance.current_gender == Globals.HandGender.MALE){
			m_handPool.EnableGroup("Male_Hands");
			m_handPool.DisableGroup("Female_Hands");
		}else{
			m_handPool.EnableGroup("Female_Hands");
			m_handPool.DisableGroup("Male_Hands");
		}
	}
}