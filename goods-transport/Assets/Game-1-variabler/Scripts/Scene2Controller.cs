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
		}
		itemsUnloaded = 0;
	}

	public void SetPrecode(Case caseData)
	{
		if (caseData.precode != null)
			PMWrapper.preCode = caseData.precode;
	}
}
