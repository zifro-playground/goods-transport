using System.Collections.Generic;
using UnityEngine;

public class CarQueue : MonoBehaviour
{
	public static LinkedList<GameObject> Cars = new LinkedList<GameObject>();

	public static void DriveQueueForward()
	{
		var carNode = Cars.Last;
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
		Cars.RemoveFirst();
	}

	public static void DriveFirstCarStraight()
	{
		GetFirstCar().GetComponent<CarMovement>().DriveStraight();
		Cars.RemoveFirst();
	}

	public static void DriveFirstCarRight()
	{
		GetFirstCar().GetComponent<CarMovement>().DriveRight();
		Cars.RemoveFirst();
	}

	public static void DriveFirstCarShort()
	{
		GetFirstCar().GetComponent<CarMovement>().DriveShort();
		Cars.RemoveFirst();
	}

	public static GameObject GetFirstCar()
	{
		if (Cars.Count == 0)
        {
            PMWrapper.RaiseError("Hittade inget tåg.");
            return null;
        }

		return Cars.First.Value;
	}

	public static void RemoveAllCars()
	{
		foreach (GameObject obj in Cars)
		{
			if (obj.name != "TestCar")
				Destroy(obj);
		}
		Cars.Clear();
	}
}
