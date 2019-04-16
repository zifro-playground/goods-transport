using System;
using System.Collections.Generic;
using Mellis;
using Mellis.Core.Interfaces;
using UnityEngine;

public class ScanType : ClrFunction {

	public ScanType() : base("scanna_sort")
	{
	}

	public override IScriptType Invoke(params IScriptType[] arguments)
	{
		GameObject firstCar = CarQueue.GetFirstCar();

		Scanner scanner = Scanner.Instance;
		scanner.Scan(firstCar);

		Dictionary<string, int> typesFound = new Dictionary<string, int>();
		string type = "";

		foreach (Transform t in firstCar.transform)
		{
			if (t.CompareTag("Palm"))
			{
				type = "palmer";
			}
			else if (t.CompareTag("Table"))
			{
				type = "bord";
			}
			else if (t.CompareTag("Chair"))
			{
				type = "stolar";
			}
			else if (t.CompareTag("Lamp"))
			{
				type = "lampor";
			}
			else if (t.CompareTag("Tree"))
			{
				type = "granar";
			}

			if (type.Length > 0)
			{
				typesFound[type] = 1;
			}
		}

		if (typesFound.Count > 1)
		{
			throw new Exception("There are more than one type of items in current car. Can not unambiguously decide item type.");
		}

		scanner.SetDisplayText(type);

		return Processor.Factory.Create(type);
	}
}
