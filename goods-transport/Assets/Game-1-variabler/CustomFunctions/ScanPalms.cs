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

		GameObject carToScan = controller.activeCars.First.Value;
		Scanner scanner = GameObject.FindGameObjectWithTag("Scanner").GetComponent<Scanner>();
		scanner.Scan(carToScan);

		int palmCount = 0;

		foreach (Transform child in carToScan.transform)
		{
			if (child.CompareTag("Palm"))
				palmCount++;
		}

		scanner.SetDisplayText(palmCount.ToString());

		return new Variable("palmCount", palmCount);
	}
}
