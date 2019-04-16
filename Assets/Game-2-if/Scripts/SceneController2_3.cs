using System.Collections.Generic;
using GameData;
using PM;
using UnityEngine;

public class SceneController2_3 : MonoBehaviour, IPMCompilerStopped, IPMCaseSwitched
{
    static SceneController2_3()
    {
        Main.RegisterFunction(new DriveStraight());
        Main.RegisterFunction(new DriveLeft());
        Main.RegisterFunction(new DriveRight());
        Main.RegisterFunction(new ScanPalms());
        Main.RegisterFunction(new ScanLamps());
        Main.RegisterFunction(new ScanTrees());
    }

    private GoodsCaseDefinition caseDef;


    public void OnPMCaseSwitched(int caseNumber)
    {
        caseDef = (GoodsCaseDefinition)PMWrapper.currentLevel.cases[caseNumber].caseDefinition;
    }

    public void OnPMCompilerStopped(StopStatus status)
	{
		if (status == StopStatus.Finished)
		{
			if (CorrectSorting())
			{
				PMWrapper.SetCaseCompleted();
			}
		}

		SortedQueue.ResetQueues();
	}

	private bool CorrectSorting()
	{
	    var leftBounds = caseDef.correctSorting.leftQueue;
	    if (!CorrectQueue(leftBounds, SortedQueue.LeftQueue, "åt vänster"))
		{
			return false;
		}

		var forwardBounds = caseDef.correctSorting.forwardQueue;
        if (!CorrectQueue(forwardBounds, SortedQueue.ForwardQueue, "rakt fram"))
		{
			return false;
		}

		var rightBounds = caseDef.correctSorting.rightQueue;
        if (!CorrectQueue(rightBounds, SortedQueue.RightQueue, "åt höger"))
		{
			return false;
		}

		return true;
	}

	private bool CorrectQueue(CorrectSortedQueueData bounds, List<GameObject> queue, string direction)
	{
	    if (!CorrectNumberOfCarsInQueue(bounds, queue, direction))
		{
			return false;
		}

		if (!CorrectBoundsInQueue(bounds, queue, direction))
		{
			return false;
		}

		return true;
	}

    private bool CorrectNumberOfCarsInQueue(CorrectSortedQueueData bounds, List<GameObject> queue, string direction)
    {
        var correctNumberOfCars = 0;
        foreach (var car in caseDef.cars)
        {
            var itemsInCar = 0;
            foreach (var carSection in car.sections)
            {
                itemsInCar += carSection.itemCount;
            }

            if (itemsInCar <= bounds.upperBound && itemsInCar >= bounds.lowerBound)
			{
				correctNumberOfCars++;
			}
		}

        if (queue.Count < correctNumberOfCars)
        {
            PMWrapper.RaiseTaskError("För få tåg sorterades " + direction);
            return false;
        }
        if (queue.Count > correctNumberOfCars)
        {
            PMWrapper.RaiseTaskError("För många tåg sorterades " + direction);
            return false;
        }
        return true;
    }

    private bool CorrectBoundsInQueue(CorrectSortedQueueData bounds, List<GameObject> queue, string direction)
    {
        foreach (GameObject car in queue)
        {
            int itemsInCar = car.GetComponent<CarInfo>().ItemsInCar;

            if (itemsInCar > bounds.upperBound)
            {
                PMWrapper.RaiseTaskError("I tunneln " + direction + " hamnade tåg med för många varor");
                return false;
            }

            if (itemsInCar < bounds.lowerBound)
            {
                PMWrapper.RaiseTaskError("I tunneln " + direction + " hamnade tåg med för få varor");
                return false;
            }
        }
        return true;
    }
}
