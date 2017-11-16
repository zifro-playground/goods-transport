using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class LevelLoader : MonoBehaviour {

	private void Start()
	{
		LoadLevel();
	}

	public void LoadLevel()
    {
		//Debug.Log(Application.streamingAssetsPath + path);
		//string fullPath = Path.Combine(Application.streamingAssetsPath, path);
		string path = "game1";

		TextAsset jsonAsset = Resources.Load<TextAsset>(path);

		if (jsonAsset == null)
			throw new Exception("Could not find the file \"" + path + "\" that should contain game data in json format.");

		string jsonString = jsonAsset.text;
		
		RootObject gameData = JsonConvert.DeserializeObject<RootObject>(jsonString);
		Debug.Log(gameData.game.levels.@case.cars.Count);
	}
}