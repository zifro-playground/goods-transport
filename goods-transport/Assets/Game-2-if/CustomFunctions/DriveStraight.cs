using Compiler;
using UnityEngine;

public class DriveStraight : Function
{
	public DriveStraight()
	{
		name = "kör_rakt";
		inputParameterAmount.Add(0);
		hasReturnVariable = false;
		pauseWalker = true;
	}

	public override Variable runFunction(Scope currentScope, Variable[] inputParas, int lineNumber)
	{
		GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController2_2>().forwardQueue.Add(CarQueue.GetFirstCar());

		Transform target = GameObject.FindGameObjectWithTag("Road").GetComponent<Road>().MiddelEndPoint;
		CarQueue.DriveQueueForward(lineNumber, target);

		return new Variable();
	}
}
