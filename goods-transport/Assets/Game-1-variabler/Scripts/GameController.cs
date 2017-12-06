using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class GameController : MonoBehaviour, IPMCaseSwitched
{
	public string gameDataFileName;

	public Case caseData;
	private GameData gameData;

	private LevelGroup[] allGroups = new LevelGroup[3]
	{
		new LevelGroup(0, 6, "Scene1"),
		//new LevelGroup(4, 11, "Scene2"),
		new LevelGroup(7, 10, "Scene2"),
		//new LevelGroup(12, 19, "Scene3")
		new LevelGroup(11, 19, "Scene3")
	};
	private LevelGroup currentGroup;

	void Awake()
	{
		LoadGameData();

		SceneManager.LoadScene("Scene1", LoadSceneMode.Additive);
		currentGroup = allGroups[0];
	}

	private void LoadGameData()
	{
		//Debug.Log(Application.streamingAssetsPath + path);
		//string fullPath = Path.Combine(Application.streamingAssetsPath, path);

		TextAsset jsonAsset = Resources.Load<TextAsset>(gameDataFileName);

		if (jsonAsset == null)
			throw new Exception("Could not find the file \"" + gameDataFileName + "\" that should contain game data in json format.");

		string jsonString = jsonAsset.text;

		gameData = JsonConvert.DeserializeObject<GameData>(jsonString);
	}

	
	public void OnPMCaseSwitched(int caseNumber)
	{
		LevelGroup newGroup = GetGroup(PMWrapper.currentLevel);

		if (currentGroup.sceneName != newGroup.sceneName)
		{
			SceneManager.UnloadSceneAsync(currentGroup.sceneName);
			SceneManager.LoadScene(newGroup.sceneName, LoadSceneMode.Additive);
			LoadLevel(PMWrapper.currentLevel, caseNumber);
			currentGroup = newGroup;
		}
		else
		{
			LoadLevel(PMWrapper.currentLevel, caseNumber);
		}
	}

	private void LoadLevel(int level, int caseNumber)
	{
		caseData = gameData.levels[level].cases[caseNumber];

		GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().LoadLevel(caseData);
	}

	private LevelGroup GetGroup(int level)
	{
		foreach (LevelGroup group in allGroups)
		{
			if (level >= group.startLevel && level <= group.endLevel)
				return group;
		}
		throw new Exception("Current level number \"" + level + "\" does not fit into any existing group intervall");
	}
}
