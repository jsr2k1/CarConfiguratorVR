using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PostProcesingController : MonoBehaviour
{
	public PostProcessingProfile[] profile;
	public PostProcessingBehaviour postpo;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		int index = PlayerPrefs.GetInt("scenario", 0);
		postpo.profile = profile[index-1];
	}
}