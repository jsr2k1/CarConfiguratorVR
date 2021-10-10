using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
	IEnumerator Start()
	{
		Globals.instance.main_camera = Camera.main;
		Globals.instance.main_camera.enabled = false;

		int index = PlayerPrefs.GetInt("scenario", 2);
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index+1, LoadSceneMode.Additive);

		while(!asyncLoad.isDone){
			yield return null;
		}
		if(index != 4){
			Globals.instance.main_camera.enabled = true;
		}
	}
}