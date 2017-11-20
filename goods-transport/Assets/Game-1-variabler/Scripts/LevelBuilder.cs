using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject section;
	public GameObject grid3x3;
	public GameObject grid4x4;
	public GameObject palm;
	public GameObject tree;
	public GameObject lamp;
	public GameObject table;

	private Case caseData;
	private List<GameObject> activeObjects = new List<GameObject>();

	public void LoadLevel(Case caseData)
	{
		this.caseData = caseData;

		// TODO Change precode

		RemoveOldAssets();
		CreateAssets();
		SetPrecode();
		SetLevelAnswere();
	}

	private void RemoveOldAssets()
	{
		foreach (GameObject obj in activeObjects)
		{
			Destroy(obj);
		}
		activeObjects.Clear();
	}

	private void CreateAssets()
	{
		GameObject sectionObj;
		GameObject gridObj = null;

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
					}
					else
					{
						gridObj = Instantiate(grid4x4);
					}
					gridObj.transform.SetParent(sectionObj.transform);
					activeObjects.Add(gridObj);
				}
				
			}
		}
		if(gridObj != null)
			CalculatePositionMatrix(gridObj, 3, 3);
	}

	private void SetLevelAnswere()
	{
		int itemsInCar = 0;

		foreach (Item item in caseData.cars[0].items)
		{
			itemsInCar += item.count;
		}

		PMWrapper.SetCurrentLevelAnswere(Compiler.VariableTypes.number, new string[] { itemsInCar.ToString() });
	}

	private void SetPrecode()
	{
		string precode = "";

		foreach (Item item in caseData.cars[0].items)
		{
			if (item.count > 0)
				precode += item.type + " = " + item.count + "\n";
		}
		PMWrapper.preCode = precode.Trim();
	}

	private void CalculatePositionMatrix(GameObject gameObject, int n, int m)
	{
		Bounds bounds = gameObject.GetComponent<MeshRenderer>().bounds;

		float xMin = bounds.min.x;
		float xMax = bounds.max.x;
		float yMin = bounds.min.y;
		float yMax = bounds.max.y;

		// Should be substituted by something more general like a ratio between boxSize and boxSpacing
		float boxSpacing = 0.27f;

		float boxSizeX = (bounds.size.x - boxSpacing * (m - 1)) / m;
		float boxSizeY = (bounds.size.y - boxSpacing * (n - 1)) / n;

		Vector2[,] positionMatrix = new Vector2[n, m];

		for (int i = 0; i < n; i++)
		{
			for (int j = 0; j < m; j++)
			{
				//print(xMin + boxSizeX / 2 + i * (boxSizeX + boxSpacing));
				float xPos = xMin + boxSizeX/2 + i*(boxSizeX + boxSpacing);
				float yPos = yMax - boxSizeY/2 - j*(boxSizeY + boxSpacing);
				positionMatrix[i, j] = new Vector2(xPos, yPos);
				GameObject obj = Instantiate(palm, new Vector3(positionMatrix[i, j].x, positionMatrix[i, j].y, 0), Quaternion.identity);
				activeObjects.Add(obj);
			}
		}
	}
}
