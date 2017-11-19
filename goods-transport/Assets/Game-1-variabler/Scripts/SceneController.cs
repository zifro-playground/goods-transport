using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour, IPMCaseSwitched
{
	private LevelController levelController;

	private LevelGroup[] allGroups = new LevelGroup[3]
	{
		new LevelGroup(0, 7, "Scene1"),
		new LevelGroup(8, 11, "Scene2"),
		new LevelGroup(12, 19, "Scene3")
	};
	private LevelGroup currentGroup;

	void Awake()
	{
		levelController = GetComponent<LevelController>();

		levelController.LoadGameData();

		SceneManager.LoadScene("Scene1", LoadSceneMode.Additive);
		currentGroup = allGroups[0];
	}
	
	public void OnPMCaseSwitched(int caseNumber)
	{
		LevelGroup newGroup = GetGroup(PMWrapper.currentLevel);

		if (currentGroup.sceneName != newGroup.sceneName)
		{
			SceneManager.UnloadSceneAsync(currentGroup.sceneName);
			SceneManager.LoadSceneAsync(newGroup.sceneName, LoadSceneMode.Additive);
			currentGroup = newGroup;
		}
		else
		{
			levelController.LoadLevel(PMWrapper.currentLevel, caseNumber);
		}
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
