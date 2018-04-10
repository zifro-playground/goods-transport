using UnityEngine;

public class CarInfo : MonoBehaviour
{
	public string CargoType;

    public int StartBatteryLevel;
	public int BatteryLevel = 0;
	public int ItemsInCar;

    public bool HasBeenCharged;

	private void OnEnable()
	{
        CarQueue.ActiveCars.Add(gameObject);
		CarQueue.Cars.AddLast(gameObject);
        
        HasBeenCharged = false;
	}
}
