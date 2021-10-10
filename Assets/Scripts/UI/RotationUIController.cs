using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class RotationUIController : MonoBehaviour {
	public InteractionCircularSlider rotationSlider;
	public Transform uiCar;
	public float initialYRotation;
	public float differenceYRotation;
	public float uiCarDifferenceYRotation;
	public GameObject soundGO;
	public float soundThreshold;

	private float savedDegrees;

	// Use this for initialization
	void Start () {
		//Comentado porque hace un extraño.
		Invoke("SetData", 0.1f);

		savedDegrees = 0;
	}
	
	// Update is called once per frame
	void Update () {
		uiCar.localEulerAngles = new Vector3(0,0, rotationSlider.SliderValue + uiCarDifferenceYRotation);
		
		float degs = -(rotationSlider.SliderValue + differenceYRotation);
		while(degs < 0)
		{
			degs += 360f;
		}
		while (degs > 360)
		{
			degs -= 360f;
		}

		Globals.instance.current_rotation = degs;
		//Debug.Log(rotationSlider.SliderValue);

		if(Mathf.Abs(uiCar.localEulerAngles.z - savedDegrees) > soundThreshold)
		{
			savedDegrees = uiCar.localEulerAngles.z;
			soundGO.SetActive(false);
			soundGO.SetActive(true);
		}
	}

	void SetData()
	{
		rotationSlider.SliderPercent = initialYRotation / 360;
	}
}
