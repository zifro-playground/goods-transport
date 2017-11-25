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