using System;
using UnityEngine;

public class SceneController2_1 : MonoBehaviour, ISceneController
{
	public void SetPrecode(Case caseData)
	{
		PMWrapper.preCode = caseData.precode;
	}
}
