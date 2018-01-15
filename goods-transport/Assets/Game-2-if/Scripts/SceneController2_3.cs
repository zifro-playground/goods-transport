using System.Collections.Generic;
using PM;
using UnityEngine;

public class SceneController2_3 : MonoBehaviour, ISceneController, IPMCompilerStopped
{
	public List<GameObject> LeftQueue;
	public List<GameObject> RightQueue;
	public List<GameObject> ForwardQueue;


	public void SetPrecode(Case caseData)
	{
		if (caseData.precode != null)
			PMWrapper.preCode = caseData.precode;
	}

	private void ResetQueues()
	{
		LeftQueue.Clear();
		RightQueue.Clear();
		ForwardQueue.Clear();
	}

	public void OnPMCompilerStopped(HelloCompiler.StopStatus status)
	{
		if (status == HelloCompiler.StopStatus.Finished)
		{
			bool correctSorted = CorrectSorting();

			if (correctSorted)
				PMWrapper.SetCaseCompleted();
		}

		ResetQueues();
	}

	private bool CorrectSorting()
	{
		var forwardBounds = LevelController.CaseData.correctSorting.forwardQueue;
		if (!CorrectQueue(forwardBounds, ForwardQueue))
			return false;

		var rightBounds = LevelController.CaseData.correctSorting.rightQueue;
		if (!CorrectQueue(rightBounds, RightQueue))
			return false;

		var leftBounds = LevelController.CaseData.correctSorting.leftQueue;
		if (!CorrectQueue(leftBounds, LeftQueue))
			return false;

		return true;
	}

	private bool CorrectQueue(SortedQueue bounds, List<GameObject> queue)
	{
		foreach (GameObject car in queue)
		{
			int itemsInCar = car.GetComponent<CarInfo>().ItemsInCar;

			if (itemsInCar <= bounds.upperBound && itemsInCar >= bounds.lowerBound)
			{
				PMWrapper.RaiseTaskError("Något blev felsorterat.");
				return false;
			}
		}
		return true;
	}
}
