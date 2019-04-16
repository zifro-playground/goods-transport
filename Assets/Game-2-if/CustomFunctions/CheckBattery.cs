using Mellis;
using Mellis.Core.Interfaces;

public class CheckBattery : ClrFunction
{
	public CheckBattery() : base("kolla_batterinivå")
	{
	}

	public override IScriptType Invoke(params IScriptType[] arguments)
	{
		int batteryLevel = CarQueue.GetFirstCar().GetComponent<CarInfo>().batteryLevel;

		ChargeStation.instance.CheckBattery(batteryLevel);

		return Processor.Factory.Create(batteryLevel);
	}
}
