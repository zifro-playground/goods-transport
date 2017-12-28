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

public class Sorting
{
	public string leftType { get; set; }
	public string rightType { get; set; }
	public string forwardType { get; set; }
}

public class Case
{
	public List<Car> cars { get; set; }
	public int answer { get; set; }
	public string precode { get; set; }
	public Sorting correctSorting { get; set; }
	public int chargeTrigger { get; set; }
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