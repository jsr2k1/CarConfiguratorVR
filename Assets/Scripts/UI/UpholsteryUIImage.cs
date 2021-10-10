using UnityEngine;
using UnityEngine.UI;

public class UpholsteryUIImage : MonoBehaviour
{
	public Sprite[] upholsteryImages;

	private Image thisImage;

	private void Awake()
	{
		Messenger.AddListener(MessengerEventsEnum.CHANGE_INT_UPHOLSTERY, ChangeUIImage);
		thisImage = GetComponent<Image>();
	}

	private void OnDestroy()
	{
		Messenger.RemoveListener(MessengerEventsEnum.CHANGE_INT_UPHOLSTERY, ChangeUIImage);
	}

	private void ChangeUIImage()
	{
		thisImage.sprite = upholsteryImages[(int)Globals.instance.currentUpholstery];
	}
}
