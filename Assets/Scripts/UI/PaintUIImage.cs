using UnityEngine;
using UnityEngine.UI;

public class PaintUIImage : MonoBehaviour
{
	public Sprite[] paintImages;

	private Image thisImage;

	private void Awake()
	{
		Messenger.AddListener(MessengerEventsEnum.CHANGE_EXT_BODY_COLOR, ChangeUIImage);
		thisImage = GetComponent<Image>();
	}

	private void OnDestroy()
	{
		Messenger.RemoveListener(MessengerEventsEnum.CHANGE_EXT_BODY_COLOR, ChangeUIImage);
	}

	private void ChangeUIImage()
	{
		thisImage.sprite = Globals.instance.paintSprite;
	}
}
