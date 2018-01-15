using System.Collections.Generic;
using PM;
using UnityEngine;

public class SceneController2_2 : MonoBehaviour, ISceneController, IPMCompilerStopped
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
		string correctForwardType = LevelController.CaseData.correctSorting.forwardQueue.type;
		if (!CorrectQueue(correctForwardType, ForwardQueue))
			return false;

		string correctRightType = LevelController.CaseData.correctSorting.rightQueue.type;
		if (!CorrectQueue(correctRightType, RightQueue))
			return false;

		string correctLeftType = LevelController.CaseData.correctSorting.leftQueue.type;
		if (!CorrectQueue(correctLeftType, LeftQueue))
			return false;

		return true;
	}

	private bool CorrectQueue(string correctType, List<GameObject> queue)
	{
		foreach (GameObject car in queue)
		{
			string cargoType = car.GetComponent<CarInfo>().CargoType;

			cargoType = FindTypeFromDefinition(cargoType);

			if ((cargoType != correctType && correctType != "whatever") || correctType == "none")
			{
				PMWrapper.RaiseTaskError("Något blev felsorterat.");
				return false;
			}
		}
		return true;
	}

	private string FindTypeFromDefinition(string type)
	{
		foreach (var typeDefinition in LevelController.CaseData.correctSorting.typeDefinitions)
		{
			if (typeDefinition.types.Contains(type))
			{
				return typeDefinition.name;
			}
		}

		return type;
	}
}
