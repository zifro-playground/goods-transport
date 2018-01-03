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
			bool correct = CorrectSorting();

			if (correct)
				PMWrapper.SetCaseCompleted();
		}
		


		ResetQueues();
	}

	private bool CorrectSorting()
	{
		string correctForwardType = LevelController.CaseData.correctSorting.forwardType;
		foreach (GameObject car in forwardQueue)
		{
			string type = car.GetComponent<CarInfo>().cargoType;
			if (type != correctForwardType)
			{
				PMWrapper.RaiseTaskError("Något blev felsorterat.");
				return false;
			}
		}
		return true;
	}
}
