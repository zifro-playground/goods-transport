using GameData;
using PM;
using UnityEngine;

public class SceneController1_3 : MonoBehaviour, IPMCompilerStopped, IPMCompilerStarted, IPMWrongAnswer,
	IPMCorrectAnswer, IPMCaseSwitched
{
	[HideInInspector]
	public int carsUnloaded;

	GoodsCaseDefinition caseDef;

	static SceneController1_3()
	{
		Main.RegisterFunction(new EmptyCar());
		Main.RegisterFunction(new DriveForward());
		Main.RegisterFunction(new ScanChairs());
		Main.RegisterFunction(new ScanPalms());
	}

	public void OnPMCaseSwitched(int caseNumber)
	{
		caseDef = (GoodsCaseDefinition)PMWrapper.currentLevel.cases[caseNumber].caseDefinition;
	}

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

	public void OnPMCorrectAnswer(string answer)
	{
		WinIfCarsUnloaded();
	}

	public void OnPMWrongAnswer(string answer)
	{
		int correctAnswer = caseDef.answer;
		int guess = int.Parse(answer.Replace(".", ""));

		if (guess < correctAnswer)
		{
			PMWrapper.RaiseTaskError("Fel svar, rätt svar är större.");
		}
		else if (guess > correctAnswer)
		{
			PMWrapper.RaiseTaskError("Fel svar, rätt svar är mindre.");
		}
	}

	void WinIfCarsUnloaded()
	{
		int carsToUnload = caseDef.cars.Count;

		if (carsUnloaded < carsToUnload)
		{
			PMWrapper.RaiseTaskError("Alla tåg tömdes inte. Nu är det " + (carsToUnload - carsUnloaded) +
			                         " som inte töms.");
		}

		if (carsToUnload == carsUnloaded)
		{
			PMWrapper.SetCaseCompleted();
		}
	}
}
