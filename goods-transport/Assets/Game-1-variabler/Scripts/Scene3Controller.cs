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
			bool levelShouldBeAnswered = UISingleton.instance.compiler.addedFunctions.Contains(new AnswerFunction());

			int carsToUnload = caseData.cars.Count;

			if (carsToUnload == carsUnloaded && !levelShouldBeAnswered)
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
