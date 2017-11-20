using System.Collections.Generic;

public class Item
{
	public string type { get; set; }
	public int count { get; set; }
}

public class Car
{
	public List<Item> items { get; set; }
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