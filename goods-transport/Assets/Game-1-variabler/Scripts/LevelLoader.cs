using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelLoader : MonoBehaviour {

	public void LoadLevel(string path, int level)
    {
		Debug.Log(Application.streamingAssetsPath);
		string fullPath = Path.Combine(Application.streamingAssetsPath, path);

		if (!File.Exists(fullPath))
			throw new Exception("Could not find the file \"" + path + "\" that should contain game data in json format.");

		string jsonString = File.ReadAllText(path);

		GameData gameData = JsonUtility.FromJson<GameData>(jsonString);
		Debug.Log("Hej " + gameData.name + ". Du är " + gameData + " år gammal.");
	}
}