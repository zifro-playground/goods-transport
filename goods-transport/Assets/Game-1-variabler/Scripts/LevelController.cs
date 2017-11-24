using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject carPrefab;
	public GameObject boxRowPrefab;
	public GameObject grid3x3;
	public GameObject grid4x4;
	public GameObject palmPrefab;
	public GameObject treePrefab;
	public GameObject lampPrefab;
	public GameObject tablePrefab;

	[Space]
	public Case caseData;
	public List<GameObject> itemsToUnload = new List<GameObject>();

	private List<GameObject> activeObjects = new List<GameObject>();

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
		SetPrecode();
		SetLevelAnswer();
	}

	private void RemoveOldAssets()
	{
		foreach (GameObject obj in activeObjects)
		{
			Destroy(obj);
		}
		activeObjects.Clear();
		itemsToUnload.Clear();
	}

	private void CreateAssets()
	{
		foreach (Car car in caseData.cars)
		{
			GameObject carObj = Instantiate(carPrefab);
			Mesh carMesh = carObj.GetComponent<MeshFilter>().mesh;
			Vector3[] carVertices = carMesh.vertices;
			Vector3[] newVertices = new Vector3[carVertices.Length];

			int i = 0;
			foreach (Vector3 vertex in carVertices)
			{
				Vector3 v = vertex;
				v.x = v.x * 2;
				v.y = v.y * 2;
				v.z = v.z * 2;
				newVertices[i] = v;
				i++;
			}

			carMesh.vertices = newVertices;
			carMesh.RecalculateNormals();
			carMesh.RecalculateBounds();
		}

		/*GameObject sectionObj;
		GameObject gridObj = null;

		Vector3[,] positionMatrix3x3 = CalculatePositionMatrix(grid3x3, 3, 3);
		Vector3[,] positionMatrix4x4 = CalculatePositionMatrix(grid4x4, 4, 4);

		foreach (Car car in caseData.cars)
		{
			foreach (Item item in car.items)
			{

				if (item.count > 0)
				{
					sectionObj = Instantiate(section);
					activeObjects.Add(sectionObj);

					if (item.type == "palmer" || item.type == "granar")
					{
						gridObj = Instantiate(grid3x3);
						PlaceItems(positionMatrix3x3, itemType[item.type], item.count);
					}
					else
					{
						gridObj = Instantiate(grid4x4);
						PlaceItems(positionMatrix4x4, itemType[item.type], item.count);
					}
					gridObj.transform.SetParent(sectionObj.transform);
					activeObjects.Add(gridObj);
				}
				
			}
		}*/
	}

	private void SetPrecode()
	{
		string precode = "";

		foreach (Section section in caseData.cars[0].sections)
		{
			if (section.count > 0)
				precode += section.type + " = " + section.count + "\n";
		}
		PMWrapper.preCode = precode.Trim();
	}

	private void SetLevelAnswer()
	{
		IWinController[] winControllers = PM.UISingleton.FindInterfaces<IWinController>();
		if (winControllers.Length > 1)
			throw new Exception("There are more than one class that implements IWinController in current scene.");
		if (winControllers.Length < 1)
			throw new Exception("Could not find any class that implements IWinController.");

		winControllers[0].SetLevelAnswer(caseData);
	}

	private Vector3[,] CalculatePositionMatrix(GameObject gameObject, int n, int m)
	{
		Bounds bounds = gameObject.GetComponent<MeshRenderer>().bounds;

		float xMin = bounds.min.x;
		float xMax = bounds.max.x;
		float yMax = bounds.max.y;
		float zMin = bounds.min.z;
		float zMax = bounds.max.z;

		// Should be substituted by something more general like a ratio between boxSize and boxSpacing
		float boxSpacing = 0.27f;

		float boxSizeX = (bounds.size.x - boxSpacing * (m - 1)) / m;
		float boxSizeZ = (bounds.size.z - boxSpacing * (n - 1)) / n;

		Vector3[,] positionMatrix = new Vector3[n, m];

		for (int i = 0; i < n; i++)
		{
			for (int j = 0; j < m; j++)
			{
				float xPos = xMin + boxSizeX/2 + i*(boxSizeX + boxSpacing);
				float zPos = zMax - boxSizeZ/2 - j*(boxSizeZ + boxSpacing);
				positionMatrix[i, j] = new Vector3(xPos, yMax, zPos);
			}
		}
		return positionMatrix;
	}

	private void PlaceItems(Vector3[,] positionMatrix, GameObject itemPrefab, int itemCount)
	{
		int a = 0;
		int b = 0;
		for (int i = 0; i < itemCount; i++)
		{
			GameObject obj = Instantiate(itemPrefab, new Vector3(positionMatrix[a, b].x, positionMatrix[a, b].y, positionMatrix[a, b].z), Quaternion.identity);
			activeObjects.Add(obj);
			itemsToUnload.Add(obj);

			a += 1;

			if (a >= positionMatrix.GetLength(0))
			{
				a = 0;
				b += 1;
			}
		}
	}
}
