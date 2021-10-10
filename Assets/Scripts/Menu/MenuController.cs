using UnityEngine;

public class MenuController : MonoBehaviour
{
	public GameObject vrCanvas;
	public GameObject loadingCanvas;    

    public void LoadScene(int index)
	{
        if (index != 0)
            LogUseApp.SaveNewUser();

        if (vrCanvas)
			vrCanvas.SetActive(false);
		
		if(loadingCanvas)
			loadingCanvas.SetActive(true);

		UnityEngine.SceneManagement.SceneManager.LoadScene(index);
	}

	public void LoadScene()
	{
//		LoadScene(PlayerPrefs.GetInt("scenario", 0));
        LoadScene(1);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
