using System.Collections.Generic;
using System.Linq;
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
	    string correctLeftType = LevelController.CaseData.correctSorting.leftQueue.type;
	    if (!CorrectQueue(correctLeftType, SortedQueue.LeftQueue, "åt vänster"))
	        return false;

	    string correctForwardType = LevelController.CaseData.correctSorting.forwardQueue.type;
	    if (!CorrectQueue(correctForwardType, SortedQueue.ForwardQueue, "rakt fram"))
			return false;

	    string correctRightType = LevelController.CaseData.correctSorting.rightQueue.type;
	    if (!CorrectQueue(correctRightType, SortedQueue.RightQueue, "åt höger"))
			return false;

	    return true;
	}

	private bool CorrectQueue(string correctType, List<GameObject> queue, string direction)
	{
	    if (!CorrectNumberOfCarsInQueue(correctType, queue, direction))
	        return false;

	    if (!CorrectTypesInQueue(correctType, queue, direction))
	        return false;

        return true;
	}

    private bool CorrectNumberOfCarsInQueue(string correctType, List<GameObject> queue, string direction)
    {
        if (correctType != "none" && correctType != "whatever")
        {
            var correctNumberOfCars = 0;
            foreach (var car in LevelController.CaseData.cars)
            {
                if (car.sections.First().type == correctType)
                    correctNumberOfCars++;
            }

            if (queue.Count < correctNumberOfCars)
            {
                PMWrapper.RaiseTaskError("För få varor sorterades " + direction);
                return false;
            }
            if (queue.Count > correctNumberOfCars)
            {
                PMWrapper.RaiseTaskError("För många varor sorterade " + direction);
                return false;
            }
        }
        return true;
    }

    private bool CorrectTypesInQueue(string correctType, List<GameObject> queue, string direction)
    {
        foreach (GameObject car in queue)
        {
            string cargoType = car.GetComponent<CarInfo>().CargoType;

            cargoType = FindTypeFromDefinition(cargoType);

            if ((cargoType != correctType && correctType != "whatever") || correctType == "none")
            {
                PMWrapper.RaiseTaskError("Något blev felsorterat i kön " + direction + ".");
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
