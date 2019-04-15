using GameData;
using UnityEngine;
using PM;

public class SceneController1_3 : MonoBehaviour, ISceneController, IPMCompilerStopped, IPMCompilerStarted, IPMWrongAnswer, IPMCorrectAnswer
{
	[HideInInspector]
	public int carsUnloaded = 0;

	public void OnPMCompilerStarted()
	{
		carsUnloaded = 0;
	}

	public void OnPMCompilerStopped(StopStatus status)
	{
		if (status == StopStatus.Finished)
		{
			if (!PMWrapper.levelShouldBeAnswered)
			{
				WinIfCarsUnloaded();
			}
		}
	}

	public void SetPrecode(CaseData caseData)
	{
		string precode = "antal_tåg = " + caseData.cars.Count;
		PMWrapper.preCode = precode;
	}

	public void OnPMWrongAnswer(string answer)
	{
		int correctAnswer = LevelController.CaseData.answer;
		int guess = int.Parse(answer.Replace(".", ""));

		if (guess < correctAnswer)
			PMWrapper.RaiseTaskError("Fel svar, rätt svar är större.");
		else if (guess > correctAnswer)
			PMWrapper.RaiseTaskError("Fel svar, rätt svar är mindre.");
	}

	public void OnPMCorrectAnswer(string answer)
	{
		WinIfCarsUnloaded();
	}

	private void WinIfCarsUnloaded()
	{
		int carsToUnload = LevelController.CaseData.cars.Count;

		if (carsUnloaded < carsToUnload)
		{
			PMWrapper.RaiseTaskError("Alla tåg tömdes inte. Nu är det " + (carsToUnload - carsUnloaded) + " som inte töms.");
		}
		if (carsToUnload == carsUnloaded)
		{
			PMWrapper.SetCaseCompleted();
		}
	}

}
