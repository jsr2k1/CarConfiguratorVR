using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class PositionController : MonoBehaviour
{
	public InteractionSlider positionSlider;
	public float threshold;
    public GameObject soundGO;
    public float soundThreshold;

    private float lastValue;
    private float savedValueSound;

	void Start()
	{
		savedValueSound = lastValue = positionSlider.VerticalSliderValue;
	}
	
	void Update()
	{
		if (positionSlider.VerticalSliderValue - threshold >= lastValue)
		{
			lastValue = positionSlider.VerticalSliderValue;
			Globals.instance.move_forward = true;
			Globals.instance.move_backwards = false;
		}
		else if (positionSlider.VerticalSliderValue + threshold <= lastValue)
		{
			lastValue = positionSlider.VerticalSliderValue;
			Globals.instance.move_forward = false;
			Globals.instance.move_backwards = true;
		}

        if (Mathf.Abs(positionSlider.VerticalSliderValue - savedValueSound) > soundThreshold)
        {
            savedValueSound = positionSlider.VerticalSliderValue;
            soundGO.SetActive(false);
            soundGO.SetActive(true);
        }
    }
}
