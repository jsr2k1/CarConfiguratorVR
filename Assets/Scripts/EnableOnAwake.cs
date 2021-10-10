using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnAwake : MonoBehaviour
{
	public GameObject target;

	void Awake()
	{
		target.SetActive(true);
	}
}