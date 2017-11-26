using UnityEngine;

public class CarMovement : MonoBehaviour 
{
	public float speed = 1;

	private bool isMoving = false;
	private float targetPos = 0;

	private void Update()
	{
		if (isMoving)
		{
			transform.Translate(-transform.right * PMWrapper.speedMultiplier * speed);
			if (transform.position.x < targetPos)
			{
				isMoving = false;
				PMWrapper.UnpauseWalker();
			}
		}
	}

	public void MoveForward(float distance)
	{
		targetPos = transform.position.x - distance;
		isMoving = true;
	}
}
