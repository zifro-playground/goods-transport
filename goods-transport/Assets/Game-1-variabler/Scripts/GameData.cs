using System.Collections.Generic;

public class Car
{
	public int lampor { get; set; }
	public int palmer { get; set; }
	public int? stolar { get; set; }
}

public class Case
{
	public int number { get; set; }
	public List<Car> cars { get; set; }
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