using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFloating : MonoBehaviour
{
	public float velocity = 4;
	public float separation = 0.5f;
	private Vector3 iniLocalPosition;
	private Vector3 finalLocalPosition;
	private Vector3 goalLocalPosition;

	// Use this for initialization
	void Start () {
		iniLocalPosition = transform.localPosition;
		finalLocalPosition = new Vector3(iniLocalPosition.x, iniLocalPosition.y, iniLocalPosition.z - separation);
		goalLocalPosition = iniLocalPosition;
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = new Vector3(iniLocalPosition.x, iniLocalPosition.y, Mathf.Lerp(transform.localPosition.z, goalLocalPosition.z, Time.deltaTime * 4));
	}

	public void SetFinalGoalLocalPosition(bool final)
	{
		if(final)
			goalLocalPosition = finalLocalPosition;
		else
			goalLocalPosition = iniLocalPosition;
	}

	public void ToggleGoalLocalPosition()
	{
		if (goalLocalPosition == iniLocalPosition)
			goalLocalPosition = finalLocalPosition;
		else
			goalLocalPosition = iniLocalPosition;
	}
}
