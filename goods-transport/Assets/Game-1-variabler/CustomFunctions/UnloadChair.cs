using UnityEngine;
using PM;

public class UnloadChair : Compiler.Function
{
	public UnloadChair()
	{
		this.name = "lasta_av_stol";
		this.inputParameterAmount.Add(0);
		this.hasReturnVariable = false;
		this.pauseWalker = true;
	}

	public override Compiler.Variable runFunction(Compiler.Scope currentScope, Compiler.Variable[] inputParas, int lineNumber)
	{
		GameObject.FindGameObjectWithTag("Chair").GetComponent<UnloadableItem>().isUnloading = true;
		GameObject.FindGameObjectWithTag("WinController").GetComponent<WinControllerScene2>().itemsUnloaded += 1;
		return new Compiler.Variable();
	}
}
