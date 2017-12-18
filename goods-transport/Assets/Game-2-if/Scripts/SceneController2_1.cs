using System;
using UnityEngine;

public class SceneController2_1 : MonoBehaviour, ISceneController
{
	public void SetPrecode(Case caseData)
	{
		if (caseData.precode != null)
			PMWrapper.preCode = caseData.precode;
	}
}
