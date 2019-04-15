using Mellis;
using Mellis.Core.Interfaces;
using UnityEngine;

public class ChargeCar : ClrYieldingFunction
{
    public ChargeCar() : base("ladda_tåg")
    {
    }

    public override void InvokeEnter(params IScriptType[] arguments)
    {
        SetCarCharged();
        ChargeStation.Instance.ChargeBattery();
    }

    private void SetCarCharged()
    {
        var carInfo = CarQueue.GetFirstCar().GetComponent<CarInfo>();
        int chargeBound = LevelController.CaseData.chargeBound;

        if (carInfo.HasBeenCharged)
            return;

        if (carInfo.StartBatteryLevel < chargeBound)
        {
            SceneController2_1.CorrectlyCharged++;
        }
        else
        {
            if (carInfo.StartBatteryLevel != 100)
                SceneController2_1.FalselyCharged++;
        }

        carInfo.HasBeenCharged = true;
    }
}