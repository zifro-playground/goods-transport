using Compiler;
using UnityEngine;

public class ChargeCar : Compiler.Function
{
	public ChargeCar()
	{
		this.name = "ladda_tåg";
		this.inputParameterAmount.Add(0);
		this.hasReturnVariable = false;
		this.pauseWalker = true;
	}

	public override Variable runFunction(Scope currentScope, Variable[] inputParas, int lineNumber)
	{
        SetCarCharged();
		ChargeStation.Instance.ChargeBattery();

		return new Variable();
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
