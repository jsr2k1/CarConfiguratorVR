using UnityEngine;

public class MenuObserver : MonoBehaviour
{
	public bool show = true;


	private void Update ()
	{
		if (!show && transform.lossyScale.x > 0.015f)
		{
			show = true;
			//Debug.Log(show);
			Messenger.Broadcast(MessengerEventsEnum.SHOW_MENU);
			//this.enabled = false;
		}
		else if(show && transform.lossyScale.x < 0.001f)
		{
			show = false;
			//Debug.Log(show);
		}
	}
}
