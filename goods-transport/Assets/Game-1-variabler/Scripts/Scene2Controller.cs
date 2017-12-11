using PM;
using UnityEngine;

public class Scene2Controller : MonoBehaviour, ISceneController, IPMCompilerStopped
{
	public int itemsUnloaded = 0;

	LevelController levelController;
	
	private void Start()
	{
		levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
	}

	public void OnPMCompilerStopped(HelloCompiler.StopStatus status)
	{
		if (status == HelloCompiler.StopStatus.Finished)
		{
			Case caseData = levelController.caseData;

			int itemsToUnload = 0;

			foreach (Section section in caseData.cars[0].sections)
			{
				itemsToUnload += section.itemCount;
			}

			if (itemsToUnload == itemsUnloaded)
				PMWrapper.SetCaseCompleted();

			if (itemsUnloaded < itemsToUnload)
			{
				string itemsSingularOrPlural = itemsToUnload - itemsUnloaded == 1 ? (itemsToUnload - itemsUnloaded) + " vara" : (itemsToUnload - itemsUnloaded) + " varor";
				PMWrapper.RaiseTaskError("Alla varor blev inte avlastade. Nu är det " + itemsSingularOrPlural + " som inte lastas av.");
			}
		}
		itemsUnloaded = 0;
	}

	public void SetPrecode(Case caseData)
	{
		if (caseData.precode != null)
			PMWrapper.preCode = caseData.precode;
	}
}
