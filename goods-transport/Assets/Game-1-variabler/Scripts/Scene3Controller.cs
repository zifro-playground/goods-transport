using UnityEngine;
using PM;

public class Scene3Controller : MonoBehaviour, ISceneController, IPMCompilerStopped
{
	[HideInInspector]
	public int carsUnloaded = 0;

	private LevelController levelController;

	private void Start()
	{
		levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
	}


	public void OnPMCompilerStopped(HelloCompiler.StopStatus status)
	{
		if (status == HelloCompiler.StopStatus.Finished)
		{
			Case caseData = levelController.caseData;

			bool levelShouldBeAnswered = false;

			// Should be moved to PMWrapper
			foreach(Compiler.Function fun in UISingleton.instance.compiler.addedFunctions)
			{
				if (fun.GetType() == new AnswerFunction().GetType())
					levelShouldBeAnswered = true;
			}

			int carsToUnload = caseData.cars.Count;
			Debug.Log("target: " + carsToUnload + " actual: " + carsUnloaded);
			if (carsToUnload == carsUnloaded && !levelShouldBeAnswered)
				PMWrapper.SetCaseCompleted();
		}
		carsUnloaded = 0;
	}

	public void SetPrecode(Case caseData)
	{
		string precode = "antal_bilar = " + caseData.cars.Count;
		PMWrapper.preCode = precode;
	}
}
