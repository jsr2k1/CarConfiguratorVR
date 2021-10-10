using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que se encarga de mostrar la UI, ocultarla y de dar funcionalidad a los botones back.
/// </summary>
public class UIManager : MonoBehaviour
{
	public GameObject[] objectsToAvoid;
	private List<GameObject> uiPanelsQueue;
	private int currentPanelIdx;

	private GameObject nextUiPanel;

	private void Awake ()
	{
		uiPanelsQueue = new List<GameObject>();
		foreach(Transform t in transform)
		{
			if (t.gameObject.activeSelf)
			{
				bool found = false;
				foreach(GameObject g in objectsToAvoid)
				{
					if(t.gameObject == g)
					{
						found = true;
						break;
					}
				}
				if (!found)
				{
					uiPanelsQueue.Add(t.gameObject);
					break;
				}
			}
		}
		currentPanelIdx = 0;
	}

	public void GoBack()
	{
		if (currentPanelIdx > 0)
		{
			currentPanelIdx--;
			Messenger.Broadcast(MessengerEventsEnum.GO_TO_PANEL);
			Invoke("UnlockInteraction", 0.3f);
			Invoke("GoBackDelayed", 0.1f);
		}
	}

	public void GoToPanel(GameObject uiPanel)
	{
		nextUiPanel = uiPanel;
		Messenger.Broadcast(MessengerEventsEnum.GO_TO_PANEL);
		Invoke("UnlockInteraction", 0.3f);
		Invoke("GoToPanelDelayed", 0.1f);
	}

	private void UnlockInteraction()
	{
		Messenger.Broadcast(MessengerEventsEnum.END_GO_TO_PANEL);
	}

	private void GoBackDelayed()
	{
		uiPanelsQueue[currentPanelIdx + 1].SetActive(false);
		uiPanelsQueue.RemoveAt(currentPanelIdx + 1);
		uiPanelsQueue[currentPanelIdx].SetActive(true);
	}

	private void GoToPanelDelayed()
	{
		uiPanelsQueue[currentPanelIdx++].SetActive(false);
		uiPanelsQueue.Add(nextUiPanel);
		uiPanelsQueue[currentPanelIdx].SetActive(true);
	}

	#if UNITY_EDITOR
	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Backspace))
		{
			GoBack();
		}
	}
	#endif
}
