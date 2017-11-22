using System.Collections.Generic;
using UnityEngine;
using PM;

public class EmptyCar : Compiler.Function
{
	public EmptyCar()
	{
		this.name = "töm_bilen";
		this.inputParameterAmount.Add(0);
		this.hasReturnVariable = false;
		this.pauseWalker = true;
	}

	public override Compiler.Variable runFunction(Compiler.Scope currentScope, Compiler.Variable[] inputParas, int lineNumber)
	{
		List<GameObject> itemsToUnload = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().itemsToUnload;
		foreach (GameObject item in itemsToUnload)
		{
			item.GetComponent<UnloadableItem>().isUnloading = true;
		}
		return new Compiler.Variable();
	}
}
