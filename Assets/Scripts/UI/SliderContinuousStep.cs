using UnityEngine;
using Leap.Unity.Interaction;

public class SliderContinuousStep : MonoBehaviour
{
	private InteractionSlider slider1;
	private InteractionCircularSlider slider2;
	private int stepsH1, stepsV1, steps2;

	private void Start()
	{
		Invoke("SetData", 0.1f);
	}

	public void OnPress()
	{
		if (slider1 != null)
		{
			slider1.verticalSteps = 0;
			slider1.horizontalSteps = 0;
		}

		if (slider2 != null)
		{
			slider2.steps = 0;
		}
	}

	public void OnUnpress()
	{
		if (slider1 != null)
		{
			slider1.verticalSteps = stepsV1;
			slider1.horizontalSteps = stepsH1;
		}
		if (slider2 != null)
		{
			slider2.ForceCalculateSliderValues();
			slider2.steps = steps2;
		}
	}

	private void SetData()
	{
		slider1 = GetComponent<InteractionSlider>();

		if (slider1 != null)
		{
			stepsH1 = slider1.horizontalSteps;
			stepsV1 = slider1.verticalSteps;
			slider1.OnPress.AddListener(new UnityEngine.Events.UnityAction(OnPress));
			slider1.OnUnpress.AddListener(new UnityEngine.Events.UnityAction(OnUnpress));
		}

		slider2 = GetComponent<InteractionCircularSlider>();

		if (slider2 != null)
		{
			steps2 = slider2.steps;
			slider2.OnPress.AddListener(new UnityEngine.Events.UnityAction(OnPress));
			slider2.OnUnpress.AddListener(new UnityEngine.Events.UnityAction(OnUnpress));
		}
	}
}
