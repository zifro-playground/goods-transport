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

	[Header("Options")]
	public float boxSpacing;

	private Case caseData;

	public void LoadLevel(Case caseData)
	{
		this.caseData = caseData;

		// TODO Delete last level
		// TODO Change precode
		// TODO Set current level answer
		// TODO create new level assets

		SetLevelAnswere();
		CreateAssets();
	}

	private void CreateAssets()
	{
		GameObject sectionObj = null;
		GameObject gridObj;

		foreach (Car car in caseData.cars)
		{
			foreach (Item item in car.items)
			{

				if (item.amount > 0)
				{
					sectionObj = Instantiate(section);

					if (item.type == "palm" || item.type == "tree")
					{
						gridObj = Instantiate(grid3x3);
					}
					else
					{
						gridObj = Instantiate(grid4x4);
					}
					gridObj.transform.SetParent(sectionObj.transform);
				}
				
			}
		}
		if(sectionObj != null)
			CalculatePositionMatrix(sectionObj, 3, 3);
	}

	private void SetLevelAnswere()
	{
		int itemsInCar = 0;

		foreach (Item item in caseData.cars[0].items)
		{
			itemsInCar += item.amount;
		}

		PMWrapper.SetCurrentLevelAnswere(Compiler.VariableTypes.number, new string[] { itemsInCar.ToString() });
	}

	private void CalculatePositionMatrix(GameObject gameObject, int n, int m)
	{
		Bounds bounds = gameObject.GetComponent<MeshFilter>().mesh.bounds;

		float xMin = bounds.min.x;
		float xMax = bounds.max.x;
		float yMin = bounds.min.y;
		float yMax = bounds.max.y;

		float boxSizeX = (bounds.size.x - boxSpacing * (m - 1)) / m;
		float boxSizeY = (bounds.size.y - boxSpacing * (n - 1)) / n;

		Vector2[,] positionMatrix = new Vector2[n, m];

		for (int i = 0; i < n; i++)
		{
			for (int j = 0; j < m; j++)
			{
				float xPos = xMin + boxSizeX/2 + i*(boxSizeX + boxSpacing);
				float yPos = yMax - boxSizeY/2 - i*(boxSizeY + boxSpacing);
				positionMatrix[i, j] = new Vector2(xPos, yPos);
				print("i: " + i + " j: " + j + " -------------------");
				print(positionMatrix[i, j]);
			}
		}
	}
}
