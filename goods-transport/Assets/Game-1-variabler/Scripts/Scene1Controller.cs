using UnityEngine;

public class Scene1Controller : MonoBehaviour, ISceneController, IPMWrongAnswer, IPMCorrectAnswer
{
	Case caseData;

	public void SetPrecode(Case caseData)
	{
		this.caseData = caseData;

		string precode = "";

		foreach (Section section in caseData.cars[0].sections)
		{
			if (section.itemCount > 0)
				precode += section.type + " = " + section.itemCount + "\n";
		}
		PMWrapper.preCode = precode.Trim();
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
		PMWrapper.SetCaseCompleted();
	}
}
