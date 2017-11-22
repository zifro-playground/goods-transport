using UnityEngine;

public class WinControllerScene1 : MonoBehaviour, IWinController
{
	public void SetLevelAnswer(Case caseData)
	{
		int itemsInCar = 0;

		foreach (Item item in caseData.cars[0].items)
		{
			itemsInCar += item.count;
		}

		PMWrapper.SetCurrentLevelAnswere(Compiler.VariableTypes.number, new string[] { itemsInCar.ToString() });
	}
}
