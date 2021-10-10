using UnityEngine;
using UnityEngine.UI;

public class VersionUIImage : MonoBehaviour
{
	public Sprite[] sprites;

	private Image thisImage;

	private void Awake()
	{
		Messenger.AddListener(MessengerEventsEnum.CHANGE_GLOBAL_RLINE, ChangeUIImage);
		thisImage = GetComponent<Image>();
	}

	private void OnDestroy()
	{
		Messenger.RemoveListener(MessengerEventsEnum.CHANGE_GLOBAL_RLINE, ChangeUIImage);
	}

	private void ChangeUIImage()
	{
		thisImage.sprite = sprites[(int)Globals.instance.currentVersion];
	}
}
