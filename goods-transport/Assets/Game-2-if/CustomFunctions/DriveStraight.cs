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
		// ugly solution for scene controllers
		var sceneController = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController2_2>();
		
		if (sceneController == null)
			GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController2_3>().ForwardQueue.Add(CarQueue.GetFirstCar());
		else
			sceneController.ForwardQueue.Add(CarQueue.GetFirstCar());

		Transform target = Road.Instance.MiddelEndPoint;
		CarQueue.DriveQueueForward(lineNumber, target);

		return new Variable();
	}
}
