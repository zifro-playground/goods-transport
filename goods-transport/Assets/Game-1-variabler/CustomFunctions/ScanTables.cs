using System;
using Compiler;
using UnityEngine;

public class ScanTables : Compiler.Function
{
	public ScanTables()
	{
		this.name = "scanna_antal_bord";
		this.inputParameterAmount.Add(0);
		this.pauseWalker = true;
		this.hasReturnVariable = false;
	}

	public override Variable runFunction(Scope currentScope, Variable[] inputParas, int lineNumber)
	{
		GameObject car = CarQueue.GetFirstCar();

		Scanner scanner = Scanner.Instance;
		scanner.Scan(car);

		int tableCount = 0;

		foreach (Transform child in car.transform)
		{
			if (child.CompareTag("Table"))
				tableCount++;
		}

		scanner.SetDisplayText(tableCount);

		return new Variable("tablecount", tableCount);
	}
}
