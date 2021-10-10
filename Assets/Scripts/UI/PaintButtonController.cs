using UnityEngine;
using UnityEngine.UI;
using Leap.Unity.Interaction;

public class PaintButtonController : MonoBehaviour
{
	public Globals.Paint paint_body;
	public Globals.Paint paint_roof;

	private Sprite sprite;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		GetComponent<InteractionButton>().OnPress.AddListener(() => OnClickPaintButton());
		sprite = transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnDestroy()
	{
		GetComponent<InteractionButton>().OnPress.RemoveListener(() => OnClickPaintButton());
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void OnClickPaintButton()
	{
		Globals.instance.currentPaintBody = paint_body;
		Globals.instance.currentPaintRoof = paint_roof;

		Globals.instance.paintSprite = sprite;

		Messenger.Broadcast(MessengerEventsEnum.CHANGE_EXT_BODY_COLOR);

		//Advance Style
		if(Globals.instance.currentVersion == Globals.Version.ADVANCE_STYLE){
			if(Globals.instance.currentUpholstery != Globals.Upholstery.CUARCITA_CERAMICA){
				Globals.instance.currentUpholstery = Globals.Upholstery.ANTRACITA_CERAMICA;
				Messenger.Broadcast(MessengerEventsEnum.CHANGE_INT_UPHOLSTERY);
			}
		}
	}
}