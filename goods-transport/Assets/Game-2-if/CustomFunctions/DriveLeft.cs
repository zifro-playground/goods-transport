using Compiler;
using UnityEngine;

public class DriveLeft : Function
{
	public DriveLeft()
	{
		name = "kör_vänster";
		inputParameterAmount.Add(0);
		hasReturnVariable = false;
		pauseWalker = true;
	}

	public override Variable runFunction(Scope currentScope, Variable[] inputParas, int lineNumber)
	{
		GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController2_2>().leftQueue.Add(CarQueue.GetFirstCar());

		Transform target = GameObject.FindGameObjectWithTag("Road").GetComponent<Road>().LeftEndPoint;
		CarQueue.DriveQueueForward(lineNumber, target);
		
		return new Variable();
	}
}
