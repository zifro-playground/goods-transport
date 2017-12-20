using Compiler;
using UnityEngine;

public class DriveLeft : Function
{
	public DriveLeft()
	{
		name = "kör_vänster";
		inputParameterAmount.Add(0);
		hasReturnVariable = false;
		pauseWalker = false;
	}

	public override Variable runFunction(Scope currentScope, Variable[] inputParas, int lineNumber)
	{
		GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController2_2>().leftQueue.Add(CarQueue.GetFirstCar());
		CarQueue.DriveQueueForward(lineNumber);

		return new Variable();
	}
}
