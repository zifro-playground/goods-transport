using UnityEngine;

public class CarInfo : MonoBehaviour
{
	public string CargoType;
	public int BatteryLevel;

	private void OnEnable()
	{
		CarQueue.Cars.AddLast(gameObject);
	}
}
