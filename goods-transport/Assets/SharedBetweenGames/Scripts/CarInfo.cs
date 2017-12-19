using UnityEngine;

public class CarInfo : MonoBehaviour
{
	public string cargoType;
	public int batteryLevel;

	private void OnEnable()
	{
		CarQueue.cars.AddLast(gameObject);
	}
}
