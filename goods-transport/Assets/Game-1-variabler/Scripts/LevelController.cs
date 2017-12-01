using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject carPlatformPrefab;
	public GameObject carFrontPrefab;
	public GameObject boxRowPrefab;
	public GameObject palmPrefab;
	public GameObject treePrefab;
	public GameObject lampPrefab;
	public GameObject tablePrefab;
	public GameObject chairPrefab;

	[Header("Distances")]
	public float carSpacing = 1;
	public float boxSpacing = 0.5f;
	public float carPadding = 0.4f;
	private float boxLength = 1; // is set from CreateAssets()

	[HideInInspector]
	public Case caseData;

	[HideInInspector]
	public LinkedList<GameObject> activeCars = new LinkedList<GameObject>();

	private Dictionary<string, GameObject> itemType;
	private void BuildItemDictionary()
	{
		if (itemType == null)
		{
			itemType = new Dictionary<string, GameObject>();

			itemType.Add("palmer", palmPrefab);
			itemType.Add("granar", treePrefab);
			itemType.Add("bord", tablePrefab);
			itemType.Add("lampor", lampPrefab);
			itemType.Add("stolar", tablePrefab);
		}
	}

	public void LoadLevel(Case caseData)
	{
		this.caseData = caseData;

		BuildItemDictionary();
		RemoveOldAssets();
		CreateAssets();
		SetPrecodeAndAnswer();
	}

	private void RemoveOldAssets()
	{
		foreach (GameObject obj in activeCars)
		{
			Destroy(obj);
		}
		activeCars.Clear();
	}

	private void CreateAssets()
	{
		float boxLength = boxRowPrefab.GetComponentInChildren<Renderer>().bounds.size.x;
		float previousCarPosition = 0;

		foreach (Car carData in caseData.cars)
		{
			GameObject carObj = new GameObject("Car");
			GameObject platform = Instantiate(carPlatformPrefab);
			GameObject front = Instantiate(carFrontPrefab);

			platform.transform.SetParent(carObj.transform);
			front.transform.SetParent(carObj.transform);

			RescaleCar(carData, platform, front);
			activeCars.AddLast(carObj);

			// Position car in queue
			float carPosX = previousCarPosition;
			carObj.transform.position = new Vector3(carPosX, 0, 0);

			// Place the rows of boxes and their items in car
			Bounds platformBounds = platform.GetComponent<Renderer>().bounds;
			Bounds frontBounds = front.GetComponent<Renderer>().bounds;
			front.transform.position = new Vector3(platformBounds.max.x, platformBounds.min.y, platformBounds.center.z);

			float carLength = platformBounds.size.x + frontBounds.size.x;
			float carWidthCenter = platformBounds.center.z;
			float carLeftEnd = platformBounds.min.x;
			float sectionLeftEnd = carLeftEnd;

			for (int i = 0; i < carData.sections.Count; i++)
			{
				Section section = carData.sections[i];
				Vector3[,] itemPositions = new Vector3[section.rows, 4];

				for (int j = 1; j <= section.rows; j++)
				{
					GameObject boxRow = Instantiate(boxRowPrefab);
					boxRow.transform.SetParent(carObj.transform);

					float rowTopEnd = boxRow.GetComponentInChildren<Renderer>().bounds.max.y;
					float rowCenter = sectionLeftEnd + carPadding + ((2 * (float)j - 1) / 2) * boxLength + (j - 1) * boxSpacing;
					
					for (int k = 1; k <= 4; k++)
					{
						float colCenter = platformBounds.min.z + carPadding + ((2 * (float)k - 1) / 2) * boxLength + (k - 1) * boxSpacing;
						itemPositions[j-1, k-1] = new Vector3(rowCenter, rowTopEnd, colCenter);
					}

					boxRow.transform.position = new Vector3(rowCenter, 0.5f, carWidthCenter);
				}
				PlaceItems(itemPositions, carObj, itemType[section.type], section.itemCount);

				float sectionLength = section.rows * boxLength + (section.rows - 1) * boxSpacing  + 2 * carPadding;
				sectionLeftEnd += sectionLength;
			}
			previousCarPosition = carObj.transform.position.x - carLength - carSpacing;
		}
	}
	private void SetAnswer()
	{
		PMWrapper.SetCaseAnswer(caseData.answer);
	}
	private void SetPrecode()
	{
		ISceneController[] sceneControllers = PM.UISingleton.FindInterfaces<ISceneController>();
		if (sceneControllers.Length > 1)
			throw new Exception("There are more than one class that implements ISceneController in current scene.");
		if (sceneControllers.Length < 1)
			throw new Exception("Could not find any class that implements ISceneController.");

		sceneControllers[0].SetPrecode(caseData);
	}

	private void RescaleCar(Car carData, GameObject carPlatform, GameObject carFront)
	{
		Vector3 platformSize = carPlatform.GetComponent<Renderer>().bounds.size;
		Vector3 frontSize = carFront.GetComponent<Renderer>().bounds.size;

		int sectionCount = 0;
		int rowsInCar = 0;
		int boxSpacingsNeeded = 0;

		foreach (Section section in carData.sections)
		{
			rowsInCar += section.rows;
			if (section.itemCount > 0)
			{
				sectionCount++;
				boxSpacingsNeeded += section.rows - 1;
			}
		}
		float newCarWidth = 4 * boxLength + 3 * boxSpacing + 2 * carPadding;
		float newPlatformLength = rowsInCar * boxLength + boxSpacingsNeeded * boxSpacing + 2 * sectionCount * carPadding;
		
		float scaleFactorLengthPlatform = newPlatformLength / platformSize.x;
		float scaleFactorWidthPlatform = newCarWidth / platformSize.z;
		float scaleFactorWidthFront = newCarWidth / frontSize.z;

		carPlatform.transform.localScale = new Vector3(scaleFactorLengthPlatform, 0.5f, scaleFactorWidthPlatform);
		carFront.transform.localScale = new Vector3(1.5f, 1.5f, scaleFactorWidthFront);
	}
	private void PlaceItems(Vector3[,] positionMatrix, GameObject parent, GameObject itemPrefab, int itemCount)
	{
		if (itemCount > positionMatrix.Length)
			throw new Exception("There are too few rows in comparison to the itemCount in car.");

		int a = 0;
		int b = 0;
		for (int i = 0; i < itemCount; i++)
		{
			GameObject obj = Instantiate(itemPrefab);
			obj.transform.position = positionMatrix[a, b];
			obj.transform.SetParent(parent.transform);

			a += 1;

			if (a >= positionMatrix.GetLength(0))
			{
				a = 0;
				b += 1;
			}
		}
	}
}
