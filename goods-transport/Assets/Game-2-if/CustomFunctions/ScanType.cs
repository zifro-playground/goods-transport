using Compiler;
using UnityEngine;

public class ScanType : Function {

	public ScanType()
	{
		name = "scanna_sort";
		inputParameterAmount.Add(0);
		hasReturnVariable = true;
		pauseWalker = false;
	}

	public override Variable runFunction(Scope currentScope, Variable[] inputParas, int lineNumber)
	{
		string type = "palmer";

		GameObject firstCar = LevelController.GetFirstCar();


		return new Variable("type", type);
	}
}
