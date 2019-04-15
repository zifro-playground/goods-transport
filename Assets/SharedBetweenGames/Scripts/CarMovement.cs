using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class CarMovement : MonoBehaviour
{
	public float QueueSpeed = 22f;
	public float AnimationSpeed = 8f;

	[HideInInspector]
	public static int OperationsRunning;

	private bool isMovingByScript;
	private bool isMovingByAnimation;

	private Vector3 targetPosition;

	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
		animator.enabled = false;

		OperationsRunning = 0;
	}

	private void Update()
	{
		if (isMovingByAnimation)
		{
			if (PMWrapper.IsCompilerUserPaused)
			{
				animator.speed = 0;
			}
			else
			{
				float gameSpeedExp = MyLibrary.LinearToExponential(0, 0.5f, 5, PMWrapper.speedMultiplier);
				animator.speed = AnimationSpeed * gameSpeedExp;
			}

			if (!AnimatorIsPlaying() && OperationsRunning == 1)
			{
				PMWrapper.UnpauseWalker();
				isMovingByAnimation = false;
				OperationsRunning = 0;
			}
		}

		if (isMovingByScript && !PMWrapper.IsCompilerUserPaused)
		{
			float gameSpeedExp = MyLibrary.LinearToExponential(0, 0.5f, 5, PMWrapper.speedMultiplier);

			transform.Translate(new Vector3(0, 0, QueueSpeed * gameSpeedExp * Time.deltaTime));

			if (Vector3.Distance(transform.position, targetPosition) < 2 * (QueueSpeed * gameSpeedExp * Time.deltaTime))
			{
				transform.position = targetPosition;
				isMovingByScript = false;
				OperationsRunning--;
			}
		}
	}

	private bool AnimatorIsPlaying()
	{
		return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 &&
		       (animator.GetCurrentAnimatorStateInfo(0).IsName("DriveLeft") ||
		        animator.GetCurrentAnimatorStateInfo(0).IsName("DriveStraight") ||
		        animator.GetCurrentAnimatorStateInfo(0).IsName("DriveRight"));
	}

	private void PlayAnimation(string animationName)
	{
		isMovingByAnimation = true;
		OperationsRunning++;

		animator.enabled = true;
		animator.SetTrigger(animationName);
	}

	public void DriveLeft()
	{
		SceneController2_2.CarsSorted++;
		PlayAnimation("DriveLeft");
	}

	public void DriveStraight()
	{
		SceneController2_2.CarsSorted++;
		PlayAnimation("DriveStraight");
	}

	public void DriveRight()
	{
		SceneController2_2.CarsSorted++;
		PlayAnimation("DriveRight");
	}

	public void DriveShort()
	{
		PlayAnimation("DriveShort");
	}

	public void DriveForward(Transform target)
	{
		targetPosition = target.position;
		isMovingByScript = true;
		OperationsRunning++;
	}
}
