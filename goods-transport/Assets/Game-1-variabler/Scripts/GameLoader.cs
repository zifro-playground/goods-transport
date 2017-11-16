using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using PM;

public class GameLoader : MonoBehaviour, IPMLevelChanged
{
	public string gameDataFileName;
	private GameData gameData;

	// Use this for initialization
	void Start ()
	{
		LoadGameData(gameDataFileName);
	}

	public void OnPMLevelChanged()
	{
	}

	public void LoadGameData(string fileName)
	{
		//Debug.Log(Application.streamingAssetsPath + path);
		//string fullPath = Path.Combine(Application.streamingAssetsPath, path);

		TextAsset jsonAsset = Resources.Load<TextAsset>(fileName);

		if (jsonAsset == null)
			throw new Exception("Could not find the file \"" + fileName + "\" that should contain game data in json format.");

		string jsonString = jsonAsset.text;

		gameData = JsonConvert.DeserializeObject<GameData>(jsonString);
	}

}
