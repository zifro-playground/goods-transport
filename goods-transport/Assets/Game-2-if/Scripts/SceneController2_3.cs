using System.Collections.Generic;
using PM;
using UnityEngine;

public class SceneController2_3 : MonoBehaviour, ISceneController, IPMCompilerStopped
{
	public void SetPrecode(Case caseData)
	{
		if (caseData.precode != null)
			PMWrapper.preCode = caseData.precode;
	}

	public void OnPMCompilerStopped(HelloCompiler.StopStatus status)
	{
		if (status == HelloCompiler.StopStatus.Finished)
		{
			bool correctSorted = CorrectSorting();

			if (correctSorted)
				PMWrapper.SetCaseCompleted();
			else
				PMWrapper.RaiseTaskError("Något blev felsorterat.");
		}

		SortedQueue.ResetQueues();
	}

	private bool CorrectSorting()
	{
		var forwardBounds = LevelController.CaseData.correctSorting.forwardQueue;
		if (!CorrectQueue(forwardBounds, SortedQueue.ForwardQueue))
			return false;

		var rightBounds = LevelController.CaseData.correctSorting.rightQueue;
		if (!CorrectQueue(rightBounds, SortedQueue.RightQueue))
			return false;

		var leftBounds = LevelController.CaseData.correctSorting.leftQueue;
		if (!CorrectQueue(leftBounds, SortedQueue.LeftQueue))
			return false;

		return true;
	}

	private bool CorrectQueue(CorrectSortedQueue bounds, List<GameObject> queue)
	{
		foreach (GameObject car in queue)
		{
			int itemsInCar = car.GetComponent<CarInfo>().ItemsInCar;

			if (itemsInCar > bounds.upperBound || itemsInCar < bounds.lowerBound)
				return false;
		}
		return true;
	}
}
