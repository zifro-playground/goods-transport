using Mellis;
using Mellis.Core.Interfaces;
using UnityEngine;

public class DriveStraight : ClrYieldingFunction
{
	public DriveStraight() : base("kör_rakt")
	{
	}

	public override void InvokeEnter(params IScriptType[] arguments)
	{
		SortedQueue.LeftQueue.Add(CarQueue.GetFirstCar());

		CarQueue.DriveQueueForward();
		CarQueue.DriveFirstCarStraight();
	}
}