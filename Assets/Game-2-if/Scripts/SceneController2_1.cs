using GameData;
using PM;
using UnityEngine;

public class SceneController2_1 : MonoBehaviour, IPMCompilerStopped, IPMCompilerStarted, IPMCaseSwitched
{
	static SceneController2_1()
	{
		Main.RegisterFunction(new CheckBattery());
		Main.RegisterFunction(new ChargeCar());
		Main.RegisterFunction(new DriveForward());
	}

    public static int correctlyCharged;
    public static int falselyCharged;
	public static int checkChargeCounter;

    private int carsToCharge;
	private int carsToCheck;
	private GoodsCaseDefinition caseDef;


	public void OnPMCaseSwitched(int caseNumber)
	{
		caseDef = (GoodsCaseDefinition)PMWrapper.currentLevel.cases[caseNumber].caseDefinition;
	}


    public void OnPMCompilerStopped(StopStatus status)
	{
		if (status == StopStatus.Finished)
		{
            CorrectCase();
		}
    }

	public void OnPMCompilerStarted()
	{
        Scanner.instance.displayText.gameObject.SetActive(true);

        carsToCharge = 0;
        correctlyCharged = 0;
        falselyCharged = 0;
		checkChargeCounter = 0;
		carsToCheck = 0;

		foreach (CarData car in caseDef.cars)
		{
			carsToCheck++;
			if (car.batteryLevel < caseDef.chargeBound)
			{
				carsToCharge++;
			}
		}
	}

    private void CorrectCase()
    {
        int chargeBound = caseDef.chargeBound;

        if (correctlyCharged == carsToCharge)
        {
			if (falselyCharged > 0)
			{
				PMWrapper.RaiseTaskError ("För många tåg laddades. Bara tåg med batterinivå < " + chargeBound + " ska laddas.");
			}
			else if (checkChargeCounter < carsToCheck)
			{
				PMWrapper.RaiseTaskError ("Alla tåg kollades inte. Se till att köra kolla_batterinivå() för varje tåg.");
			}
			else
			{
				PMWrapper.SetCaseCompleted();
			}
		}
        else
        {
            if (correctlyCharged + falselyCharged == carsToCharge)
			{
				PMWrapper.RaiseTaskError("Fel tåg laddades.");
			}
			else if (correctlyCharged + falselyCharged < carsToCharge)
			{
				PMWrapper.RaiseTaskError("För få tåg laddades. Alla tåg med batterinivå < " + chargeBound + " ska laddas.");
			}
			else
			{
				PMWrapper.RaiseTaskError("För många tåg laddades. Bara tåg med batterinivå < " + chargeBound + " ska laddas.");
			}
		}
    }
}
