public class DayNightButtonController : ButtonController
{
	public override void OnButtonPress()
	{
		base.OnButtonPress();

		Globals.instance.isNight = !Globals.instance.isNight;
		Messenger.Broadcast(MessengerEventsEnum.CHANGE_NIGHT);
		Messenger.Broadcast(MessengerEventsEnum.BUTTON_NIGHT_PRESSED);
	}
}