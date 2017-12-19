using Compiler;
using UnityEngine;

public class DriveRight : Function
{
	public DriveRight()
	{
		name = "kör_höger";
		inputParameterAmount.Add(0);
		hasReturnVariable = false;
		pauseWalker = false;
	}

	public override Variable runFunction(Scope currentScope, Variable[] inputParas, int lineNumber)
	{
		GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController2_2>().rightQueue.Add(CarQueue.GetFirstCar());
		CarQueue.DriveQueueForward(lineNumber);

		return new Variable();
	}
}
