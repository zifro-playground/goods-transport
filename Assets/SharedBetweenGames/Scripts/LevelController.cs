using System;
using System.Collections.Generic;
using System.Linq;
using GameData;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour, IPMCaseSwitched
{
	const float BOX_LENGTH = 1; // is set from CreateAssets()

	[FormerlySerializedAs("Blue")]
	public Material blue;

	[FormerlySerializedAs("BoxRowPrefab")]
	public GameObject boxRowPrefab;

	[FormerlySerializedAs("BoxSpacing")]
	public float boxSpacing = 0.5f;

	[FormerlySerializedAs("CarFrontPrefab")]
	public GameObject carFrontPrefab;

	[FormerlySerializedAs("CarPadding")]
	public float carPadding = 0.4f;

	[FormerlySerializedAs("CarPlatformPrefab")]
	public GameObject carPlatformPrefab;

	[Header("Prefabs")]
	[FormerlySerializedAs("CarPrefab")]
	public GameObject carPrefab;

	[Header("Distances")]
	[FormerlySerializedAs("CarSpacing")]
	public float carSpacing = 1.5f;

	[FormerlySerializedAs("ChairPrefab")]
	public GameObject chairPrefab;

	[FormerlySerializedAs("Green")]
	public Material green;

	[FormerlySerializedAs("LampPrefab")]
	public GameObject lampPrefab;

	[FormerlySerializedAs("PalmPrefab")]
	public GameObject palmPrefab;

	[Header("Materials")]
	[FormerlySerializedAs("Red")]
	public Material red;

	[FormerlySerializedAs("TablePrefab")]
	public GameObject tablePrefab;

	[FormerlySerializedAs("TreePrefab")]
	public GameObject treePrefab;

	Dictionary<string, Material> colorToMaterial;

	Dictionary<string, GameObject> itemTypeToPrefab;

	GameObject queue;

	public void OnPMCaseSwitched(int caseNumber)
	{
		BuildItemDictionary();
		BuildColorDictionary();

		RemoveOldAssets();

		var goodsCaseDefinition = (GoodsCaseDefinition)PMWrapper.currentLevel.cases[caseNumber].caseDefinition;

		CreateAssets(goodsCaseDefinition);

		PMWrapper.SetCaseAnswer(goodsCaseDefinition.answer);
	}

	void BuildItemDictionary()
	{
		if (itemTypeToPrefab == null)
		{
			itemTypeToPrefab = new Dictionary<string, GameObject> {
				{"palmer", palmPrefab},
				{"granar", treePrefab},
				{"bord", tablePrefab},
				{"lampor", lampPrefab},
				{"stolar", chairPrefab}
			};
		}
	}

	void BuildColorDictionary()
	{
		colorToMaterial = new Dictionary<string, Material> {
			{"red", red},
			{"green", green},
			{"blue", blue}
		};
	}

	void RemoveOldAssets()
	{
		CarQueue.RemoveAllCars();
		Destroy(queue);
	}

	void CreateAssets(GoodsCaseDefinition caseDef)
	{
		queue = new GameObject("Queue");
		queue.AddComponent<CarQueue>();

		float boxLength = boxRowPrefab.GetComponentInChildren<Renderer>().bounds.size.x;
		float previousCarPositionX = 0;

		foreach (CarData carData in caseDef.cars)
		{
			GameObject carObj = Instantiate(carPrefab, queue.transform, true);
			CarInfo carInfo = carObj.GetComponent<CarInfo>();
			carInfo.startBatteryLevel = carData.batteryLevel;
			carInfo.batteryLevel = carData.batteryLevel;
			carInfo.itemsInCar = 0;

			GameObject platform = Instantiate(carPlatformPrefab, carObj.transform, true);
			GameObject front = Instantiate(carFrontPrefab, carObj.transform, true);

			SetCarMaterial(carData, front);

			RescaleCar(carData, platform, front);

			// Position car in queue
			Vector3 tempPos = carObj.transform.position;
			tempPos.x = previousCarPositionX;
			carObj.transform.position = tempPos;

			// Place the rows of boxes and their items in car
			Bounds platformBounds = platform.GetComponent<Renderer>().bounds;
			Bounds frontBounds = front.GetComponent<Renderer>().bounds;
			front.transform.position =
				new Vector3(platformBounds.max.x, platformBounds.min.y - 0.25f, platformBounds.center.z);

			float carLength = platformBounds.size.x + frontBounds.size.x;
			float carWidthCenter = platformBounds.center.z;
			float carLeftEnd = platformBounds.min.x;
			float sectionLeftEnd = carLeftEnd;

			foreach (SectionData section in carData.sections)
			{
				var itemPositions = new Vector3[section.rows, 4];

				for (int j = 1; j <= section.rows; j++)
				{
					GameObject boxRow = Instantiate(boxRowPrefab, carObj.transform, true);

					float rowCenter = sectionLeftEnd + carPadding + (2 * (float)j - 1) / 2 * boxLength +
					                  (j - 1) * boxSpacing;
					boxRow.transform.position = new Vector3(rowCenter, 0.5f, carWidthCenter);

					float rowTopEnd = boxRow.GetComponentInChildren<Renderer>().bounds.max.y;

					for (int k = 1; k <= 4; k++)
					{
						float colCenter = platformBounds.min.z + carPadding + (2 * (float)k - 1) / 2 * boxLength +
						                  (k - 1) * boxSpacing;
						itemPositions[j - 1, k - 1] = new Vector3(rowCenter, rowTopEnd, colCenter);
					}
				}

				PlaceItems(itemPositions, carObj, itemTypeToPrefab[section.type], section.itemCount);
				carInfo.itemsInCar += section.itemCount;
				carInfo.cargoType = section.type;

				float sectionLength = section.rows * boxLength + (section.rows - 1) * boxSpacing + 2 * carPadding;
				sectionLeftEnd += sectionLength;
			}

			previousCarPositionX = carObj.transform.position.x - carLength - carSpacing;
		}
	}

	void RescaleCar(CarData carData, GameObject carPlatform, GameObject carFront)
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
				throw new Exception("The itemCount in a section can not be negative (in level: " +
				                    PMWrapper.currentLevel + ")");
			}
		}

		float newCarWidth = 4 * BOX_LENGTH + 3 * boxSpacing + 2 * carPadding;
		float newPlatformLength =
			rowsInCar * BOX_LENGTH + boxSpacingsNeeded * boxSpacing + 2 * sectionCount * carPadding;

		float scaleFactorLengthPlatform = newPlatformLength / platformSize.x;
		float scaleFactorWidthPlatform = newCarWidth / platformSize.z;

		carPlatform.transform.localScale = new Vector3(scaleFactorLengthPlatform, 2.3f, scaleFactorWidthPlatform);
	}

	void PlaceItems(Vector3[,] positionMatrix, GameObject parent, GameObject itemPrefab, int itemCount)
	{
		if (itemCount > positionMatrix.Length)
		{
			throw new Exception("There are too few rows in comparison to the itemCount in car.");
		}

		int a = 0;
		int b = positionMatrix.GetLength(1) - 1;
		for (int i = 0; i < itemCount; i++)
		{
			GameObject obj = Instantiate(itemPrefab, parent.transform, true);
			obj.transform.position = positionMatrix[a, b];

			a += 1;

			if (a >= positionMatrix.GetLength(0))
			{
				a = 0;
				b -= 1;
			}
		}
	}

	void SetCarMaterial(CarData carData, params GameObject[] objects)
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

	Material GetRandomMaterial()
	{
		int randomIndex = Random.Range(0, colorToMaterial.Keys.Count);
		string color = colorToMaterial.Keys.ElementAt(randomIndex);
		return colorToMaterial[color];
	}
}
