using GameData;
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
        ChargeStation.instance.ChargeBattery();
    }

    private static void SetCarCharged()
    {
        var caseDef = (GoodsCaseDefinition) PMWrapper.currentLevel.cases[PMWrapper.currentCase].caseDefinition;
        int chargeBound = caseDef.chargeBound;

		CarInfo carInfo = CarQueue.GetFirstCar().GetComponent<CarInfo>();

        if (carInfo.hasBeenCharged)
		{
			return;
		}

		if (carInfo.startBatteryLevel < chargeBound)
        {
            SceneController2_1.correctlyCharged++;
        }
        else
        {
            if (carInfo.startBatteryLevel != 100)
			{
				SceneController2_1.falselyCharged++;
			}
		}

        carInfo.hasBeenCharged = true;
    }
}
