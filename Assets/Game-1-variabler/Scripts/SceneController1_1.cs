using GameData;
using UnityEngine;

public class SceneController1_1 : MonoBehaviour, IPMWrongAnswer, IPMCorrectAnswer, IPMCaseSwitched
{
	int correctAnswer;

	public void OnPMCaseSwitched(int caseNumber)
	{
		var caseDef = (GoodsCaseDefinition)PMWrapper.currentLevel.cases[caseNumber].caseDefinition;
		string precode = "";

		foreach (SectionData section in caseDef.cars[0].sections)
		{
			if (section.itemCount > 0)
			{
				precode += section.type + " = " + section.itemCount + "\n";
			}
		}

		PMWrapper.preCode = precode.Trim();
		correctAnswer = caseDef.answer;
	}

	public void OnPMCorrectAnswer(string answer)
	{
		PMWrapper.SetCaseCompleted();
	}

	public void OnPMWrongAnswer(string answer)
	{
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
}
