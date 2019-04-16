using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChargeStation : MonoBehaviour
{
	public static ChargeStation instance;

	[FormerlySerializedAs("Walls")]
	public Transform walls;
	[FormerlySerializedAs("Display")]
	public Text display;

	[FormerlySerializedAs("WallSpeed")]
	public float wallSpeed = 50f;

	private bool isRaisingWalls;
    private bool isLoweringWalls;
	private bool isCharging;

	private const int FULL_BATTERY_LEVEL = 100;

    private CarInfo currentCarInfo;

	private void Start()
	{
        if (instance != null)
		{
			throw new Exception("There should only be one instance of ChargeStation.");
		}

		instance = this;
	}

	private void Update()
	{
		if (PMWrapper.isCompilerUserPaused)
		{
			return;
		}

		if (isRaisingWalls)
		{
			MoveWalls(Direction.Up);
		}

		if (isLoweringWalls)
		{
			MoveWalls(Direction.Down);
		}

		if (isCharging)
		{
			Charge();
		}
	}

    private void MoveWalls(Direction direction)
    {
        var move = Vector3.zero;
        float gameSpeedExp = MyLibrary.LinearToExponential(0, 1f, 10, PMWrapper.speedMultiplier);

        if (direction == Direction.Up)
		{
			move = Vector3.up * Time.deltaTime * gameSpeedExp * wallSpeed;
		}
		else if (direction == Direction.Down)
		{
			move = Vector3.down * Time.deltaTime * gameSpeedExp * wallSpeed;
		}

		walls.transform.Translate(move);

        if (direction == Direction.Up && walls.transform.localPosition.z > 0.2f)
        {
            isRaisingWalls = false;
            isCharging = true;
        }
        else if (direction == Direction.Down && walls.transform.localPosition.z < -0.3f)
        {
            isLoweringWalls = false;
            PMWrapper.ResolveYield();
        }
    }

    private void Charge()
	{
        if (currentCarInfo.batteryLevel < FULL_BATTERY_LEVEL)
        {
            currentCarInfo.batteryLevel++;
            display.text = currentCarInfo.batteryLevel.ToString();
        }
        else
        {
            isCharging = false;
            isLoweringWalls = true;
        }
	}

	public void CheckBattery(int currentBatteryLevel)
	{
		SceneController2_1.checkChargeCounter++;
		display.text = currentBatteryLevel.ToString();
	}

	public void ChargeBattery()
	{
        currentCarInfo = CarQueue.GetFirstCar().GetComponent<CarInfo>();
        isRaisingWalls = true;
    }
}

enum Direction
{
    Up,
    Down
}
