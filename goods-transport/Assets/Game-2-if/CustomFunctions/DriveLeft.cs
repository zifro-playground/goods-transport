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
		GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController2_2>().LeftQueue.Add(CarQueue.GetFirstCar());

		Transform target = Road.Instance.LeftEndPoint;
		CarQueue.DriveQueueForward(lineNumber, target);
		
		return new Variable();
	}
}
