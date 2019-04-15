using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using GameData;
using UnityEngine;

public class LevelController : MonoBehaviour
{
	[Header("Prefabs")]
	public GameObject CarPrefab;
	public GameObject CarPlatformPrefab;
	public GameObject CarFrontPrefab;
	public GameObject BoxRowPrefab;
	public GameObject PalmPrefab;
	public GameObject TreePrefab;
	public GameObject LampPrefab;
	public GameObject TablePrefab;
	public GameObject ChairPrefab;

	[Header("Materials")]
	public Material Red;
	public Material Green;
	public Material Blue;

	[Header("Distances")]
	public float CarSpacing = 1.5f;
	public float BoxSpacing = 0.5f;
	public float CarPadding = 0.4f;
	private const float BoxLength = 1; // is set from CreateAssets()

	private GameObject queue;

	[HideInInspector]
	public static CaseData CaseData;

	private Dictionary<string, GameObject> itemTypeToPrefab;
	private void BuildItemDictionary()
	{
		if (itemTypeToPrefab == null)
		{
			itemTypeToPrefab = new Dictionary<string, GameObject>
			{
				{"palmer", PalmPrefab},
				{"granar", TreePrefab},
				{"bord", TablePrefab},
				{"lampor", LampPrefab},
				{"stolar", ChairPrefab}
			};

		}
	}

	private Dictionary<string, Material> colorToMaterial;
	private void BuildColorDictionary()
	{
		colorToMaterial = new Dictionary<string, Material>
		{
			{"red", Red},
			{"green", Green},
			{"blue", Blue}
		};

	}

	public void LoadCase(CaseData newCaseData)
	{
		CaseData = newCaseData;
		
		BuildItemDictionary();
		BuildColorDictionary();

		RemoveOldAssets();
		CreateAssets();
		SetPrecode();
		SetAnswer();
	}

	private void RemoveOldAssets()
	{
		CarQueue.RemoveAllCars();
		Destroy(queue);
	}

	private void CreateAssets()
	{
		queue = new GameObject("Queue");
		queue.AddComponent<CarQueue>();

		float boxLength = BoxRowPrefab.GetComponentInChildren<Renderer>().bounds.size.x;
		float previousCarPositionX = 0;

		foreach (CarData carData in CaseData.cars)
		{
			GameObject carObj = Instantiate(CarPrefab);
			CarInfo carInfo = carObj.GetComponent<CarInfo>();
            carInfo.StartBatteryLevel = carData.batteryLevel;
			carInfo.BatteryLevel = carData.batteryLevel;
			carInfo.ItemsInCar = 0;

			carObj.transform.SetParent(queue.transform);

			GameObject platform = Instantiate(CarPlatformPrefab);
			GameObject front = Instantiate(CarFrontPrefab);

			SetCarMaterial(carData, front);

			platform.transform.SetParent(carObj.transform);
			front.transform.SetParent(carObj.transform);

			RescaleCar(carData, platform, front);

			// Position car in queue
			Vector3 tempPos = carObj.transform.position;
			tempPos.x = previousCarPositionX;
			carObj.transform.position = tempPos;

			// Place the rows of boxes and their items in car
			Bounds platformBounds = platform.GetComponent<Renderer>().bounds;
			Bounds frontBounds = front.GetComponent<Renderer>().bounds;
			front.transform.position = new Vector3(platformBounds.max.x, platformBounds.min.y-0.25f, platformBounds.center.z);

			float carLength = platformBounds.size.x + frontBounds.size.x;
			float carWidthCenter = platformBounds.center.z;
			float carLeftEnd = platformBounds.min.x;
			float sectionLeftEnd = carLeftEnd;

			foreach (SectionData section in carData.sections)
			{
				var itemPositions = new Vector3[section.rows, 4];

				for (int j = 1; j <= section.rows; j++)
				{
					GameObject boxRow = Instantiate(BoxRowPrefab);
					boxRow.transform.SetParent(carObj.transform);

					float rowCenter = sectionLeftEnd + CarPadding + ((2 * (float)j - 1) / 2) * boxLength + (j - 1) * BoxSpacing;
					boxRow.transform.position = new Vector3(rowCenter, 0.5f, carWidthCenter);
					
					float rowTopEnd = boxRow.GetComponentInChildren<Renderer>().bounds.max.y;

					for (int k = 1; k <= 4; k++)
					{
						float colCenter = platformBounds.min.z + CarPadding + ((2 * (float)k - 1) / 2) * boxLength + (k - 1) * BoxSpacing;
						itemPositions[j-1, k-1] = new Vector3(rowCenter, rowTopEnd, colCenter);
					}

				}
				PlaceItems(itemPositions, carObj, itemTypeToPrefab[section.type], section.itemCount);
				carInfo.ItemsInCar += section.itemCount;
				carInfo.CargoType = section.type;

				float sectionLength = section.rows * boxLength + (section.rows - 1) * BoxSpacing  + 2 * CarPadding;
				sectionLeftEnd += sectionLength;
			}
			previousCarPositionX = carObj.transform.position.x - carLength - CarSpacing;
		}
	}
	private void SetAnswer()
	{
		PMWrapper.SetCaseAnswer(CaseData.answer);
	}
	private void SetPrecode()
	{
		ISceneController[] sceneControllers = PM.UISingleton.FindInterfaces<ISceneController>();
		if (sceneControllers.Length > 1)
			throw new Exception("There are more than one class that implements ISceneController in current scene.");
		if (sceneControllers.Length < 1)
			throw new Exception("Could not find any class that implements ISceneController.");

		sceneControllers[0].SetPrecode(CaseData);
	}

	private void RescaleCar(CarData carData, GameObject carPlatform, GameObject carFront)
	{
		Vector3 platformSize = carPlatform.GetComponent<Renderer>().bounds.size;

		int sectionCount = 0;
		int rowsInCar = 0;
		int boxSpacingsNeeded = 0;

		foreach (SectionData section in carData.sections)
		{
			rowsInCar += section.rows;
			if (section.itemCount >= 0)
			{
				sectionCount++;
				boxSpacingsNeeded += section.rows - 1;
			}
            else
            {
                throw new Exception("The itemCount in a section can not be negative (in level: " + PMWrapper.currentLevel + ")");
            }
		}
		float newCarWidth = 4 * BoxLength + 3 * BoxSpacing + 2 * CarPadding;
		float newPlatformLength = rowsInCar * BoxLength + boxSpacingsNeeded * BoxSpacing + 2 * sectionCount * CarPadding;
		
		float scaleFactorLengthPlatform = newPlatformLength / platformSize.x;
		float scaleFactorWidthPlatform = newCarWidth / platformSize.z;

		carPlatform.transform.localScale = new Vector3(scaleFactorLengthPlatform, 2.3f, scaleFactorWidthPlatform);
	}
	private void PlaceItems(Vector3[,] positionMatrix, GameObject parent, GameObject itemPrefab, int itemCount)
	{
		if (itemCount > positionMatrix.Length)
			throw new Exception("There are too few rows in comparison to the itemCount in car.");

		int a = 0;
		int b = positionMatrix.GetLength(1) - 1;
		for (int i = 0; i < itemCount; i++)
		{
			GameObject obj = Instantiate(itemPrefab);
			obj.transform.position = positionMatrix[a, b];
			obj.transform.SetParent(parent.transform);

			a += 1;

			if (a >= positionMatrix.GetLength(0))
			{
				a = 0;
				b -= 1;
			}
		}
	}

	private void SetCarMaterial(CarData carData, params GameObject[] objects)
	{
		Material material;

		if (carData.color == null)
		{
			material = GetRandomMaterial();
		}
		else if (!colorToMaterial.ContainsKey(carData.color))
		{
			Debug.Log("Warning! The color \"" + carData.color + "\" specified in json is not supported.");
			material = GetRandomMaterial();
		}
		else
		{

			material = colorToMaterial[carData.color];
		}

		foreach (GameObject obj in objects)
		{
			Renderer rend = obj.GetComponent<Renderer>();
			rend.material = material;
		}
	}
	private Material GetRandomMaterial()
	{
		int randomIndex = UnityEngine.Random.Range(0, colorToMaterial.Keys.Count);
		string color = colorToMaterial.Keys.ElementAt(randomIndex);
		return colorToMaterial[color];
	}
}
