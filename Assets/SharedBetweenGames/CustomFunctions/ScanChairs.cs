using System;
using Mellis;
using Mellis.Core.Interfaces;
using UnityEngine;

public class ScanChairs : ClrYieldingFunction
{
    public ScanChairs() : base("scanna_antal_stolar")
    {
    }

    public override void InvokeEnter(params IScriptType[] arguments)
    {
        GameObject car = CarQueue.GetFirstCar();

        Scanner scanner = Scanner.instance;
        scanner.Scan(car);

        int chairCount = 0;

        foreach (Transform child in car.transform)
        {
            if (child.CompareTag("Chair"))
			{
				chairCount++;
			}
		}

        scanner.SetDisplayText(chairCount);
    }
}
