using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_LineCtrl : MonoBehaviour
{
	Renderer m_renderer;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		m_renderer = GetComponent<Renderer>();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		m_renderer.enabled = (Globals.instance.currentUpholstery == Globals.Upholstery.TOUAREG_CUERO_R_LINE_GRACE ||
							  Globals.instance.currentUpholstery == Globals.Upholstery.TOUAREG_CUERO_R_LINE_VIENA);
	}
}