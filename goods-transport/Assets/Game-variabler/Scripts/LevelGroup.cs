using UnityEngine;

public class LevelGroup
{
	public int startLevel;
	public int endLevel;
	public string sceneName;

	public LevelGroup (int startLevel, int endLevel, string sceneName)
	{
		this.startLevel = startLevel;
		this.endLevel = endLevel;
		this.sceneName = sceneName;
	}
}