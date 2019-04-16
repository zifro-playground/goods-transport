using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator))]
public class CarMovement : MonoBehaviour
{
	public static int operationsRunning;

	[FormerlySerializedAs("AnimationSpeed")]
	public float animationSpeed = 8f;

	[FormerlySerializedAs("QueueSpeed")]
	public float queueSpeed = 22f;

	Animator animator;
	bool isMovingByAnimation;

	bool isMovingByScript;

	Vector3 targetPosition;

	public void DriveLeft()
	{
		PlayAnimation("DriveLeft");
	}

	public void DriveStraight()
	{
		PlayAnimation("DriveStraight");
	}

	public void DriveRight()
	{
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
		operationsRunning++;
	}

	void Start()
	{
		animator = GetComponent<Animator>();
		animator.enabled = false;

		operationsRunning = 0;
	}

	void Update()
	{
		if (isMovingByAnimation)
		{
			if (PMWrapper.isCompilerUserPaused)
			{
				animator.speed = 0;
			}
			else
			{
				float gameSpeedExp = MyLibrary.LinearToExponential(0, 0.5f, 5, PMWrapper.speedMultiplier);
				animator.speed = animationSpeed * gameSpeedExp;
			}

			if (!AnimatorIsPlaying() && operationsRunning == 1)
			{
				PMWrapper.ResolveYield();
				isMovingByAnimation = false;
				operationsRunning = 0;
			}
		}

		if (isMovingByScript && !PMWrapper.isCompilerUserPaused)
		{
			float gameSpeedExp = MyLibrary.LinearToExponential(0, 0.5f, 5, PMWrapper.speedMultiplier);

			transform.Translate(new Vector3(0, 0, queueSpeed * gameSpeedExp * Time.deltaTime));

			if (Vector3.Distance(transform.position, targetPosition) < 2 * (queueSpeed * gameSpeedExp * Time.deltaTime))
			{
				transform.position = targetPosition;
				isMovingByScript = false;
				operationsRunning--;
			}
		}
	}

	bool AnimatorIsPlaying()
	{
		return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 &&
		       (animator.GetCurrentAnimatorStateInfo(0).IsName("DriveLeft") ||
		        animator.GetCurrentAnimatorStateInfo(0).IsName("DriveStraight") ||
		        animator.GetCurrentAnimatorStateInfo(0).IsName("DriveRight"));
	}

	void PlayAnimation(string animationName)
	{
		isMovingByAnimation = true;
		operationsRunning++;

		animator.enabled = true;
		animator.SetTrigger(animationName);
	}
}
