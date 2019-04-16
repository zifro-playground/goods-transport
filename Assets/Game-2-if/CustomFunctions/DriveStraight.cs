using Mellis;
using Mellis.Core.Interfaces;

public class DriveStraight : ClrYieldingFunction
{
	public DriveStraight() : base("kör_rakt")
	{
	}

	public override void InvokeEnter(params IScriptType[] arguments)
	{
		SortedQueue.LEFT_QUEUE.Add(CarQueue.GetFirstCar());

		CarQueue.DriveQueueForward();
		CarQueue.DriveFirstCarStraight();
		SceneController2_2.carsSorted++;
	}
}
