using System;
using UnityEngine;
using Newtonsoft.Json;

public class LevelController : MonoBehaviour
{
	public string gameDataFileName;

	private GameData gameData;
	private Case caseData;


	public void LoadGameData()
	{
		//Debug.Log(Application.streamingAssetsPath + path);
		//string fullPath = Path.Combine(Application.streamingAssetsPath, path);

		TextAsset jsonAsset = Resources.Load<TextAsset>(gameDataFileName);

		if (jsonAsset == null)
			throw new Exception("Could not find the file \"" + gameDataFileName + "\" that should contain game data in json format.");

		string jsonString = jsonAsset.text;

		gameData = JsonConvert.DeserializeObject<GameData>(jsonString);
	}

	public void LoadLevel(int level, int caseNumber)
	{
		caseData = gameData.levels[level].cases[caseNumber];

		GameObject.FindGameObjectWithTag("SceneController").GetComponent<LevelBuilder>().LoadLevel(caseData);
	}

}
