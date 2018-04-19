using PM;
using UnityEngine;

public class SceneController2_1 : MonoBehaviour, ISceneController, IPMCompilerStopped, IPMCompilerStarted
{
    public static int correctlyCharged = 0;
    public static int falselyCharged = 0;
	public static int checkChargeCounter = 0;

    private int carsToCharge;
	private int carsToCheck;

    public void SetPrecode(Case caseData)
	{
		if (caseData.precode != null)
			PMWrapper.preCode = caseData.precode;
	}

	public void OnPMCompilerStopped(HelloCompiler.StopStatus status)
	{
		if (status == HelloCompiler.StopStatus.Finished)
		{
            CorrectCase();
		}
    }

	public void OnPMCompilerStarted()
	{
        Scanner.Instance.DisplayText.gameObject.SetActive(true);

        carsToCharge = 0;
        correctlyCharged = 0;
        falselyCharged = 0;
		checkChargeCounter = 0;
		carsToCheck = 0;
		foreach (Car car in LevelController.CaseData.cars)
		{
			carsToCheck++;
			if (car.batteryLevel < LevelController.CaseData.chargeBound)
				carsToCharge++;
		}
	}

    private void CorrectCase()
    {
        int chargeBound = LevelController.CaseData.chargeBound;

        if (correctlyCharged == carsToCharge)
        {
			if (falselyCharged > 0)
				PMWrapper.RaiseTaskError ("För många tåg laddades. Bara tåg med batterinivå < " + chargeBound + " ska laddas.");
			else if (checkChargeCounter < carsToCheck)
				PMWrapper.RaiseTaskError ("För få batterinivålev kollade. Alla tås batterinivååte kollas fö att vi ska vet om tået ska bli laddat.");
            else
                PMWrapper.SetCaseCompleted();
        }
        else
        {
            if (correctlyCharged + falselyCharged == carsToCharge)
                PMWrapper.RaiseTaskError("Fel tåg laddades.");
            else if (correctlyCharged + falselyCharged < carsToCharge)
                PMWrapper.RaiseTaskError("För få tåg laddades. Alla tåg med batterinivå < " + chargeBound + " ska laddas.");
            else
                PMWrapper.RaiseTaskError("För många tåg laddades. Bara tåg med batterinivå < " + chargeBound + " ska laddas.");
        }
    }
}

    {
        int chargeBound = LevelController.CaseData.chargeBound;

        if (correctlyCharged == carsToCharge)
        {
            if (falselyCharged > 0)
                PMWrapper.RaiseTaskError("För många tåg laddades. Bara tåg med batterinivå < " + chargeBound + " ska laddas.");
            else
                PMWrapper.SetCaseCompleted();
        }
        else
        {
            if (correctlyCharged + falselyCharged == carsToCharge)
                PMWrapper.RaiseTaskError("Fel tåg laddades.");
            else if (correctlyCharged + falselyCharged < carsToCharge)
                PMWrapper.RaiseTaskError("För få tåg laddades. Alla tåg med batterinivå < " + chargeBound + " ska laddas.");
            else
                PMWrapper.RaiseTaskError("För många tåg laddades. Bara tåg med batterinivå < " + chargeBound + " ska laddas.");
        }
    }
}
