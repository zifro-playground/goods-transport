using Compiler;
using UnityEngine;

public class DriveRight : Function
{
	public DriveRight()
	{
		name = "kör_höger";
		inputParameterAmount.Add(0);
		hasReturnVariable = false;
		pauseWalker = true;
	}

	public override Variable runFunction(Scope currentScope, Variable[] inputParas, int lineNumber)
	{
		GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController2_2>().rightQueue.Add(CarQueue.GetFirstCar());

		Transform target = Road.Instance.RightEndPoint;
		CarQueue.DriveQueueForward(lineNumber, target);

		return new Variable();
	}
}
