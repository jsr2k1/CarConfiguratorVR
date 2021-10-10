
public class AmbilightSwitchButtonController : ButtonController
{
	public override void OnButtonPress()
	{
		base.OnButtonPress();

		Globals.instance.bAmbilight = !Globals.instance.bAmbilight;
		Messenger.Broadcast(MessengerEventsEnum.BUTTON_SWITCH_AMBILIGHT_PRESSED);
		Messenger.Broadcast(MessengerEventsEnum.SWITCH_AMBILIGHT);
	}
}