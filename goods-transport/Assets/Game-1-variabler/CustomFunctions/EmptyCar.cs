using System.Collections.Generic;
using UnityEngine;

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
		GameObject currentCar = CarQueue.GetFirstCar();
		UnloadableItem[] itemsToUnload = currentCar.GetComponentsInChildren<UnloadableItem>();

		if (itemsToUnload.Length == 0)
			PMWrapper.RaiseError(lineNumber, "Kan inte tömma en tom bil. Kom ihåg att köra fram nästa bil innan du tömmer igen.");

		foreach (UnloadableItem item in itemsToUnload)
		{
			if (item != null)
				item.isUnloading = true;
		}
		GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController1_3>().carsUnloaded += 1;

		return new Compiler.Variable();
	}
}
