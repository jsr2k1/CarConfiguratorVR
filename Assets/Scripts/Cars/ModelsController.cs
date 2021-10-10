using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelsController : MonoBehaviour
{
	public GameObject[] models;

	void Awake()
	{
		foreach(GameObject obj in models){
			if(obj){
				obj.SetActive(true);
			}
		}
	}
}