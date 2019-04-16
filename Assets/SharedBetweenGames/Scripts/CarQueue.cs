using System.Collections.Generic;
using UnityEngine;

public class CarQueue : MonoBehaviour
{
	public static readonly LinkedList<GameObject> CARS = new LinkedList<GameObject>();

	public static void DriveQueueForward()
	{
		var carNode = CARS.Last;
		while (carNode != null)
		{
			CarMovement carMovement = carNode.Value.GetComponent<CarMovement>();
			if (carNode.Previous != null)
			{
				Transform target = carNode.Previous.Value.transform;
				carMovement.DriveForward(target);
			}

			carNode = carNode.Previous;
		}
	}

	public static void DriveFirstCarLeft()
	{
		GetFirstCar().GetComponent<CarMovement>().DriveLeft();
		CARS.RemoveFirst();
	}

	public static void DriveFirstCarStraight()
	{
		GetFirstCar().GetComponent<CarMovement>().DriveStraight();
		CARS.RemoveFirst();
	}

	public static void DriveFirstCarRight()
	{
		GetFirstCar().GetComponent<CarMovement>().DriveRight();
		CARS.RemoveFirst();
	}

	public static void DriveFirstCarShort()
	{
		GetFirstCar().GetComponent<CarMovement>().DriveShort();
		CARS.RemoveFirst();
	}

	public static GameObject GetFirstCar()
	{
		if (CARS.Count == 0)
        {
            PMWrapper.RaiseError("Hittade inget tåg.");
            return null;
        }

		return CARS.First.Value;
	}

	public static void RemoveAllCars()
	{
		foreach (GameObject obj in CARS)
		{
			if (obj.name != "TestCar")
			{
				Destroy(obj);
			}
		}
		CARS.Clear();
	}
}
