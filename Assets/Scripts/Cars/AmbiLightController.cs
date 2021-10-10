using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbiLightController : MonoBehaviour
{
	public Material mat_ambilight;
	public Material mat_ambilight_plane;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnEnable()
	{
		Messenger.AddListener(MessengerEventsEnum.CHANGE_AMBILIGHT, ChangeAmbilight);
		Messenger.AddListener(MessengerEventsEnum.SWITCH_AMBILIGHT, SwitchAmbilight);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnDisable()
	{
		Messenger.RemoveListener(MessengerEventsEnum.CHANGE_AMBILIGHT, ChangeAmbilight);
		Messenger.RemoveListener(MessengerEventsEnum.SWITCH_AMBILIGHT, SwitchAmbilight);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnDestroy()
	{
		mat_ambilight.SetColor("_EmissionColor", Color.black);
		mat_ambilight_plane.SetColor("_Color", Color.black);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void ChangeAmbilight()
	{
		if(Globals.instance.bAmbilight){
			Color c = Globals.instance.ambilight_colors[(int)Globals.instance.currentAmbiLight];
			mat_ambilight.SetColor("_EmissionColor", c);

			if(Globals.instance.bCockpitOn){
				Color plane_color = new Color(Mathf.Clamp(c.r, 0F, 1F), Mathf.Clamp(c.g, 0F, 1F), Mathf.Clamp(c.b, 0F, 1F));
				mat_ambilight_plane.SetColor("_Color", plane_color);
			}
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void SwitchAmbilight()
	{
		Color c = Globals.instance.ambilight_colors[(int)Globals.instance.currentAmbiLight];
		mat_ambilight.SetColor("_EmissionColor", Globals.instance.bAmbilight ? c : Color.black);

		if(Globals.instance.bCockpitOn){
			Color plane_color = new Color(Mathf.Clamp(c.r, 0F, 1F), Mathf.Clamp(c.g, 0F, 1F), Mathf.Clamp(c.b, 0F, 1F));
			mat_ambilight_plane.SetColor("_Color", Globals.instance.bAmbilight ? plane_color : Color.black);
		}
	}
}