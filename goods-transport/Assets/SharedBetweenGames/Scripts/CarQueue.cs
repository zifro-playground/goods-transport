using System.Collections.Generic;
using UnityEngine;

public class CarQueue : MonoBehaviour
{
	public static LinkedList<GameObject> Cars = new LinkedList<GameObject>();

	public static void DriveQueueForward(int lineNumber, Transform endTarget)
	{
		var carNode = Cars.Last;
		while (carNode != null)
		{
			CarMovement carMovement = carNode.Value.GetComponent<CarMovement>();
			if (carNode.Previous != null)
			{
				Transform target = carNode.Previous.Value.transform;
				carMovement.SetNavigationTarget(target, false);
			}
			else
			{
				carMovement.SetNavigationTarget(endTarget, true);
				Cars.RemoveFirst();
			}

			carNode = carNode.Previous;
		}
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
			if (obj.name != "TestCar")
				Destroy(obj);
		}
		Cars.Clear();
	}
}
