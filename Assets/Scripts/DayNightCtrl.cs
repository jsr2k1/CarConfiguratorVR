using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;

public class DayNightCtrl : MonoBehaviour
{
	public Material mat_day;
	public Material mat_night;
	public Material mat_picto;

	public Color picto_day;
	public Color picto_night;

	public PostProcessingProfile post_day;
	public PostProcessingProfile post_night;

	public float ambient_intensity_day = 1F;
	public float ambient_intensity_night = 1F;

	GameObject scenario_day;
	GameObject scenario_night;

	PostProcessingBehaviour post_behaviour;

	Scene scene_day;
	Scene scene_night;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		Messenger.AddListener(MessengerEventsEnum.CHANGE_NIGHT, OnChangeNight);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		StartCoroutine(LoadDay("Tunel_Day", "G_Scenario_Day"));
		StartCoroutine(LoadNight("Tunel_Night", "G_Scenario_Night"));

		scenario_day = GameObject.Find("G_Scenario_Day");
		scenario_night = GameObject.Find("G_Scenario_Night");

		post_behaviour = Globals.instance.main_camera.gameObject.GetComponent<PostProcessingBehaviour>();

		scene_day = SceneManager.GetSceneByName("Tunel_Day");
		scene_night = SceneManager.GetSceneByName("Tunel_Night");

		mat_picto.SetColor("_EmissionColor", picto_day);

		Globals.instance.hasDayNight = true;
		Globals.instance.main_camera.enabled = false;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnDestroy()
	{
		Messenger.RemoveListener(MessengerEventsEnum.CHANGE_NIGHT, OnChangeNight);
		mat_picto.SetColor("_EmissionColor", picto_day);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	IEnumerator LoadDay(string scene_name, string obj_name)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene_name, LoadSceneMode.Additive);

		while(!asyncLoad.isDone){
			yield return null;
		}
		scenario_day = GameObject.Find(obj_name);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	IEnumerator LoadNight(string scene_name, string obj_name)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene_name, LoadSceneMode.Additive);

		while(!asyncLoad.isDone){
			yield return null;
		}
		scenario_night = GameObject.Find(obj_name);
		OnChangeNight();

		Globals.instance.main_camera.enabled = true;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnChangeNight()
	{
		SceneManager.SetActiveScene(Globals.instance.isNight ? scene_night : scene_day);

		if(scenario_day)
			scenario_day.SetActive(!Globals.instance.isNight);
		if(scenario_night)
			scenario_night.SetActive(Globals.instance.isNight);

		post_behaviour.profile = Globals.instance.isNight ? post_night : post_day;
		mat_picto.SetColor("_EmissionColor", Globals.instance.isNight ? picto_night : picto_day);

		RenderSettings.skybox = Globals.instance.isNight ? mat_night : mat_day;
		RenderSettings.ambientIntensity = Globals.instance.isNight ? ambient_intensity_night : ambient_intensity_day;
		DynamicGI.UpdateEnvironment();
	}
}