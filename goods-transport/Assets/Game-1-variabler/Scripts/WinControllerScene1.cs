using UnityEngine;

public class WinControllerScene1 : MonoBehaviour, IWinController
{
	public void SetLevelAnswer(Case caseData)
	{
		int itemsInCar = 0;

		foreach (Section section in caseData.cars[0].sections)
		{
			itemsInCar += section.count;
		}

		PMWrapper.SetCurrentLevelAnswere(Compiler.VariableTypes.number, new string[] { itemsInCar.ToString() });
	}
}
