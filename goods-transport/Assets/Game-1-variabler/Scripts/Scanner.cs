using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using PM;

public class Scanner : MonoBehaviour, IPMCompilerStopped
{
	public Text display;
	public GameObject scannerObject;
	private Transform scanner;

	private Vector3 targetPos;
	private bool isScanning;
	private float speed = 1;
	
	void Start () 
	{
		DisableScanner();
		scanner = scannerObject.transform;
	}
	
	void Update ()
	{
		if (isScanning)
		{
			Vector3 targetDir = targetPos - scanner.position;
			float step = speed * Time.deltaTime;
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

		Bounds carBounds = obj.GetComponent<Renderer>().bounds;
		float carFront = carBounds.max.x;
		float carRear = carBounds.min.x;

		float distanceToCar = scanner.position.y - obj.transform.position.y;
		float lightSpotAngle = scannerObject.GetComponent<Light>().spotAngle;
		float lightRadiusAtCar = Mathf.Tan(Mathf.Deg2Rad * (lightSpotAngle / 2)) * distanceToCar;

		scannerObject.SetActive(true);
		scanner.LookAt(new Vector3(carFront - lightRadiusAtCar + 1, 0, 0));
		targetPos = new Vector3(carRear + lightRadiusAtCar - 1, 0, 0);
	}

	public void SetDisplayText(string text)
	{
		display.text = text;
	}

	private void DisableScanner()
	{
		isScanning = false;
		scannerObject.SetActive(false);
		display.gameObject.SetActive(false);
	}

	public IEnumerator ActivateDisplayForSeconds(int seconds)
	{
		display.gameObject.SetActive(true);

		yield return new WaitForSeconds(seconds);

		display.gameObject.SetActive(false);
	}

	public void OnPMCompilerStopped(HelloCompiler.StopStatus status)
	{
		DisableScanner();
		StopAllCoroutines();
	}
}
