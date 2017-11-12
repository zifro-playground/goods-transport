using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour, IPMLevelChanged
{
	private string currentScene;

	void Start ()
	{
		SceneManager.LoadSceneAsync("Scene1", LoadSceneMode.Additive);
		currentScene = "Scene1";
	}
	
	public void OnPMLevelChanged()
	{
		if (PMWrapper.currentLevel == 8)
		{
			if (currentScene == "Scene1")
				SceneManager.UnloadSceneAsync("Scene1");
			else if (currentScene == "Scene3")
				SceneManager.UnloadSceneAsync("Scene3");

			SceneManager.LoadSceneAsync("Scene2", LoadSceneMode.Additive);
			currentScene = "Scene2";
		}
	}
}
