using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using PM;

public class Scanner : MonoBehaviour, IPMCompilerStopped
{
	public static Scanner Instance;

	public Text DisplayText;
	public GameObject Light;
	private Transform scanner;

	private Vector3 targetPos;
	private bool isScanning;
	private float speed = 1;
	
	void Start ()
	{
		Instance = this;
		DisableScanner();
		scanner = Light.transform;
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
				PMWrapper.UnpauseWalker();
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
		float lightSpotAngle = Light.GetComponent<Light>().spotAngle;
		float lightRadiusAtCar = Mathf.Tan(Mathf.Deg2Rad * (lightSpotAngle / 2)) * distanceToCar;

		Light.SetActive(true);
		scanner.LookAt(new Vector3(carFront - lightRadiusAtCar + 1, 0, 0));
		targetPos = new Vector3(carRear + lightRadiusAtCar - 1, 0, 0);
	}

	public void SetDisplayText(string text)
	{
		DisplayText.text = text;
	}

	public void SetDisplayText(int text)
	{
		DisplayText.text = text.ToString();
	}

	private void DisableScanner()
	{
		isScanning = false;
		Light.SetActive(false);
		DisplayText.gameObject.SetActive(false);
	}

	public IEnumerator ActivateDisplayForSeconds(int seconds)
	{
		DisplayText.gameObject.SetActive(true);

		yield return new WaitForSeconds(seconds);

		DisplayText.gameObject.SetActive(false);
	}

	public void OnPMCompilerStopped(StopStatus status)
	{
		DisableScanner();
		StopAllCoroutines();
	}
}
