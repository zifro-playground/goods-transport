using UnityEngine;
using PM;

public class Scene3Controller : MonoBehaviour, ISceneController, IPMCompilerStopped, IPMWrongAnswer
{
	[HideInInspector]
	public int carsUnloaded = 0;

	private LevelController levelController;
	private Case caseData;

	private void Start()
	{
		levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
	}

	public void OnPMCompilerStopped(HelloCompiler.StopStatus status)
	{
		if (status == HelloCompiler.StopStatus.Finished)
		{
			Case caseData = levelController.caseData;

			int carsToUnload = caseData.cars.Count;

			if (carsUnloaded < carsToUnload)
			{
				string carSingularOrPlural = carsToUnload - carsUnloaded == 1 ? (carsToUnload - carsUnloaded) + " bil" : (carsToUnload - carsUnloaded) + " bilar";
				PMWrapper.RaiseTaskError("Alla bilar blev inte tömda. Nu är det " + carSingularOrPlural + " som inte töms.");
			}
			if (carsToUnload == carsUnloaded && !PMWrapper.levelShouldBeAnswered)
			{
				PMWrapper.SetCaseCompleted();
			}
		}
		carsUnloaded = 0;
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
}
