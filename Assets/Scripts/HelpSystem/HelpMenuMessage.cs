using UnityEngine;

public class HelpMenuMessage : MonoBehaviour
{
	public float timeToHelp = 10f;

	private void Awake()
	{
		Messenger.AddListener(MessengerEventsEnum.SHOW_MENU, DisableThis);
		transform.localPosition = new Vector3(0, 0, 0.45f);
	}

	private void OnDestroy()
	{
		Messenger.RemoveListener(MessengerEventsEnum.SHOW_MENU, DisableThis);
	}

	private void Update ()
	{
		timeToHelp -= Time.deltaTime;

		if(timeToHelp < 0)
		{
			transform.GetChild(0).gameObject.SetActive(true);
		}
	}

	private void DisableThis()
	{
		transform.GetChild(0).gameObject.SetActive(false);
		this.enabled = false;
	}
}
