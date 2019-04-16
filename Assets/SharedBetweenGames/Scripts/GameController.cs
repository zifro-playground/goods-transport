using GameData;
using PM;
using UnityEngine;

public class GameController : MonoBehaviour
{
	static GameController()
	{
		Main.RegisterCaseDefinitionContract<GoodsCaseDefinition>();
	}
}
