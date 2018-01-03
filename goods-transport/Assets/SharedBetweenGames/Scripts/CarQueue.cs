using System.Collections.Generic;
using UnityEngine;

public class CarQueue : MonoBehaviour
{
	public static LinkedList<GameObject> Cars = new LinkedList<GameObject>();

	public static void DriveQueueForward(int lineNumber)
	{
		if (Cars.Count == 0)
			PMWrapper.RaiseError(lineNumber, "Hittar ingen bil att köra framåt.");

		Bounds firstCarBounds = MyLibrary.CalculateBoundsInChildren(Cars.First.Value);
		float firstCarLength = firstCarBounds.extents.x;
		float secondCarLength = 0;

		if (Cars.First.Next != null)
		{
			Bounds secondCarBounds = MyLibrary.CalculateBoundsInChildren(Cars.First.Next.Value);
			secondCarLength = secondCarBounds.extents.x;
		}

		LevelController controller = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
		float distance = firstCarLength + controller.CarSpacing + secondCarLength;

		bool shouldDestroy = true;

		foreach (GameObject car in Cars)
		{
			CarMovement carMovement = car.GetComponent<CarMovement>();
			if (carMovement != null)
			{
				carMovement.MoveForward(distance, shouldDestroy);
				shouldDestroy = false;
			}
		}
		Cars.RemoveFirst();
	}

	public static GameObject GetFirstCar()
	{
		if (Cars.Count == 0)
			return null;

		return Cars.First.Value;
	}

	public static void RemoveAllCars()
	{
		foreach (GameObject obj in Cars)
		{
			Destroy(obj);
		}
		Cars.Clear();
	}
}
