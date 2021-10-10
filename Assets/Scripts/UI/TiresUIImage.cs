using UnityEngine;
using UnityEngine.UI;

public class TiresUIImage : MonoBehaviour
{
	public Sprite[] tireImages;

	private Image thisImage;

	private void Start()
	{
		Messenger.AddListener(MessengerEventsEnum.CHANGE_EXT_TIRES, ChangeUIImage);
		thisImage = GetComponent<Image>();
	}

	private void OnDestroy()
	{
		Messenger.RemoveListener(MessengerEventsEnum.CHANGE_EXT_TIRES, ChangeUIImage);
	}

	private void ChangeUIImage()
	{
		thisImage.sprite = tireImages[(int)Globals.instance.currentTires];
	}
}
