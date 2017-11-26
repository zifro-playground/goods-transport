using UnityEngine;

public class UnloadableItem : MonoBehaviour
{
	public bool isUnloading = false;
	public float unloadingSpeed = 3f;
	
	void Update ()
	{
		if (isUnloading)
			transform.Translate(-transform.up * unloadingSpeed * PMWrapper.speedMultiplier);

		if (transform.position.y > 3)
		{
			PMWrapper.UnpauseWalker();
			Destroy(gameObject);
		}
	}
}
