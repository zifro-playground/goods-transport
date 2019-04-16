using Mellis;
using Mellis.Core.Interfaces;
using UnityEngine;

public class UnloadPalm : ClrYieldingFunction
{
    public UnloadPalm() : base("lasta_av_palm")
    {
    }

    public override void InvokeEnter(params IScriptType[] arguments)
    {
        var palm = GameObject.FindGameObjectWithTag("Palm");

        if (palm == null)
		{
			PMWrapper.RaiseError("Hittade ingen palm att lasta av.");
		}

		palm.GetComponent<UnloadableItem>().isUnloading = true;
        GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController1_2>().itemsUnloaded += 1;
    }
}
