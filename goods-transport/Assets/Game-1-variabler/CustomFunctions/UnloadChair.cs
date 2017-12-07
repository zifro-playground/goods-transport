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
		GameObject chair = GameObject.FindGameObjectWithTag("Chair");

		if (chair == null)
			PMWrapper.RaiseError(lineNumber, "Hittade ingen stol att lasta av.");

		chair.GetComponent<UnloadableItem>().isUnloading = true;
		GameObject.FindGameObjectWithTag("SceneController").GetComponent<Scene2Controller>().itemsUnloaded += 1;
		return new Compiler.Variable();
	}
}
