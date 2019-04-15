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

		Scanner scanner = Scanner.Instance;
		scanner.Scan(car);

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
