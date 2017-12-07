using UnityEngine;

public class UnloadableItem : MonoBehaviour
{
	public bool isUnloading = false;
	public float unloadingSpeed = 0.2f;
	
	void Update ()
	{
		float gameSpeedExp = MyLibrary.LinearToExponential(0, 0.5f, 5, PMWrapper.speedMultiplier);

		if (isUnloading && !PMWrapper.isCompilerUserPaused)
			transform.Translate(-transform.up * unloadingSpeed * gameSpeedExp);

		if (transform.position.y > 7)
		{
			PMWrapper.UnpauseWalker();
			Destroy(gameObject);
		}
	}
}
