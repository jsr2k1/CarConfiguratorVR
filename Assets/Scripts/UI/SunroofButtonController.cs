
public class SunroofButtonController : ButtonController
{
	public override void OnButtonPress()
	{
		base.OnButtonPress();

		Globals.instance.sunroof = !Globals.instance.sunroof;
		Messenger.Broadcast(MessengerEventsEnum.BUTTON_SUNROOF_PRESSED);
		Messenger.Broadcast(MessengerEventsEnum.CHANGE_GLOBAL_SUNROOF);
	}
}