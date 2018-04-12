using System;
using UnityEngine;
using UnityEngine.UI;

public class ChargeStation : MonoBehaviour
{
	public static ChargeStation Instance;

	public Transform Walls;
	public Text Display;

	public float WallSpeed = 50f;

	private bool isRasingWalls;
    private bool isLoweringWalls;
	private bool isCharging;

	private const int FullBatteryLevel = 100;

    private CarInfo currentCarInfo;

	private void Start()
	{
        if (Instance != null)
            throw new Exception("There should only be one instace of ChargeStation.");

		Instance = this;
	}

	private void Update()
	{
		if (PMWrapper.IsCompilerUserPaused)
			return;

		if (isRasingWalls)
			MoveWalls(Direction.Up);

        if (isLoweringWalls)
            MoveWalls(Direction.Down);

		if (isCharging)
			Charge();
	}

    private void MoveWalls(Direction direction)
    {
        var move = Vector3.zero;
        float gameSpeedExp = MyLibrary.LinearToExponential(0, 1f, 10, PMWrapper.speedMultiplier);

        if (direction == Direction.Up)
            move = Vector3.up * Time.deltaTime * gameSpeedExp * WallSpeed;
        else if (direction == Direction.Down)
            move = Vector3.down * Time.deltaTime * gameSpeedExp * WallSpeed;

        Walls.transform.Translate(move);

        if (direction == Direction.Up && Walls.transform.localPosition.z > 0.2f)
        {
            isRasingWalls = false;
            isCharging = true;
        }
        else if (direction == Direction.Down && Walls.transform.localPosition.z < -0.3f)
        {
            isLoweringWalls = false;
            PMWrapper.UnpauseWalker();
        }
    }

    private void Charge()
	{
        if (currentCarInfo.BatteryLevel < FullBatteryLevel)
        {
            currentCarInfo.BatteryLevel++;
            Display.text = currentCarInfo.BatteryLevel.ToString();
        }
        else
        {
            isCharging = false;
            isLoweringWalls = true;
        }
	}

	public void CheckBattery(int currentBatteryLevel)
	{
		Display.text = currentBatteryLevel.ToString();
        PMWrapper.UnpauseWalker();
	}

	public void ChargeBattery()
	{
        currentCarInfo = CarQueue.GetFirstCar().GetComponent<CarInfo>();
        isRasingWalls = true;
    }
}

enum Direction
{
    Up,
    Down
}
