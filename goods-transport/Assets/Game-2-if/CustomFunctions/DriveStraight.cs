using Compiler;
using UnityEngine;

public class DriveStraight : Function
{
	public DriveStraight()
	{
		name = "kör_rakt";
		inputParameterAmount.Add(0);
		hasReturnVariable = false;
		pauseWalker = false;
	}

	public override Variable runFunction(Scope currentScope, Variable[] inputParas, int lineNumber)
	{
		Debug.Log("Åker rakt");
		return new Variable();
	}
}
