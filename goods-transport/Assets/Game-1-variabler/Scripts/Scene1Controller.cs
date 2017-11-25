using UnityEngine;

public class Scene1Controller : MonoBehaviour, ISceneController
{
	public void SetLevelAnswer(Case caseData)
	{
		int itemsInCar = 0;

		foreach (Section section in caseData.cars[0].sections)
		{
			itemsInCar += section.itemCount;
		}
		PMWrapper.SetCaseAnswer(itemsInCar);
	}

	public void SetPrecode(Case caseData)
	{
		string precode = "";

		foreach (Section section in caseData.cars[0].sections)
		{
			if (section.itemCount > 0)
				precode += section.type + " = " + section.itemCount + "\n";
		}
		PMWrapper.preCode = precode.Trim();
	}
}
