using System.Collections.Generic;
using GameData;
using PM;
using UnityEngine;

public class SceneController2_3 : MonoBehaviour, IPMCompilerStopped, IPMCaseSwitched
{
	GoodsCaseDefinition caseDef;

	static SceneController2_3()
	{
		Main.RegisterFunction(new DriveStraight());
		Main.RegisterFunction(new DriveLeft());
		Main.RegisterFunction(new DriveRight());
		Main.RegisterFunction(new ScanPalms());
		Main.RegisterFunction(new ScanLamps());
		Main.RegisterFunction(new ScanTrees());
	}

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

	bool CorrectSorting()
	{
		CorrectSortedQueueData leftBounds = caseDef.correctSorting.leftQueue;
		if (!CorrectQueue(leftBounds, SortedQueue.LEFT_QUEUE, "åt vänster"))
		{
			return false;
		}

		CorrectSortedQueueData forwardBounds = caseDef.correctSorting.forwardQueue;
		if (!CorrectQueue(forwardBounds, SortedQueue.FORWARD_QUEUE, "rakt fram"))
		{
			return false;
		}

		CorrectSortedQueueData rightBounds = caseDef.correctSorting.rightQueue;
		if (!CorrectQueue(rightBounds, SortedQueue.RIGHT_QUEUE, "åt höger"))
		{
			return false;
		}

		return true;
	}

	bool CorrectQueue(CorrectSortedQueueData bounds, List<GameObject> queue, string direction)
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

	bool CorrectNumberOfCarsInQueue(CorrectSortedQueueData bounds, List<GameObject> queue, string direction)
	{
		int correctNumberOfCars = 0;
		foreach (CarData car in caseDef.cars)
		{
			int itemsInCar = 0;
			foreach (SectionData carSection in car.sections)
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

	bool CorrectBoundsInQueue(CorrectSortedQueueData bounds, List<GameObject> queue, string direction)
	{
		foreach (GameObject car in queue)
		{
			int itemsInCar = car.GetComponent<CarInfo>().itemsInCar;

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
