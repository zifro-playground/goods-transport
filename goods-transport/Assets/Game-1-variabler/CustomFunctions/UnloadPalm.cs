using UnityEngine;
using PM;

public class UnloadPalm : Compiler.Function
{	
	public UnloadPalm()
	{
		this.name = "lasta_av_palm";
		this.inputParameterAmount.Add(0);
		this.hasReturnVariable = false;
		this.pauseWalker = true;
	}

	public override Compiler.Variable runFunction(Compiler.Scope currentScope, Compiler.Variable[] inputParas, int lineNumber)
	{
		GameObject palm = GameObject.FindGameObjectWithTag("Palm");

		if (palm == null)
			PMWrapper.RaiseError(lineNumber, "Hittade ingen palm att lasta av.");

		palm.GetComponent<UnloadableItem>().isUnloading = true;
		GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController1_2>().itemsUnloaded += 1;
		return new Compiler.Variable();
	}
}
