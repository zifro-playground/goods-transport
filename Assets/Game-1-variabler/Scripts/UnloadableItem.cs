using UnityEngine;

public class UnloadableItem : MonoBehaviour
{
	public bool IsUnloading = false;
	public float UnloadingSpeed = 0.2f;
	
	void Update ()
	{
		float gameSpeedExp = MyLibrary.LinearToExponential(0, 0.5f, 5, PMWrapper.speedMultiplier);

		if (IsUnloading && !PMWrapper.isCompilerUserPaused)
		{
			transform.Translate(-transform.up * UnloadingSpeed * gameSpeedExp);
		}

		if (transform.position.y > 7)
		{
			PMWrapper.ResolveYield();
			Destroy(gameObject);
		}
	}
}
