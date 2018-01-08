using Compiler;
using UnityEngine;

public class ChargeCar : Compiler.Function
{
	public ChargeCar()
	{
		this.name = "ladda_bil";
		this.inputParameterAmount.Add(0);
		this.hasReturnVariable = false;
		this.pauseWalker = true;
	}

	public override Variable runFunction(Scope currentScope, Variable[] inputParas, int lineNumber)
	{
		GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController2_1>().CarsCharged++;
		ChargeStation.Instance.ChargeBattery();

		return new Variable();
	}
}
