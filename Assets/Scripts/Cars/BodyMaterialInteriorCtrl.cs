using UnityEngine;
using System.Collections;

public class BodyMaterialInteriorCtrl : MonoBehaviour
{
	Material default_mat;
	Renderer m_renderer;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		m_renderer = GetComponent<Renderer>();
		default_mat = m_renderer.sharedMaterial;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
//		ChangeMaterial();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnEnable()
	{
		Messenger<bool>.AddListener(MessengerEventsEnum.CHANGE_INT_BODY_COLOR, ChangeMaterial);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnDisable()
	{
		Messenger<bool>.RemoveListener(MessengerEventsEnum.CHANGE_INT_BODY_COLOR, ChangeMaterial);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void ChangeMaterial(bool bChangeColor)
	{
		if(Globals.instance != null)
			m_renderer.sharedMaterial = bChangeColor ? Globals.instance.current_material_int_advance_style : default_mat;
	}
}
