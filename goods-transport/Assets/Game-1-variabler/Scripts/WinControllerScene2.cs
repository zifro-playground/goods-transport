using PM;
using UnityEngine;

public class WinControllerScene2 : MonoBehaviour, IWinController, IPMCompilerStopped
{
	public int itemsUnloaded = 0;

	LevelController levelController;
	Case caseData;

	private void Start()
	{
		levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
		caseData = levelController.caseData;
	}

	public void OnPMCompilerStopped(HelloCompiler.StopStatus status)
	{
		if (status == HelloCompiler.StopStatus.Finished)
		{
			int itemsToUnload = 0;

			foreach (Section section in caseData.cars[0].sections)
			{
				itemsToUnload += section.count;
			}

			if (itemsToUnload == itemsUnloaded)
				PMWrapper.SetCaseCompleted();
		}
	}

	public void SetLevelAnswer(Case caseData)
	{
		// This level does not use the AnswerFunction
	}
}
