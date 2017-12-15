using UnityEngine;
using PM;

public class SceneController1_3 : MonoBehaviour, ISceneController, IPMCompilerStopped, IPMCompilerStarted, IPMWrongAnswer, IPMCorrectAnswer
{
	[HideInInspector]
	public int carsUnloaded = 0;

	private LevelController levelController;
	private Case caseData;

	private void Start()
	{
		levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
	}

	public void OnPMCompilerStarted()
	{
		carsUnloaded = 0;
	}

	public void OnPMCompilerStopped(HelloCompiler.StopStatus status)
	{
		if (status == HelloCompiler.StopStatus.Finished)
		{
			if (!PMWrapper.levelShouldBeAnswered)
			{
				WinIfCarsUnloaded();
			}
		}
	}

	public void SetPrecode(Case caseData)
	{
		this.caseData = caseData;
		string precode = "antal_bilar = " + caseData.cars.Count;
		PMWrapper.preCode = precode;
	}

	public void OnPMWrongAnswer(string answer)
	{
		int correctAnswer = caseData.answer;
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
		Case caseData = levelController.caseData;

		int carsToUnload = caseData.cars.Count;

		if (carsUnloaded < carsToUnload)
		{
			string carSingularOrPlural = carsToUnload - carsUnloaded == 1 ? (carsToUnload - carsUnloaded) + " bil" : (carsToUnload - carsUnloaded) + " bilar";
			PMWrapper.RaiseTaskError("Alla bilar blev inte tömda. Nu är det " + carSingularOrPlural + " som inte töms.");
		}
		if (carsToUnload == carsUnloaded)
		{
			PMWrapper.SetCaseCompleted();
		}
	}

}
