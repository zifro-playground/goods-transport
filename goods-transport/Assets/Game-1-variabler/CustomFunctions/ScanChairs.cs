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
		LevelController controller = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();

		GameObject car = controller.activeCars.First.Value;
		GameObject carPlatform = null;

		// Find the car platform to focus on when scanning
		foreach (Transform child in car.transform)
		{
			if (child.gameObject.CompareTag("CarPlatform"))
				carPlatform = child.gameObject;
		}
		if (carPlatform == null)
			throw new Exception("Could not find any child of first car in queue with tag \"CarPlatform\".");

		Scanner scanner = GameObject.FindGameObjectWithTag("Scanner").GetComponent<Scanner>();
		scanner.Scan(carPlatform);

		int chairCount = 0;

		foreach (Transform child in car.transform)
		{
			if (child.CompareTag("Chair"))
				chairCount++;
		}

		scanner.SetDisplayText(chairCount.ToString());

		return new Variable("chaircount", chairCount);
	}
}
