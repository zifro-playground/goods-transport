using System;
using Mellis;
using Mellis.Core.Interfaces;
using UnityEngine;

public class ScanLamps : ClrYieldingFunction
{
    public ScanLamps() : base("scanna_antal_lampor")
    {
    }

    public override void InvokeEnter(params IScriptType[] arguments)
    {
        GameObject car = CarQueue.GetFirstCar();

        if (car == null)
		{
			PMWrapper.RaiseError("Kan inte hitta något att scanna.");
		}

		Scanner scanner = Scanner.instance;
        scanner.Scan(car);

        int lampCount = 0;

        foreach (Transform child in car.transform)
        {
            if (child.CompareTag("Lamp"))
			{
				lampCount++;
			}
		}

        scanner.SetDisplayText(lampCount);
    }
}
