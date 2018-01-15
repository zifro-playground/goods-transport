using UnityEngine;

public class CarInfo : MonoBehaviour
{
	public string CargoType;
	public int BatteryLevel;
	public int ItemsInCar;

	private void OnEnable()
	{
		CarQueue.Cars.AddLast(gameObject);
	}
}
