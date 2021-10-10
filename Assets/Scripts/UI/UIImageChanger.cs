using UnityEngine;
using UnityEngine.UI;

public class UIImageChanger : MonoBehaviour
{
	public Sprite[] sprites;
	public MessengerEventsEnum messengerEvent;
	private Image thisImage;

	private int idx;

	private void Awake()
	{
		Messenger.AddListener(messengerEvent, ChangeUIImage);
		thisImage = GetComponent<Image>();
		idx = 0;
	}

	private void OnDestroy()
	{
		Messenger.RemoveListener(messengerEvent, ChangeUIImage);
	}

	private void ChangeUIImage()
	{
		idx++;
		idx = idx % 2;
		thisImage.sprite = sprites[idx];
	}
}
