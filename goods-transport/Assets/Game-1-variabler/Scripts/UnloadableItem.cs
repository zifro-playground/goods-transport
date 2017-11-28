using UnityEngine;

public class UnloadableItem : MonoBehaviour
{
	public bool isUnloading = false;
	public float unloadingSpeed = 0.2f;
	
	void Update ()
	{
		if (isUnloading)
			transform.Translate(-transform.up * unloadingSpeed * PMWrapper.speedMultiplier);

		if (transform.position.y > 10)
		{
			PMWrapper.UnpauseWalker();
			Destroy(gameObject);
		}
	}
}
