using Mellis;
using Mellis.Core.Interfaces;
using UnityEngine;

public class UnloadLamp : ClrYieldingFunction
{
    public UnloadLamp() : base("lasta_av_lampa")
    {
    }

    public override void InvokeEnter(params IScriptType[] arguments)
    {
        GameObject lamp = GameObject.FindGameObjectWithTag("Lamp");

        if (lamp == null)
		{
			PMWrapper.RaiseError("Hittade ingen lampa att lasta av.");
		}

		lamp.GetComponent<UnloadableItem>().IsUnloading = true;
        GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController1_2>().itemsUnloaded += 1;
    }
}