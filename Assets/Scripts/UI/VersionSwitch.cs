using UnityEngine;

public class VersionSwitch : MonoBehaviour
{
	private void OnEnable()
	{
		switch(Globals.instance.currentVersion)
		{
			case Globals.Version.ADVANCE:
				transform.GetChild(0).gameObject.SetActive(true);
				transform.GetChild(1).gameObject.SetActive(false);
				transform.GetChild(2).gameObject.SetActive(false);
				break;
			case Globals.Version.ADVANCE_STYLE:
				transform.GetChild(1).gameObject.SetActive(true);
				transform.GetChild(0).gameObject.SetActive(false);
				transform.GetChild(2).gameObject.SetActive(false);
				break;
			case Globals.Version.SPORT:
				transform.GetChild(2).gameObject.SetActive(true);
				transform.GetChild(0).gameObject.SetActive(false);
				transform.GetChild(1).gameObject.SetActive(false);
				break;
		}
	}

	private void OnDisable()
	{
		foreach(Transform t in transform)
		{
			t.gameObject.SetActive(false);
		}
	}
}
