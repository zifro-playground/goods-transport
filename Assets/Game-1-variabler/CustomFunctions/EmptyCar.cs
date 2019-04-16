using Mellis;
using Mellis.Core.Interfaces;
using UnityEngine;

public class EmptyCar : ClrYieldingFunction
{
    public EmptyCar() : base("töm_tåg")
    {
    }

    public override void InvokeEnter(params IScriptType[] arguments)
    {
        GameObject currentCar = CarQueue.GetFirstCar();

        if (currentCar == null)
		{
			PMWrapper.RaiseError("Hittade inget tåg att tömma. ");
		}

		UnloadableItem[] itemsToUnload = currentCar.GetComponentsInChildren<UnloadableItem>();

        if (itemsToUnload.Length == 0)
		{
			PMWrapper.RaiseError("Kan inte tömma ett tomt tåg. Kom ihåg att köra fram nästa tåg innan du tömmer igen.");
		}

		foreach (UnloadableItem item in itemsToUnload)
        {
            if (item != null)
			{
				item.isUnloading = true;
			}
		}

        GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController1_3>().carsUnloaded += 1;
    }
}
