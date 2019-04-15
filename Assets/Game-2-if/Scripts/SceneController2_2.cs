using System.Collections.Generic;
using System.Linq;
using GameData;
using PM;
using UnityEngine;

public class SceneController2_2 : MonoBehaviour, IPMCompilerStopped, IPMCompilerStarted, IPMCaseSwitched
{
	static SceneController2_2()
	{
		Main.RegisterFunction(new ScanType());
		Main.RegisterFunction(new DriveStraight());
		Main.RegisterFunction(new DriveLeft());
		Main.RegisterFunction(new DriveRight());
	}

    public static int CarsSorted;

	private int carsToSort;
	private GoodsCaseDefinition caseDef;


	public void OnPMCaseSwitched(int caseNumber)
	{
		caseDef = (GoodsCaseDefinition)PMWrapper.currentLevel.cases[caseNumber].caseDefinition;
	}
	
	public void OnPMCompilerStarted()
	{
		CarsSorted = 0;
		carsToSort = caseDef.cars.Count;
	}

	public void OnPMCompilerStopped(StopStatus status)
	{
		if (status == StopStatus.Finished)
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
	    string correctLeftType = caseDef.correctSorting.leftQueue.type;
	    if (!CorrectQueue(correctLeftType, SortedQueue.LeftQueue, "åt vänster"))
	        return false;

	    string correctForwardType = caseDef.correctSorting.forwardQueue.type;
	    if (!CorrectQueue(correctForwardType, SortedQueue.ForwardQueue, "rakt fram"))
			return false;

	    string correctRightType = caseDef.correctSorting.rightQueue.type;
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
            foreach (var car in caseDef.cars)
            {
	            var currentType = FindTypeFromDefinition(car.sections.First().type);

				if (currentType == correctType)
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
		var typeDefinitions = caseDef.correctSorting.typeDefinitions;

		if (typeDefinitions != null) { 
			foreach (var typeDefinition in typeDefinitions)
			{
				//print("Type: " + type);
				if (typeDefinition.types.Contains(type))
				{
					return typeDefinition.name;
				}
			}
		}
		return type;
	}
}
