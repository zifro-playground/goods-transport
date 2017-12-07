using System;
using Compiler;
using UnityEngine;

public class ScanPalms : Compiler.Function
{
	public ScanPalms()
	{
		this.name = "scanna_antal_palmer";
		this.inputParameterAmount.Add(0);
		this.pauseWalker = true;
		this.hasReturnVariable = false;
	}

	public override Variable runFunction(Scope currentScope, Variable[] inputParas, int lineNumber)
	{
		LevelController controller = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();

		if (controller.activeCars.Count == 0)
			PMWrapper.RaiseError(lineNumber, "Kan inte hitta något att scanna.");

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

		int palmCount = 0;

		foreach (Transform child in car.transform)
		{
			if (child.CompareTag("Palm"))
				palmCount++;
		}

		scanner.SetDisplayText(palmCount.ToString());

		return new Variable("palmCount", palmCount);
	}
}
