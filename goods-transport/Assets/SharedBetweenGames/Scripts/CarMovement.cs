using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class CarMovement : MonoBehaviour
{
	public float Speed = 0.3f;

	private bool shouldDestroy;
	private bool moveByUpdate;
	private Vector3 targetPos;

	private NavMeshAgent agent;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		if (moveByUpdate)
		{
			float gameSpeedExp = MyLibrary.LinearToExponential(0, 0.5f, 5, PMWrapper.speedMultiplier);

			transform.Translate(transform.right * Speed * gameSpeedExp);
			if (transform.position.x > targetPos.x)
			{
				moveByUpdate = false;
				transform.position = targetPos;
				PMWrapper.UnpauseWalker();
				if (shouldDestroy)
					Destroy(gameObject);
			}
		}
	}

	public void MoveForward(float distance, bool shouldDestroyAtTarget)
	{
		targetPos = transform.position;
		targetPos.x += distance;
		moveByUpdate = true;
		this.shouldDestroy = shouldDestroyAtTarget;
	}

	public void SetNavigationTarget(Transform target)
	{
		moveByUpdate = false;
		agent.SetDestination(target.position);
	}
}
