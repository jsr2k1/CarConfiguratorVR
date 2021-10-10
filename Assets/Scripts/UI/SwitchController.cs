using UnityEngine;
using Leap.Unity.Interaction;

public class SwitchController : MonoBehaviour
{
	public Material[] materials;
	public MessengerEventsEnum messengerEvent;
	public MessengerEventsEnum receiveBtnEvent;

	private InteractionSlider _switch;
	private bool state = false;
	private MeshRenderer switchBase;
//	private Collider switchInput;
	private int materialIdx;
	private AudioSource sound;

	private bool firstTime = true;

	private void Awake()
	{
		Messenger.AddListener(receiveBtnEvent, ToggleState);
		_switch = GetComponentInChildren<InteractionSlider>();
		switchBase = GetComponentInChildren<MeshRenderer>();
//		switchInput = GetComponentInChildren<Collider>();
		sound = GetComponentInChildren<AudioSource>();
		materialIdx = 0;
	}

	private void Update()
	{
		if(firstTime)
		{
			firstTime = false;
			switchBase.material = materials[materialIdx];
		}

		ChangeState((_switch.HorizontalSliderPercent == 0) ? false : true);
	}

	private void ChangeLook()
	{
		materialIdx++;
		materialIdx = materialIdx % 2;
		switchBase.material = materials[materialIdx];
	}

	private void ChangeState(bool newState, bool broadcast = true)
	{
		if (state != newState)
		{
			state = !state;

			sound.Play();

			ChangeLook();

			if (broadcast)
			{
				Globals.instance.sunroof = !Globals.instance.sunroof;
				Messenger.Broadcast(messengerEvent);
			}
		}
	}

	private void ToggleState()
	{
		state = !state;
		_switch.HorizontalSliderValue = (_switch.HorizontalSliderPercent == 0) ? 1 : 0;
		ChangeLook();
	}

	private void OnDestroy()
	{
		Messenger.RemoveListener(receiveBtnEvent, ToggleState);
	}
}
