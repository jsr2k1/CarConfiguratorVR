public class UpholsteryButtonController : ButtonController
{
	public Globals.Upholstery upholstery;

	public override void OnButtonPress()
	{
		base.OnButtonPress();

		Globals.instance.currentUpholstery = upholstery;
		Messenger.Broadcast(MessengerEventsEnum.CHANGE_INT_UPHOLSTERY);

		if(Globals.instance.currentVersion == Globals.Version.ADVANCE_STYLE){
			//Advance Style
			if(upholstery == Globals.Upholstery.ANTRACITA_AZUL_RAVENNA){
				Globals.instance.currentPaintBody = Globals.Paint.METAL_AZUL_RAVENNA;
				Globals.instance.current_material_int_advance_style = Globals.instance.car_materials_int_advance_style[0];
				Messenger<bool>.Broadcast(MessengerEventsEnum.CHANGE_INT_BODY_COLOR, true);
			}
			else if(upholstery == Globals.Upholstery.ANTRACITA_CURCUMA){
				Globals.instance.currentPaintBody = Globals.Paint.METAL_AMARILLO_CURCUMA;
				Globals.instance.current_material_int_advance_style = Globals.instance.car_materials_int_advance_style[1];
				Messenger<bool>.Broadcast(MessengerEventsEnum.CHANGE_INT_BODY_COLOR, true);
			}
			else if(upholstery == Globals.Upholstery.ANTRACITA_NARANJA_CALATEA){
				Globals.instance.currentPaintBody = Globals.Paint.METAL_NARANJA_CALATEA;
				Globals.instance.current_material_int_advance_style = Globals.instance.car_materials_int_advance_style[2];
				Messenger<bool>.Broadcast(MessengerEventsEnum.CHANGE_INT_BODY_COLOR, true);
			}
			else{
				Messenger<bool>.Broadcast(MessengerEventsEnum.CHANGE_INT_BODY_COLOR, false);
			}
		}
	}
}
