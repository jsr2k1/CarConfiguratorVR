using UnityEngine;
using UnityEngine.UI;

public class VersionUICheckmark : MonoBehaviour
{
	public Globals.Version version;
	Image thisImage;

	private void Awake()
	{
		Messenger.AddListener(MessengerEventsEnum.CHANGE_GLOBAL_RLINE, ChangeUIImage);
		thisImage = GetComponent<Image>();
		ChangeUIImage();
	}

	private void OnDestroy()
	{
		Messenger.RemoveListener(MessengerEventsEnum.CHANGE_GLOBAL_RLINE, ChangeUIImage);
	}

	private void ChangeUIImage()
	{
		thisImage.enabled = (version == Globals.instance.currentVersion) ? true : false;
	}
}
