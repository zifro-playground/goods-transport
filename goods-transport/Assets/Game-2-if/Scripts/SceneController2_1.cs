using System;
using PM;
using UnityEngine;

public class SceneController2_1 : MonoBehaviour, ISceneController, IPMCompilerStopped
{
	public int carsCharged = 0;

	public void SetPrecode(Case caseData)
	{
		if (caseData.precode != null)
			PMWrapper.preCode = caseData.precode;
	}

	public void OnPMCompilerStopped(HelloCompiler.StopStatus status)
	{
		if (status == HelloCompiler.StopStatus.Finished)
		{
			int carsToCharge = 0;

			foreach (Car car in LevelController.caseData.cars)
			{
				if (car.batteryLevel < LevelController.caseData.chargeTrigger)
					carsToCharge++;
			}
			
			if (carsCharged < carsToCharge)
			{
				int carsNotCharged = carsToCharge - carsCharged;
				string carsSingularOrPlural = carsNotCharged == 1 ? "1 bil" : carsNotCharged + " bilar";
				PMWrapper.RaiseTaskError(carsSingularOrPlural + " blev inte laddade. Alla bilar med batterinivå < " + LevelController.caseData.chargeTrigger + " ska laddas.");
			}
			if (carsCharged > carsToCharge)
				PMWrapper.RaiseTaskError("För många bilar laddades. Bara bilar med batterinivå < " + LevelController.caseData.chargeTrigger + " ska laddas.");
			if (carsCharged == carsToCharge)
				PMWrapper.SetCaseCompleted();
		}

		carsCharged = 0;
	}
}
