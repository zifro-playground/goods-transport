using UnityEngine;
using PM;

public class Scene3Controller : MonoBehaviour, ISceneController, IPMCompilerStopped
{
	[HideInInspector]
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
		int itemsInCars = 0;
		foreach (Car car in caseData.cars)
		{
			foreach (Section section in car.sections)
			{
				itemsInCars += section.itemCount;
			}
		}
		PMWrapper.SetCaseAnswer(itemsInCars);
	}

	public void SetPrecode(Case caseData)
	{
		string precode = "antal_bilar = " + caseData.cars.Count;
		PMWrapper.preCode = precode;
	}
}
