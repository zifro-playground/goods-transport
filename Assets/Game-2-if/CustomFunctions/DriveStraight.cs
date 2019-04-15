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
		SortedQueue.ForwardQueue.Add(CarQueue.GetFirstCar());

		CarQueue.DriveQueueForward();
		CarQueue.DriveFirstCarStraight();

		return new Variable();
	}
}
