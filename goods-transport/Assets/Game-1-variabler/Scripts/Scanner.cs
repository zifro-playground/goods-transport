using PM;
using UnityEngine;

public class Scanner : MonoBehaviour, IPMCompilerStopped
{
	public GameObject scannerLamp;

	private Vector3 targetPos;
	private bool isScanning;
	private float speed = 1;
	
	void Start () 
	{
		scannerLamp.SetActive(false);
	}
	
	void Update ()
	{
		if (isScanning)
		{
			Vector3 targetDir = targetPos - scannerLamp.transform.position;
			float step = speed * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards(scannerLamp.transform.forward, targetDir, step, 0);
			
			scannerLamp.transform.rotation = Quaternion.LookRotation(newDir);
		}
	}

	public void Scan(GameObject obj)
	{
		isScanning = true;

		Bounds carBounds = obj.GetComponent<Renderer>().bounds;
		float carFront = carBounds.max.x;
		float carRear = carBounds.min.x;

		scannerLamp.SetActive(true);
		scannerLamp.transform.LookAt(new Vector3(carFront, 0, 0));
		targetPos = new Vector3(carRear, 0, 0);
	}

	public void OnPMCompilerStopped(HelloCompiler.StopStatus status)
	{

	}
}
