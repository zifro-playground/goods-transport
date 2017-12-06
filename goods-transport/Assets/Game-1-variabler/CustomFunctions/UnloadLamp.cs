using UnityEngine;
using PM;

public class UnloadLamp : Compiler.Function
{
	public UnloadLamp()
	{
		this.name = "lasta_av_lampa";
		this.inputParameterAmount.Add(0);
		this.hasReturnVariable = false;
		this.pauseWalker = true;
	}

	public override Compiler.Variable runFunction(Compiler.Scope currentScope, Compiler.Variable[] inputParas, int lineNumber)
	{
		GameObject.FindGameObjectWithTag("Lamp").GetComponent<UnloadableItem>().isUnloading = true;
		GameObject.FindGameObjectWithTag("SceneController").GetComponent<Scene2Controller>().itemsUnloaded += 1;
		return new Compiler.Variable();
	}
}
