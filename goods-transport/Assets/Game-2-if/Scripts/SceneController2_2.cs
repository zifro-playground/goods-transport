using System.Collections.Generic;
using PM;
using UnityEngine;

public class SceneController2_2 : MonoBehaviour, ISceneController, IPMCompilerStopped
{
	public List<GameObject> leftQueue;
	public List<GameObject> rightQueue;
	public List<GameObject> forwardQueue;


	public void SetPrecode(Case caseData)
	{
		if (caseData.precode != null)
			PMWrapper.preCode = caseData.precode;
	}

	private void ResetQueues()
	{
		leftQueue.Clear();
		rightQueue.Clear();
		forwardQueue.Clear();
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
		string correctForwardType = LevelController.CaseData.correctSorting.forwardQueue.type;
		if (!CorrectQueue(correctForwardType, forwardQueue))
			return false;

		string correctRightType = LevelController.CaseData.correctSorting.rightQueue.type;
		if (!CorrectQueue(correctRightType, rightQueue))
			return false;

		string correctLeftType = LevelController.CaseData.correctSorting.leftQueue.type;
		if (!CorrectQueue(correctLeftType, leftQueue))
			return false;

		return true;
	}

	private bool CorrectQueue(string correctType, List<GameObject> queue)
	{
		foreach (GameObject car in queue)
		{
			string type = car.GetComponent<CarInfo>().CargoType;
			if ((type != correctType && correctType != "whatever") || correctType == "none")
			{
				PMWrapper.RaiseTaskError("Något blev felsorterat.");
				return false;
			}
		}
		return true;
	}
}
