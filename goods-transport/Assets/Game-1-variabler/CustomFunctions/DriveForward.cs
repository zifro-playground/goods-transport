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
		LevelController controller = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
		Queue<GameObject> carsToMove = controller.activeCars;

		foreach (GameObject carObj in carsToMove)
		{
			CarMovement car = carObj.GetComponent<CarMovement>();
			if (car != null)
			{
				float distance = carObj.GetComponent<Renderer>().bounds.size.x + controller.carSpacing;
				car.MoveForward(distance);
			}
		}
		GameObject firstCar = carsToMove.Dequeue();
		GameObject.Destroy(firstCar);

		return new Compiler.Variable();
	}
}
