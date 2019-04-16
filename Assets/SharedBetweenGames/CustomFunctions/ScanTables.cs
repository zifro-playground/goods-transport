using System;
using Mellis;
using Mellis.Core.Interfaces;
using UnityEngine;

public class ScanTables : ClrYieldingFunction
{
    public ScanTables() : base("scanna_antal_bord")
    {
    }

    public override void InvokeEnter(params IScriptType[] arguments)
    {
        GameObject car = CarQueue.GetFirstCar();

        Scanner scanner = Scanner.instance;
        scanner.Scan(car);

        int tableCount = 0;

        foreach (Transform child in car.transform)
        {
            if (child.CompareTag("Table"))
			{
				tableCount++;
			}
		}

        scanner.SetDisplayText(tableCount);
    }
}
