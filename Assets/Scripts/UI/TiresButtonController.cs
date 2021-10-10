public class TiresButtonController : ButtonController
{
	public Globals.Tires tires;

	public override void OnButtonPress()
	{
		base.OnButtonPress();

		Globals.instance.currentTires = tires;
		Messenger.Broadcast(MessengerEventsEnum.CHANGE_EXT_TIRES);
	}
}
