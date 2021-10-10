using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class LightingController : MonoBehaviour
{
	public PostProcessingProfile[] profile;
	public float[] ambientIntensities;
	public Material[] skyboxes;
	public PostProcessingBehaviour postpo;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		int index = PlayerPrefs.GetInt("scenario", 0);
		postpo.profile = profile[index-1];
		RenderSettings.skybox = skyboxes[index-1];
		RenderSettings.ambientIntensity = ambientIntensities[index-1];
		DynamicGI.UpdateEnvironment();
	}
}