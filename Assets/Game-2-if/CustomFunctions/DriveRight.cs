using Mellis;
using Mellis.Core.Interfaces;
using UnityEngine;

public class DriveRight : ClrYieldingFunction
{
	public DriveRight() : base("kör_höger")
	{
	}

	public override void InvokeEnter(params IScriptType[] arguments)
	{
		SortedQueue.LeftQueue.Add(CarQueue.GetFirstCar());

		CarQueue.DriveQueueForward();
		CarQueue.DriveFirstCarRight();
	}
}