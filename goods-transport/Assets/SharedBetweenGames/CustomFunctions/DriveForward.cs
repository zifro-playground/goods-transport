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
		LinkedList<GameObject> carsToMove = LevelController.activeCars;

		if (carsToMove.Count == 0)
			PMWrapper.RaiseError(lineNumber, "Hittar ingen bil att köra framåt.");

		Bounds firstCarBounds = MyLibrary.CalculateBoundsInChildren(carsToMove.First.Value);
		float firstCarLength = firstCarBounds.extents.x;
		float secondCarLength = 0;

		if (carsToMove.First.Next != null)
		{
			Bounds secondCarBounds = MyLibrary.CalculateBoundsInChildren(carsToMove.First.Next.Value);
			secondCarLength = secondCarBounds.extents.x;
		}
		
		LevelController controller = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
		float distance = firstCarLength + controller.carSpacing + secondCarLength;

		LinkedListNode<GameObject> carNode = carsToMove.First;
		bool shouldDestroy = true;

		while (carNode != null)
		{
			CarMovement car = carNode.Value.GetComponent<CarMovement>();
			if (car != null)
			{
				car.MoveForward(distance, shouldDestroy);
				shouldDestroy = false;
			}
			carNode = carNode.Next;
		}
		carsToMove.RemoveFirst();

		return new Compiler.Variable();
	}
}
