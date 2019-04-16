using UnityEngine;
using UnityEngine.Serialization;

public class CarInfo : MonoBehaviour
{
	[FormerlySerializedAs("BatteryLevel")]
	public int batteryLevel;

	[FormerlySerializedAs("CargoType")]
	public string cargoType;

	[FormerlySerializedAs("HasBeenCharged")]
	public bool hasBeenCharged;

	[FormerlySerializedAs("ItemsInCar")]
	public int itemsInCar;

	[FormerlySerializedAs("StartBatteryLevel")]
	public int startBatteryLevel;

	void OnEnable()
	{
		CarQueue.CARS.AddLast(gameObject);

		hasBeenCharged = false;
	}
}
