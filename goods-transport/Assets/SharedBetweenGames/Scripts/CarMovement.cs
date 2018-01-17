using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class CarMovement : MonoBehaviour, IPMCompilerUserUnpaused, IPMCompilerUserPaused
{
	public float Speed = 22f;

	private bool isFirstCar;
	private bool shouldDestroy;
	private bool isMoving;
	private bool driveStraight;

	private NavMeshAgent agent;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		if (isMoving && !PMWrapper.IsCompilerUserPaused)
		{
			float gameSpeedExp = MyLibrary.LinearToExponential(0, 0.5f, 5, PMWrapper.speedMultiplier);
			agent.speed = Speed * gameSpeedExp;
			agent.acceleration = Speed * gameSpeedExp;

			if (driveStraight)
			{
				Vector3 tempPosition = transform.position;
				tempPosition.z = 0;
				transform.position = tempPosition;
			}

			if (Vector3.Distance(transform.position, agent.destination) < 0.5f)
			{
				isMoving = false;

				if (isFirstCar)
					PMWrapper.UnpauseWalker();

				if (shouldDestroy)
					agent.enabled = false;
			}
		}
	}

	public void SetNavigationTarget(Transform target, bool isFirst)
	{
		isMoving = true;
		isFirstCar = isFirst;
		shouldDestroy = isFirst;
		driveStraight = !isFirst;

		agent.updateRotation = isFirst;
		agent.SetDestination(target.position);
	}

	public void OnPMCompilerUserUnpaused()
	{
		float gameSpeedExp = MyLibrary.LinearToExponential(0, 0.5f, 5, PMWrapper.speedMultiplier);
		agent.speed = Speed * gameSpeedExp;
	}

	public void OnPMCompilerUserPaused()
	{
		print("Pause compiler");
		agent.speed = 0;
	}
}
