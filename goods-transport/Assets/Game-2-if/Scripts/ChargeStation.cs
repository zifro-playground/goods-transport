using UnityEngine;
using UnityEngine.UI;

public class ChargeStation : MonoBehaviour
{
	public static ChargeStation Instance;

	public Transform ChargeArm;
	public Text Display;

	public float RotationSpeed = 50f;
	private Vector3 targetDir;

	private bool rotatingChargeArm;
	private bool armIsDown;
	private bool charging;
	private bool checkingBattery;

	private const int FullBatteryLevel = 100;
	private int currentCarBatteryLevel;

	private void Start()
	{
		Instance = this;
	}

	private void Update()
	{
		if (rotatingChargeArm)
		{
			float gameSpeedExp = MyLibrary.LinearToExponential(0, 0.5f, 5, PMWrapper.speedMultiplier);
			float step = RotationSpeed * gameSpeedExp * Time.deltaTime;
			
			Vector3 newDir = Vector3.RotateTowards(ChargeArm.forward, targetDir, step, 0);

			ChargeArm.rotation = Quaternion.LookRotation(newDir);

			if(Vector3.Angle(ChargeArm.forward, targetDir) < 0.5f)
			{
				rotatingChargeArm = false;
				
				if (!charging && !armIsDown)
				{
					checkingBattery = false;
					Display.text = "";
					PMWrapper.UnpauseWalker();
				}

				if (!charging && armIsDown)
					checkingBattery = true;
			}
		}

		if (charging && !rotatingChargeArm)
		{
			currentCarBatteryLevel++;
			Display.text = currentCarBatteryLevel.ToString();

			if (currentCarBatteryLevel >= FullBatteryLevel)
			{
				charging = false;
				RaiseChargeArm();
			}
		}

		if (checkingBattery)
		{
			Display.text = currentCarBatteryLevel.ToString();
			RaiseChargeArm();
		}
	}

	public void CheckBattery(bool shouldCharge)
	{
		currentCarBatteryLevel = CarQueue.GetFirstCar().GetComponent<CarInfo>().BatteryLevel;
		if (shouldCharge)
		{
			LowerChargeArm();
			charging = true;
		}
		else
		{
			LowerChargeArm();
		}
	}

	private void LowerChargeArm()
	{
		targetDir = -Vector3.forward;
		rotatingChargeArm = true;
		armIsDown = true;
	}

	private void RaiseChargeArm()
	{
		targetDir = Vector3.up;
		rotatingChargeArm = true;
		armIsDown = false;
	}
}
