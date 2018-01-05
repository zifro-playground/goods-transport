using UnityEngine;
using System.Collections.Generic;

public class DriveForward : Compiler.Function 
{
	public DriveForward()
	{
		this.name = "kör_framåt";
		this.inputParameterAmount.Add(0);
		this.hasReturnVariable = false;
		this.pauseWalker = true;
	}

	public override Compiler.Variable runFunction(Compiler.Scope currentScope, Compiler.Variable[] inputParas, int lineNumber)
	{
		Transform target = Road.Instance.ShortEndPoint;
		CarQueue.DriveQueueForward(lineNumber, target);

		return new Compiler.Variable();
	}
}
