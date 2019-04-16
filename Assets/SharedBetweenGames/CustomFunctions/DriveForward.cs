using Mellis;
using Mellis.Core.Interfaces;

public class DriveForward : ClrYieldingFunction
{
	public DriveForward() : base("kör_framåt")
	{
	}

	public override void InvokeEnter(params IScriptType[] arguments)
	{
		CarQueue.DriveQueueForward();
		CarQueue.DriveFirstCarShort();
	}
}
