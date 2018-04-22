using PM;
using UnityEngine;

public class SceneController2_1 : MonoBehaviour, ISceneController, IPMCompilerStopped, IPMCompilerStarted
{
    public static int CorrectlyCharged = 0;
    public static int FalselyCharged = 0;
	public static int CheckChargeCounter = 0;

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
        CorrectlyCharged = 0;
        FalselyCharged = 0;
		CheckChargeCounter = 0;
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

        if (CorrectlyCharged == carsToCharge)
        {
			if (FalselyCharged > 0)
				PMWrapper.RaiseTaskError ("För många tåg laddades. Bara tåg med batterinivå < " + chargeBound + " ska laddas.");
			else if (CheckChargeCounter < carsToCheck)
				PMWrapper.RaiseTaskError ("Alla tåg kollades inte. Se till att köra kolla_batterinivå() för varje tåg.");
            else
                PMWrapper.SetCaseCompleted();
        }
        else
        {
            if (CorrectlyCharged + FalselyCharged == carsToCharge)
                PMWrapper.RaiseTaskError("Fel tåg laddades.");
            else if (CorrectlyCharged + FalselyCharged < carsToCharge)
                PMWrapper.RaiseTaskError("För få tåg laddades. Alla tåg med batterinivå < " + chargeBound + " ska laddas.");
            else
                PMWrapper.RaiseTaskError("För många tåg laddades. Bara tåg med batterinivå < " + chargeBound + " ska laddas.");
        }
    }
}
