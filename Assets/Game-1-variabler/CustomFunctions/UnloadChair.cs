using Mellis;
using Mellis.Core.Interfaces;
using UnityEngine;

public class UnloadChair : ClrYieldingFunction
{
    public UnloadChair() : base("lasta_av_stol")
    {
    }

    public override void InvokeEnter(params IScriptType[] arguments)
    {
        var chair = GameObject.FindGameObjectWithTag("Chair");

        if (chair == null)
		{
			PMWrapper.RaiseError("Hittade ingen stol att lasta av.");
		}

		chair.GetComponent<UnloadableItem>().IsUnloading = true;
        GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController1_2>().itemsUnloaded += 1;
    }
}