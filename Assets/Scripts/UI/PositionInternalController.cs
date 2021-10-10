using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class PositionInternalController : MonoBehaviour {
	public InteractionSlider positionSlider;
    public GameObject soundGO;
	
	private bool m_enabled = false;

    private Globals.InternalPositions lastInternalPosition;
	private Globals.InternalPositions currentInternalPosition;

	private void Awake()
	{
		Messenger.AddListener(MessengerEventsEnum.CHANGE_GLOBAL_POS, OnChangeGlobalPos);
	}

	private void OnDestroy()
	{
		Messenger.RemoveListener(MessengerEventsEnum.CHANGE_GLOBAL_POS, OnChangeGlobalPos);
	}

	void Start ()
	{
		//SetPosition(ref lastInternalPosition);
		lastInternalPosition = Globals.InternalPositions.FRONT_LEFT;
		currentInternalPosition = Globals.InternalPositions.FRONT_LEFT;
	}

	void OnEnable()
	{
		//SetPosition(ref lastInternalPosition);
		lastInternalPosition = Globals.InternalPositions.FRONT_LEFT;
		currentInternalPosition = Globals.InternalPositions.FRONT_LEFT;
	}

	void Update ()
	{
		if(m_enabled)
			currentInternalPosition = SetPosition();
		//Debug.Log(currentInternalPosition);
		//Debug.Log(lastInternalPosition);
		if(currentInternalPosition != lastInternalPosition)
		{
			Globals.instance.internalPosition = lastInternalPosition = currentInternalPosition;

			Messenger.Broadcast(MessengerEventsEnum.CHANGE_INT_POS);

			soundGO.SetActive(false);
			soundGO.SetActive(true);
}
	}

	private Globals.InternalPositions SetPosition()
	{
		if (positionSlider.HorizontalSliderValue == 0 && positionSlider.VerticalSliderValue == 1)
		{
			return Globals.InternalPositions.FRONT_LEFT;
			//Debug.Log("Asiento del piloto");
		}
		else if (positionSlider.HorizontalSliderValue == 1 && positionSlider.VerticalSliderValue == 1)
		{
			return Globals.InternalPositions.FRONT_RIGHT;
			//Debug.Log("Asiento del copiloto");
		}
		else if (positionSlider.HorizontalSliderValue == 0 && positionSlider.VerticalSliderValue == 0)
		{
			return Globals.InternalPositions.REAR_LEFT;
			//Debug.Log("Asiento de detrás del piloto");
		}
		else// if (positionSlider.HorizontalSliderValue == 1 && positionSlider.VerticalSliderValue == 0)
		{
			return Globals.InternalPositions.REAR_RIGHT;
			//Debug.Log("Asiento de detrás del copiloto");
		}
	}

	private void OnChangeGlobalPos()
	{
		if(Globals.instance.camPosition != Globals.CamPositions.EXTERIOR)
		{
			m_enabled = false;
		}
	}

	public void Enable(bool b)
	{
		m_enabled = b;
	}
}
