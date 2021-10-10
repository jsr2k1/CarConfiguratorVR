using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelCtrl : MonoBehaviour
{
	public Globals.Models model;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		foreach(Transform child in transform){
			child.gameObject.SetActive(model == Globals.instance.currentModel);
		}
	}
}