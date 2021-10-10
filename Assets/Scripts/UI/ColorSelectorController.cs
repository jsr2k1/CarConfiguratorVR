using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap.Unity.Interaction;

public class ColorSelectorController : MonoBehaviour {

	public InteractionCircularSlider circularSlider;
	public Image firstColorImage;
	public Color[] colors;
	public Transform colorCube;
	public Vector3 finalColorCubeLocalEulerAngles = new Vector3(-20.011f, -413.399f, 205.326f);
	public GameObject soundGO;
	public GameObject finalSoundGO;
	public bool useColorsasButtons;

	private ColorFloating[] floatingColors;
	private ColorFloating currentColor;
	private int steps;
	private Transform parent;
	private int lastColorIndex = 0;
	private int colorIndex = 0;
	private Vector3 baseColorCubeLocalEulerAngles;

	private void Awake()
	{
		float fillAmount = 1f / colors.Length;
		float degrees = 360 * fillAmount;

		int i = 0;
		parent = firstColorImage.transform.parent;


		foreach (Color c in colors)
		{
			Image img = Instantiate(firstColorImage);
			img.transform.SetParent(parent);

			img.color = c;

			img.fillAmount = fillAmount;
			img.rectTransform.localEulerAngles = new Vector3(0, 0, i++ * degrees);
			img.rectTransform.localScale = new Vector3(1, 1, 1);
			img.rectTransform.localPosition = new Vector3(0, 0, 0);

			if (useColorsasButtons)
			{
				int num = i;
				img.transform.GetChild(0).GetChild(0).GetComponent<InteractionButton>().OnPress.AddListener(() => { ChangeColorTo(num); });
			}
		}

		firstColorImage.transform.parent.eulerAngles = new Vector3(firstColorImage.transform.parent.eulerAngles.x, firstColorImage.transform.parent.eulerAngles.y, firstColorImage.transform.parent.eulerAngles.z + 90 - degrees/2);

		DestroyImmediate(firstColorImage.gameObject);

		int y = 0;
		floatingColors = new ColorFloating[parent.childCount];
		foreach(Transform child in parent)
		{
			floatingColors[y++] = child.GetComponent<ColorFloating>();
		}

		steps = circularSlider.steps = colors.Length;
		baseColorCubeLocalEulerAngles = colorCube.localEulerAngles;
		colorCube.GetComponent<Renderer>().material.color = colors[colorIndex];

		currentColor = floatingColors[0];
	}

	private void Update()
	{
		if (!circularSlider.isDepressed && !circularSlider.isGrasped)
		{
			circularSlider.ForceCalculateSliderValues();
			SelectColorIndexAndImages(true);
			ChangePaint();
		}
		else
		{
			SelectColorIndexAndImages();
		}
	}

	private void SelectColorIndexAndImages(bool setColorIndex = false)
	{
		float value = circularSlider.SliderPercent;

		float f = 0;
		foreach (ColorFloating color in floatingColors)
		{
			float min = (f / steps) - (1f / (2 * steps));
			float max = (f / steps) + (1f / (2 * steps));

			//if (setColorIndex)
				//Debug.Log(f + ": " + min + " < " + value + " < " + max);

			if (value > min && value < max || value > min +1  && value < max +1)
			{
				if(color != currentColor)
				{
					currentColor = color;
					soundGO.SetActive(false);
					soundGO.SetActive(true);
				}
				color.SetFinalGoalLocalPosition(true);
				color.transform.SetParent(circularSlider.transform);
				color.transform.SetParent(parent);
				
				if (setColorIndex)
				{
					colorIndex = (int)f;
				}
			}
			else
			{
				color.SetFinalGoalLocalPosition(false);
			}
			f += 1;
		}
	}

	public void ChangePaint()
	{
		//Debug.Log("l " + lastColorIndex);
		//Debug.Log("c " + colorIndex);

		if (lastColorIndex != colorIndex)
		{
//			Debug.Log(circularSlider.SliderPercent);

			finalSoundGO.SetActive(false);
			finalSoundGO.SetActive(true);

			StopCoroutine(Rotate());
			StartCoroutine(Rotate());

			//Joel -> No entiendo para que se usa esto
//			Globals.instance.current_material_body = Globals.instance.car_materials_body[colorIndex];
//			Globals.instance.current_material_roof = Globals.instance.car_materials_roof[colorIndex];
			Messenger.Broadcast(MessengerEventsEnum.CHANGE_EXT_BODY_COLOR);
			lastColorIndex = colorIndex;
		}
	}

	IEnumerator Rotate()
	{
		Material mat = colorCube.GetComponent<Renderer>().material;
		Color color = mat.color;
		float time = 0.5f;
		for (float f = 0f; f <= time; f += Time.deltaTime)
		{
			mat.color = new Color(Mathf.Lerp(color.r, colors[colorIndex].r, 1 / time * f), Mathf.Lerp(color.g, colors[colorIndex].g, 1 / time * f), Mathf.Lerp(color.b, colors[colorIndex].b, 1 / time * f), Mathf.Lerp(color.a, colors[colorIndex].a, 1 / time * f));
			colorCube.localEulerAngles = new Vector3(Mathf.Lerp(baseColorCubeLocalEulerAngles.x, finalColorCubeLocalEulerAngles.x, 1/time*f), Mathf.Lerp(baseColorCubeLocalEulerAngles.y, finalColorCubeLocalEulerAngles.y, 1/time*f), Mathf.Lerp(baseColorCubeLocalEulerAngles.z, finalColorCubeLocalEulerAngles.z, 1/time*f));
			yield return null;
		}
		mat.color = colors[colorIndex];
		colorCube.localEulerAngles = finalColorCubeLocalEulerAngles;
	}

	public void ChangeColorTo(int i)
	{
//		Debug.Log("cambio de color por botón: " + i);
		circularSlider.SliderPercent = (i-1) / (float)floatingColors.Length;
//		Debug.Log(circularSlider.SliderPercent);
		circularSlider.ForceCalculateSliderValues();
		SelectColorIndexAndImages(true);
		ChangePaint();
	}
}
