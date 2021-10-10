using UnityEngine;
using System.Collections;

public class InsideCtrl : MonoBehaviour
{
	public Globals.CamPositions cam_pos;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		foreach(Transform child in transform){
			child.gameObject.SetActive(Globals.instance.camPosition == cam_pos);
		}
	}
}
