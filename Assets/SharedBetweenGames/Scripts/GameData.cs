using System;
using System.Collections.Generic;

public class Section
{
	public string type { get; set; }
	public int itemCount { get; set; }
	public int rows { get; set; }
}

public class Car
{
	public List<Section> sections { get; set; }
	public int batteryLevel { get; set; }
	public string color { get; set; }
}

public class TypeDefinition
{
    public string name { get; set; }
    public List<string> types { get; set; }
}

public class CorrectSortedQueue
{
	public int lowerBound { get; set; }
	public int upperBound { get; set; }

	public string type { get; set; }
}

public class Sorting
{
	public List<TypeDefinition> typeDefinitions { get; set; }

	public CorrectSortedQueue leftQueue { get; set; }
	public CorrectSortedQueue rightQueue { get; set; }
	public CorrectSortedQueue forwardQueue { get; set; }
}

public class Case
{
	public List<Car> cars { get; set; }

	public string precode { get; set; }

	public Sorting correctSorting { get; set; }
	public int answer { get; set; }
	public int chargeBound { get; set; }
}

public class Level
{
	public string id { get; set; }
	public List<Case> cases { get; set; }
}

public class GameData
{
	public List<Level> levels { get; set; }
}