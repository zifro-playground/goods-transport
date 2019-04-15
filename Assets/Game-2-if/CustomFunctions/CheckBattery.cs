using Mellis;
using Mellis.Core.Interfaces;
using UnityEngine;

public class CheckBattery : ClrFunction
{
    public CheckBattery() : base("kolla_batterinivå")
    {
    }

    public override IScriptType Invoke(params IScriptType[] arguments)
    {
        int batteryLevel = CarQueue.GetFirstCar().GetComponent<CarInfo>().BatteryLevel;

        ChargeStation.Instance.CheckBattery(batteryLevel);

        return Processor.Factory.Create(batteryLevel);
    }
}