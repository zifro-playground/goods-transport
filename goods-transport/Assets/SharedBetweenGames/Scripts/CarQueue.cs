using System.Collections.Generic;
using UnityEngine;

public class CarQueue : MonoBehaviour
{
	public static LinkedList<GameObject> cars = new LinkedList<GameObject>();

	public static void DriveQueueForward(int lineNumber)
	{
		if (cars.Count == 0)
			PMWrapper.RaiseError(lineNumber, "Hittar ingen bil att köra framåt.");

		Bounds firstCarBounds = MyLibrary.CalculateBoundsInChildren(cars.First.Value);
		float firstCarLength = firstCarBounds.extents.x;
		float secondCarLength = 0;

		if (cars.First.Next != null)
		{
			Bounds secondCarBounds = MyLibrary.CalculateBoundsInChildren(cars.First.Next.Value);
			secondCarLength = secondCarBounds.extents.x;
		}

		LevelController controller = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
		float distance = firstCarLength + controller.carSpacing + secondCarLength;

		bool shouldDestroy = true;

		foreach (GameObject car in cars)
		{
			CarMovement carMovement = car.GetComponent<CarMovement>();
			if (carMovement != null)
			{
				carMovement.MoveForward(distance, shouldDestroy);
				shouldDestroy = false;
			}
		}
		cars.RemoveFirst();
	}

	public static GameObject GetFirstCar()
	{
		if (cars.Count == 0)
			return null;

		return cars.First.Value;
	}

	public static void RemoveAllCars()
	{
		foreach (GameObject obj in cars)
		{
			Destroy(obj);
		}
		cars.Clear();
	}
}
