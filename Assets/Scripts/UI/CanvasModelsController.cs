using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasModelsController : MonoBehaviour
{
	public Globals.Models model;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		transform.GetChild(0).gameObject.SetActive(Globals.instance.currentModel == model);
	}
}