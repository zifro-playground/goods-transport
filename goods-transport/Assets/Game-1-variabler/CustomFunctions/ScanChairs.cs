﻿using Compiler;
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
		LevelController controller = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();

		GameObject carToScan = controller.activeCars.First.Value;
		Scanner scanner = GameObject.FindGameObjectWithTag("Scanner").GetComponent<Scanner>();
		scanner.Scan(carToScan);

		int chairCount = 0;

		foreach (Transform child in carToScan.transform)
		{
			if (child.CompareTag("Chair"))
				chairCount++;
		}

		scanner.SetDisplayText(chairCount.ToString());

		return new Variable("chaircount", chairCount);
	}
}