using System;
using Compiler;
using UnityEngine;

public class ScanChairs : Compiler.Function 
{
	public ScanChairs()
	{
		this.name = "scanna_antal_stolar";
		this.inputParameterAmount.Add(0);
		this.pauseWalker = true;
		this.hasReturnVariable = false;
	}

	public override Variable runFunction(Scope currentScope, Variable[] inputParas, int lineNumber)
	{
		GameObject car = CarQueue.GetFirstCar();

		Scanner scanner = GameObject.FindGameObjectWithTag("Scanner").GetComponent<Scanner>();
		scanner.Scan(car);

		int chairCount = 0;

		foreach (Transform child in car.transform)
		{
			if (child.CompareTag("Chair"))
				chairCount++;
		}

		scanner.SetDisplayText(chairCount);

		return new Variable("chaircount", chairCount);
	}
}
