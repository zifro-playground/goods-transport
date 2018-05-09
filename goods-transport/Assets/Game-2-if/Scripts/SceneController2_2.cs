using System.Collections.Generic;
using PM;
using UnityEngine;

public class SceneController2_2 : MonoBehaviour, ISceneController, IPMCompilerStopped, IPMCompilerStarted
{
	public static int CarsSorted;

	private int carsToSort;

	public void SetPrecode(Case caseData)
	{
		if (caseData.precode != null)
			PMWrapper.preCode = caseData.precode;
	}

	public void OnPMCompilerStarted()
	{
		CarsSorted = 0;
		carsToSort = LevelController.CaseData.cars.Count;
	}

	public void OnPMCompilerStopped(HelloCompiler.StopStatus status)
	{
		if (status == HelloCompiler.StopStatus.Finished)
		{
			if (CarsSorted < carsToSort)
				PMWrapper.RaiseTaskError("Alla varor sorterades inte.");
			else if (CorrectSorting())
				PMWrapper.SetCaseCompleted();
		}
		
		SortedQueue.ResetQueues();
	}

	private bool CorrectSorting()
	{
		string correctForwardType = LevelController.CaseData.correctSorting.forwardQueue.type;
		if (!CorrectQueue(correctForwardType, SortedQueue.ForwardQueue))
			return false;

		string correctRightType = LevelController.CaseData.correctSorting.rightQueue.type;
		if (!CorrectQueue(correctRightType, SortedQueue.RightQueue))
			return false;

		string correctLeftType = LevelController.CaseData.correctSorting.leftQueue.type;
		if (!CorrectQueue(correctLeftType, SortedQueue.LeftQueue))
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
		var typeDefinitions = LevelController.CaseData.correctSorting.typeDefinitions;

		if (typeDefinitions != null) { 
			foreach (var typeDefinition in typeDefinitions)
			{
				if (typeDefinition.types.Contains(type))
				{
					return typeDefinition.name;
				}
			}
		}

		return type;
	}
}
