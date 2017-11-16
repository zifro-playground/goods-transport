using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour, IPMLevelChanged
{
    private LevelLoader loader;

	private LevelGroup[] allGroups = new LevelGroup[3]
	{
		new LevelGroup(0, 7, "Scene1"),
		new LevelGroup(8, 11, "Scene2"),
		new LevelGroup(12, 19, "Scene3")
	};
	private LevelGroup currentGroup;

	void Awake()
	{
		SceneManager.LoadSceneAsync("Scene1", LoadSceneMode.Additive);
		currentGroup = allGroups[0];
        loader = GetComponent<LevelLoader>();
	}
	
	public void OnPMLevelChanged()
	{
		LevelGroup newGroup = GetGroup(PMWrapper.currentLevel);

		if (currentGroup.sceneName != newGroup.sceneName)
		{
			SceneManager.UnloadSceneAsync(currentGroup.sceneName);
			SceneManager.LoadSceneAsync(newGroup.sceneName, LoadSceneMode.Additive);
			currentGroup = newGroup;
		}
        loader.LoadLevel("game1.json", PMWrapper.currentLevel);
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
