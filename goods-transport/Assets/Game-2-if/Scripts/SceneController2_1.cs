using PM;
using UnityEngine;

public class SceneController2_1 : MonoBehaviour, ISceneController, IPMCompilerStopped, IPMCompilerStarted
{
	public int CarsCharged;
	private int carsToCharge;

	public void SetPrecode(Case caseData)
	{
		if (caseData.precode != null)
			PMWrapper.preCode = caseData.precode;
	}



	public void OnPMCompilerStopped(HelloCompiler.StopStatus status)
	{
		if (status == HelloCompiler.StopStatus.Finished)
		{
			if (CarsCharged < carsToCharge)
			{
				int carsNotCharged = carsToCharge - CarsCharged;
				string carsSingularOrPlural = carsNotCharged == 1 ? "1 bil" : carsNotCharged + " bilar";
				PMWrapper.RaiseTaskError(carsSingularOrPlural + " blev inte laddade. Alla bilar med batterinivå < " + LevelController.CaseData.chargeBound + " ska laddas.");
			}
			if (CarsCharged > carsToCharge)
				PMWrapper.RaiseTaskError("För många bilar laddades. Bara bilar med batterinivå < " + LevelController.CaseData.chargeBound + " ska laddas.");
			if (CarsCharged == carsToCharge)
				PMWrapper.SetCaseCompleted();
		}

		CarsCharged = 0;
	}

	public void OnPMCompilerStarted()
	{
		carsToCharge = 0;

		foreach (Car car in LevelController.CaseData.cars)
		{
			if (car.batteryLevel < LevelController.CaseData.chargeBound)
				carsToCharge++;
		}
	}
}
