using System;
using Compiler;
using UnityEngine;

public class ScanLamps : Function
{
	public ScanLamps()
	{
		name = "scanna_antal_lampor";
		inputParameterAmount.Add(0);
		pauseWalker = true;
		hasReturnVariable = false;
	}

	public override Variable runFunction(Scope currentScope, Variable[] inputParas, int lineNumber)
	{
		GameObject car = CarQueue.GetFirstCar();

		if (car == null)
			PMWrapper.RaiseError(lineNumber, "Kan inte hitta något att scanna.");

		GameObject carPlatform = null;

		// Find the car platform to focus on when scanning
		foreach (Transform child in car.transform)
		{
			if (child.gameObject.CompareTag("CarPlatform"))
				carPlatform = child.gameObject;
		}
		if (carPlatform == null)
			throw new Exception("Could not find any child of first car in queue with tag \"CarPlatform\".");

		Scanner scanner = Scanner.Instance;
		scanner.Scan(carPlatform);

		int lampCount = 0;

		foreach (Transform child in car.transform)
		{
			if (child.CompareTag("Lamp"))
				lampCount++;
		}

		scanner.SetDisplayText(lampCount);

		return new Variable("lampCount", lampCount);
	}
}
