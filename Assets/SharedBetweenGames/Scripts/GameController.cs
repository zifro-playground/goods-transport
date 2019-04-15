using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.Collections;
using PM;

public class GameController : MonoBehaviour, IPMCaseSwitched, IPMCompilerStopped
{
	public string gameDataFileName;
	public LevelController levelController;
	public LevelGroup[] levelGroups;

	public Case caseData;
	private GameData gameData;

	[Serializable]
	public struct LevelGroup
	{
		public int startLevel;
		public int endLevel;
		public string sceneName;
	}

	private LevelGroup currentGroup;

	void Awake()
	{
		LoadGameData();

		SceneManager.LoadScene(levelGroups[0].sceneName, LoadSceneMode.Additive);
		currentGroup = levelGroups[0];
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
		LevelGroup newGroup = GetGroup(PMWrapper.currentLevelIndex);

		if (currentGroup.sceneName != newGroup.sceneName)
		{
			StartCoroutine(UnloadCurrentScene(newGroup, caseNumber));
			currentGroup = newGroup;
		}
		else
		{
			LoadCase(PMWrapper.currentLevelIndex, caseNumber);
		}
	}

	private IEnumerator UnloadCurrentScene(LevelGroup newGroup, int caseNumber)
	{
		AsyncOperation async = SceneManager.UnloadSceneAsync(currentGroup.sceneName);
		yield return async;
		StartCoroutine(LoadNextScene(newGroup, caseNumber));
	}

	private IEnumerator LoadNextScene(LevelGroup newGroup, int caseNumber)
	{
		AsyncOperation async = SceneManager.LoadSceneAsync(newGroup.sceneName, LoadSceneMode.Additive);
		yield return async;
		LoadCase(PMWrapper.currentLevelIndex, caseNumber);
	}

	private void LoadCase(int level, int caseNumber)
	{
		caseData = gameData.levels[level].cases[caseNumber];

		levelController.LoadCase(caseData);
	}

	private LevelGroup GetGroup(int level)
	{
		foreach (LevelGroup group in levelGroups)
		{
			if (level >= group.startLevel && level <= group.endLevel)
				return group;
		}
		throw new Exception("Current level number \"" + level + "\" does not fit into any existing group intervall");
	}

	public void OnPMCompilerStopped(StopStatus status)
	{
		LoadCase(PMWrapper.currentLevelIndex, 0);
	}
}
