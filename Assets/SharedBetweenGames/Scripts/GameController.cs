using UnityEngine;
using GameData;
using PM;

public class GameController : MonoBehaviour
{
    static GameController()
    {
        Main.RegisterCaseDefinitionContract<GoodsCaseDefinition>();
    }
}
