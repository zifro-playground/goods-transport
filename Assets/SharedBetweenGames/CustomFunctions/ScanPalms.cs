using Mellis;
using Mellis.Core.Interfaces;
using UnityEngine;

public class ScanPalms : ClrYieldingFunction
{
	public ScanPalms() : base("scanna_antal_palmer")
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

		int palmCount = 0;

		foreach (Transform child in car.transform)
		{
			if (child.CompareTag("Palm"))
			{
				palmCount++;
			}
		}

		scanner.SetDisplayText(palmCount);
	}
}
