using System;
using GameData;
using PM;
using UnityEngine;

public class SceneController1_2 : MonoBehaviour, ISceneController, IPMCompilerStopped, IPMCompilerStarted
{
	public int itemsUnloaded = 0;

	private CaseData caseData;

	public void OnPMCompilerStarted()
	{
		// Can not be loaded in OnCompilerStopped since the method also switches case and replaces LevelController.CaseData with data from case 0
		caseData = LevelController.CaseData;
	}

	public void OnPMCompilerStopped(StopStatus status)
	{
		if (status == StopStatus.Finished)
		{			
			int itemsToUnload = 0;

			foreach (SectionData section in caseData.cars[0].sections)
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
				PMWrapper.SetCaseCompleted();

		}
		itemsUnloaded = 0;
	}

	public void SetPrecode(CaseData caseData)
	{
		if (caseData.precode != null)
			PMWrapper.preCode = caseData.precode;
	}

}
