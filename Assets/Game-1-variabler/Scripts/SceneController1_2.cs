using System;
using GameData;
using PM;
using UnityEngine;

public class SceneController1_2 : MonoBehaviour, IPMCaseSwitched, IPMCompilerStopped
{
	static SceneController1_2()
	{
		Main.RegisterFunction(new UnloadPalm());
		Main.RegisterFunction(new UnloadLamp());
	}

    public int itemsUnloaded = 0;

	private GoodsCaseDefinition caseDef;

	public void OnPMCompilerStopped(StopStatus status)
	{
		if (status == StopStatus.Finished)
		{			
			int itemsToUnload = 0;

			foreach (var section in caseDef.cars[0].sections)
			{
				itemsToUnload += section.itemCount;
			}

			if (itemsUnloaded < itemsToUnload)
			{
				int itemsNotUnloaded = itemsToUnload - itemsUnloaded;
				string itemsSingularOrPlural = itemsNotUnloaded == 1 ? "1 vara" : itemsNotUnloaded + " varor";
				PMWrapper.RaiseTaskError("Alla varor blev inte avlastade. Nu är det " + itemsSingularOrPlural + " som inte lastas av.");
			}

			if (itemsToUnload == itemsUnloaded)
			{
				PMWrapper.SetCaseCompleted();
			}
		}
		itemsUnloaded = 0;
	}

	public void OnPMCaseSwitched(int caseNumber)
	{
		caseDef = (GoodsCaseDefinition)PMWrapper.currentLevel.cases[caseNumber].caseDefinition;
	}
}
