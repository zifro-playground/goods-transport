using System.Collections.Generic;

public class Car
{
	public int granar { get; set; }
	public int palmer { get; set; }
	public int stolar { get; set; }
}

public class Case
{
	public List<Car> cars { get; set; }
}

public class Levels
{
	public Case @case { get; set; }
}

public class Game
{
	public Levels levels { get; set; }
}

public class RootObject
{
	public Game game { get; set; }
}