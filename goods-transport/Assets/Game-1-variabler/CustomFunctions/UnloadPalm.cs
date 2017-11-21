using UnityEngine;
using PM;

public class UnloadPalm : Compiler.Function
{	
	public UnloadPalm()
	{
		this.name = "lasta_av_palm";
		this.inputParameterAmount.Add(0);
		this.hasReturnVariable = false;
		this.pauseWalker = false;
	}

	public override Compiler.Variable runFunction(Compiler.Scope currentScope, Compiler.Variable[] inputParas, int lineNumber)
	{
		Debug.Log("Lastar av palm");
		return new Compiler.Variable();
	}
}
