using System.Collections;
using UnityEngine;

public class UIVersionController : MonoBehaviour
{
	public Sprite advanceSprite;
	public Sprite advanceStyleSprite;
	public Sprite sportSprite;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	private void Start()
	{
		switch (Globals.instance.currentVersion)
		{
			case Globals.Version.ADVANCE:
				ChangeToAdvance();
				break;
			case Globals.Version.ADVANCE_STYLE:
				ChangeToAdvanceStyle();
				break;
			case Globals.Version.SPORT:
				ChangeToSport();
				break;
			case Globals.Version.TOUAREG:
				ChangeToTouareg();
				break;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void ChangeToAdvance()
	{
		Globals.instance.currentVersion = Globals.Version.ADVANCE;
		Globals.instance.currentPaintBody = Globals.Paint.METAL_PLATA_CLARO;
		Globals.instance.currentPaintRoof = Globals.Paint.METAL_PLATA_CLARO;
		Globals.instance.paintSprite = advanceSprite;
		Globals.instance.currentTires = Globals.Tires.ADVANCE_MONTERO;
		Globals.instance.currentUpholstery = Globals.Upholstery.ANTRACITA_CERAMICA;

		Events();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void ChangeToAdvanceStyle()
	{
		Globals.instance.currentVersion = Globals.Version.ADVANCE_STYLE;
		Globals.instance.currentPaintBody = Globals.Paint.LISO_GRIS_URANO;
		Globals.instance.currentPaintRoof = Globals.Paint.LISO_BLANCO_PURO;
		Globals.instance.paintSprite = advanceStyleSprite;
		Globals.instance.currentTires = Globals.Tires.ADVANCE_STYLE_MAYFIELD;
		Globals.instance.currentUpholstery = Globals.Upholstery.ANTRACITA_CERAMICA;

		Events();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void ChangeToSport()
	{
		Globals.instance.currentVersion = Globals.Version.SPORT;
		Globals.instance.currentPaintBody = Globals.Paint.METAL_AZUL_RAVENNA;
		Globals.instance.currentPaintRoof = Globals.Paint.METAL_AZUL_RAVENNA;
		Globals.instance.paintSprite = sportSprite;
		Globals.instance.currentTires = Globals.Tires.SPORT_MONTERO;
		Globals.instance.currentUpholstery = Globals.Upholstery.ANTRACITA_CERAMICA_V2;

		Events();
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void ChangeToTouareg()
	{
		Globals.instance.currentPaintBody = Globals.Paint.METAL_AZUL_AGUAMARINA;
		Globals.instance.currentPaintRoof = Globals.Paint.METAL_AZUL_AGUAMARINA;
		Globals.instance.currentUpholstery = Globals.Upholstery.TOUAREG_CUERO_BASIC_NEGRO;

		Messenger.Broadcast(MessengerEventsEnum.CHANGE_VERSION);
		Messenger.Broadcast(MessengerEventsEnum.CHANGE_GLOBAL_RLINE);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	private void Events()
	{
		Messenger.Broadcast(MessengerEventsEnum.CHANGE_VERSION);
		Messenger.Broadcast(MessengerEventsEnum.CHANGE_EXT_BODY_COLOR);
		Messenger.Broadcast(MessengerEventsEnum.CHANGE_GLOBAL_RLINE);
		Messenger.Broadcast(MessengerEventsEnum.CHANGE_EXT_TIRES);
		Messenger.Broadcast(MessengerEventsEnum.CHANGE_INT_UPHOLSTERY);
		Messenger<bool>.Broadcast(MessengerEventsEnum.CHANGE_INT_BODY_COLOR, false);
	}
}
