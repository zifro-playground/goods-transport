using UnityEngine;

public class CarMovement : MonoBehaviour 
{
	public float speed = 0.3f;

	private bool shouldDestroy = false;
	private bool isMoving = false;
	private Vector3 targetPos;

	private void Update()
	{
		if (isMoving)
		{
			transform.Translate(transform.right * PMWrapper.speedMultiplier * speed);
			if (transform.position.x > targetPos.x)
			{
				isMoving = false;
				transform.position = targetPos;
				PMWrapper.UnpauseWalker();
				if (shouldDestroy)
					Destroy(gameObject);
			}
		}
	}

	public void MoveForward(float distance, bool shouldDestroy)
	{
		targetPos = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);
		isMoving = true;
		this.shouldDestroy = shouldDestroy;
	}
}
