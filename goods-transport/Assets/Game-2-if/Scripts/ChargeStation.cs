using UnityEngine;
using UnityEngine.UI;

public class ChargeStation : MonoBehaviour
{
	public static ChargeStation Instance;

	public Transform ChargeArm;
	public Text Display;

	public float RotationSpeed = 50f;
	private Vector3 targetDir;

	private bool isRotatingChargeArm;
	private bool armIsDown;
	private bool isCharging;
	private bool isCheckingBattery;
	private bool willCharge;

	private const int FullBatteryLevel = 100;
	private int currentCarBatteryLevel;

	private void Start()
	{
		Instance = this;
	}

	private void Update()
	{
		if (PMWrapper.IsCompilerUserPaused)
			return;

		if (isRotatingChargeArm)
			RotateArm();

		if (isCharging && !isRotatingChargeArm)
			Charge();

		if (isCheckingBattery)
			CheckBattery();
	}

	private void CheckBattery()
	{
		Display.text = currentCarBatteryLevel.ToString();
		
		if (!willCharge)
		{
			RaiseChargeArm();
		}
		else
		{
			PMWrapper.UnpauseWalker();
		}

		isCheckingBattery = false;
	}

	private void Charge()
	{
		currentCarBatteryLevel++;
		Display.text = currentCarBatteryLevel.ToString();

		if (currentCarBatteryLevel >= FullBatteryLevel)
		{
			isCharging = false;
			RaiseChargeArm();
		}
	}

	private void RotateArm()
	{
		float gameSpeedExp = MyLibrary.LinearToExponential(0, 0.5f, 5, PMWrapper.speedMultiplier);
		float step = RotationSpeed * gameSpeedExp * Time.deltaTime;

		Vector3 newDir = Vector3.RotateTowards(ChargeArm.forward, targetDir, step, 0);

		ChargeArm.rotation = Quaternion.LookRotation(newDir);

		if (Vector3.Angle(ChargeArm.forward, targetDir) < 0.5f)
		{
			isRotatingChargeArm = false;

			if (armIsDown)
			{
				ChargeArm.eulerAngles = new Vector3(180, 0, 180);
				isCheckingBattery = true;
			}
			else
			{
				ChargeArm.eulerAngles = new Vector3(270, 0, 180);
				Display.text = "";
				PMWrapper.UnpauseWalker();
			}
		}
	}

	private void LowerChargeArm()
	{
		targetDir = -Vector3.forward;
		isRotatingChargeArm = true;
		armIsDown = true;
	}

	private void RaiseChargeArm()
	{
		targetDir = Vector3.up;
		isRotatingChargeArm = true;
		armIsDown = false;
	}

	public void CheckBattery(bool shouldCharge)
	{
		currentCarBatteryLevel = CarQueue.GetFirstCar().GetComponent<CarInfo>().BatteryLevel;
		willCharge = shouldCharge;
		if (shouldCharge)
		{
			LowerChargeArm();
		}
		else
		{
			LowerChargeArm();
		}
	}

	public void ChargeBattery()
	{
		isCharging = true;
	}
}
