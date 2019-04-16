using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using PM;
using UnityEngine.Serialization;

public class Scanner : MonoBehaviour, IPMCompilerStopped
{
	public static Scanner instance;

	[FormerlySerializedAs("DisplayText")]
	public Text displayText;
	[FormerlySerializedAs("Light")]
	public GameObject scannerLight;
	private Transform scanner;

	private Vector3 targetPos;
	private bool isScanning;
	private float speed = 1;
	
	void Start ()
	{
		instance = this;
		DisableScanner();
		scanner = scannerLight.transform;
	}
	
	void Update ()
	{
		if (isScanning)
		{
			float gameSpeedExp = MyLibrary.LinearToExponential(0, 0.5f, 5, PMWrapper.speedMultiplier);

			Vector3 targetDir = targetPos - scanner.position;
			float step = speed * gameSpeedExp * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards(scanner.forward, targetDir, step, 0);

			scanner.rotation = Quaternion.LookRotation(newDir);

			if (Vector3.Angle(scanner.forward, targetDir) < 5)
			{
				DisableScanner();
				int timeActivated = Mathf.Max(1, Mathf.CeilToInt(4 * (1 - PMWrapper.speedMultiplier)));
				StartCoroutine(ActivateDisplayForSeconds(timeActivated));
				PMWrapper.ResolveYield();
			}
		}
	}


	public void Scan(GameObject obj)
	{
		isScanning = true;

		Bounds carBounds = MyLibrary.CalculateBoundsInChildren(obj);
		float carFront = carBounds.max.x;
		float carRear = carBounds.min.x;

		float distanceToCar = scanner.position.y - obj.transform.position.y;
		float lightSpotAngle = scannerLight.GetComponent<Light>().spotAngle;
		float lightRadiusAtCar = Mathf.Tan(Mathf.Deg2Rad * (lightSpotAngle / 2)) * distanceToCar;

		scannerLight.SetActive(true);
		scanner.LookAt(new Vector3(carFront - lightRadiusAtCar + 1, 0, 0));
		targetPos = new Vector3(carRear + lightRadiusAtCar - 1, 0, 0);
	}

	public void SetDisplayText(string text)
	{
		displayText.text = text;
	}

	public void SetDisplayText(int text)
	{
		displayText.text = text.ToString();
	}

	private void DisableScanner()
	{
		isScanning = false;
		scannerLight.SetActive(false);
		displayText.gameObject.SetActive(false);
	}

	public IEnumerator ActivateDisplayForSeconds(int seconds)
	{
		displayText.gameObject.SetActive(true);

		yield return new WaitForSeconds(seconds);

		displayText.gameObject.SetActive(false);
	}

	public void OnPMCompilerStopped(StopStatus status)
	{
		DisableScanner();
		StopAllCoroutines();
	}
}
