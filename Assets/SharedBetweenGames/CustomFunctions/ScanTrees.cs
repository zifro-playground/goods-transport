using Mellis;
using Mellis.Core.Interfaces;
using UnityEngine;

public class ScanTrees : ClrYieldingFunction
{
	public ScanTrees() : base("scanna_antal_granar")
	{
	}

	public override void InvokeEnter(params IScriptType[] arguments)
	{
		GameObject car = CarQueue.GetFirstCar();

		if (car == null)
		{
			PMWrapper.RaiseError("Kan inte hitta något att scanna.");
		}

		Scanner scanner = Scanner.instance;
		scanner.Scan(car);

		int treeCount = 0;

		foreach (Transform child in car.transform)
		{
			if (child.CompareTag("Tree"))
			{
				treeCount++;
			}
		}

		scanner.SetDisplayText(treeCount);
	}
}
