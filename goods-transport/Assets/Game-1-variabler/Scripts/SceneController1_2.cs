using PM;
using UnityEngine;

public class SceneController1_2 : MonoBehaviour, ISceneController, IPMCompilerStopped
{
	public int itemsUnloaded = 0;

	public void OnPMCompilerStopped(HelloCompiler.StopStatus status)
	{
		if (status == HelloCompiler.StopStatus.Finished)
		{			
			int itemsToUnload = 0;

			foreach (Section section in LevelController.caseData.cars[0].sections)
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

	public void SetPrecode(Case caseData)
	{
		if (caseData.precode != null)
			PMWrapper.preCode = caseData.precode;
	}

}
