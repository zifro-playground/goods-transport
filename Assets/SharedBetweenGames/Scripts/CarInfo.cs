using UnityEngine;
using UnityEngine.Serialization;

public class CarInfo : MonoBehaviour
{
	[FormerlySerializedAs("CargoType")]
	public string cargoType;

    [FormerlySerializedAs("StartBatteryLevel")]
    public int startBatteryLevel;
	[FormerlySerializedAs("BatteryLevel")]
	public int batteryLevel;
	[FormerlySerializedAs("ItemsInCar")]
	public int itemsInCar;

    [FormerlySerializedAs("HasBeenCharged")]
    public bool hasBeenCharged;

	private void OnEnable()
	{
		CarQueue.CARS.AddLast(gameObject);
        
        hasBeenCharged = false;
	}
}
