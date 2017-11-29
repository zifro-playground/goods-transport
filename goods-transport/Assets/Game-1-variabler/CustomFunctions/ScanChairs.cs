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

		GameObject carToScan = controller.activeCars.First.Value;
		GameObject.FindGameObjectWithTag("Scanner").GetComponent<Scanner>().Scan(carToScan);

		int chairCount = 0;

		return new Variable("chaircount", chairCount);
	}
}
