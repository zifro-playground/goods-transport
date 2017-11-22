using UnityEngine;
using PM;

public class WinControllerScene3 : MonoBehaviour, IWinController, IPMCompilerStopped
{
	public int carsUnloaded = 0;

	private bool levelShouldBeAnswered = false;

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
			int carsToUnload = 0;

			carsToUnload += caseData.cars.Count;

			if (carsToUnload == carsUnloaded)
				PMWrapper.SetCaseCompleted();
		}
	}

	public void SetLevelAnswer(Case caseData)
	{

	}
}
