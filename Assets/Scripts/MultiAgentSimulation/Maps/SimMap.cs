using System;
using System.Collections.Generic;

public class SimMap
{
	public const int GRID_SIZE = 2;
	
	public string id;
	public SimMapType mapType;
	
	public SimBox box;
	public ISimMapListener mapListener = new SimMapListenerNull();
	
	private int[] values;
	public int sizeX;
	public int sizeY;
	
	private SimRuleContext context = new SimRuleContext();
	private int ticks;
	
	private SimMapRandomCoordinates randomCoordinates = new SimMapRandomCoordinates();
	private SimMapCoordinatesInsideRadius coordinatesInsideRadius = new SimMapCoordinatesInsideRadius();
	
	public void Init(SimMapType mapType, SimBox box, int sizeX, int sizeY)
	{
		this.id = mapType.id;
		this.mapType = mapType;
		this.box = box;
		
		values = new int[sizeX * sizeY];
		
		this.sizeX = sizeX;
		this.sizeY = sizeY;
		
		context.box = box;
		context.mapPositionRadius = 0;
	}
	
	public void SetValue(int x, int y, int val)
	{
		if (x < 0 || x >= sizeX)
			throw new ArgumentOutOfRangeException("x", x, "Invalid X Position");
		
		if (y < 0 || y >= sizeY)
			throw new ArgumentOutOfRangeException("y", y, "Invalid Y Position");
		
		if (val < 0)
			val = 0;
		else if (val > mapType.capacity)
			val = mapType.capacity;
		
		if (val != values[y * sizeX + x])
		{
			values[y * sizeX + x] = val;
			
			mapListener.OnMapModified(this, x, y, val);
		}
	}
	
	public int Get(int x, int y)
	{
		if (x < 0 || x >= sizeX)
			throw new ArgumentOutOfRangeException("x", x, "Invalid X Position");
		
		if (y < 0 || y >= sizeY)
			throw new ArgumentOutOfRangeException("y", y, "Invalid Y Position");
		
		return values[y * sizeX + x];
	}
	
	public int Get(int x, int y, int radius)
	{
		if (x < 0 || x >= sizeX)
			throw new ArgumentOutOfRangeException("x", x, "Invalid X Position");
		
		if (y < 0 || y >= sizeY)
			throw new ArgumentOutOfRangeException("y", y, "Invalid Y Position");
		
		if (radius < 0)
			throw new ArgumentOutOfRangeException("radius", radius, "radius can't be a negative value");
		
		coordinatesInsideRadius.Init(radius, x, y, 0, sizeX, 0, sizeY, false);
		
		int totalInsideRadius = 0;
		
		while(coordinatesInsideRadius.GetNextCoordinate(out x, out y))
			totalInsideRadius += values[y * sizeX + x];
		
		return totalInsideRadius;
	}
	
	public int Capacity(int x, int y)
	{
		if (x < 0 || x >= sizeX)
			throw new ArgumentOutOfRangeException("x", x, "Invalid X Position");
		
		if (y < 0 || y >= sizeY)
			throw new ArgumentOutOfRangeException("y", y, "Invalid Y Position");
		
		return mapType.capacity;
	}
	
	public int Capacity(int x, int y, int radius)
	{
		if (x < 0 || x >= sizeX)
			throw new ArgumentOutOfRangeException("x", x, "Invalid X Position");
		
		if (y < 0 || y >= sizeY)
			throw new ArgumentOutOfRangeException("y", y, "Invalid Y Position");
		
		if (radius < 0)
			throw new ArgumentOutOfRangeException("radius", radius, "radius can't be a negative value");
		
		coordinatesInsideRadius.Init(radius, x, y, 0, sizeX, 0, sizeY, false);
		
		int capacityInsideRadius = 0;
		
		while(coordinatesInsideRadius.GetNextCoordinate(out x, out y))
			capacityInsideRadius += mapType.capacity;
		
		return capacityInsideRadius;
	}
	
	public void Add(int x, int y, int toAdd)
	{
		if (x < 0 || x >= sizeX)
			throw new ArgumentOutOfRangeException("x", x, "Invalid X Position");
		
		if (y < 0 || y >= sizeY)
			throw new ArgumentOutOfRangeException("y", y, "Invalid Y Position");
		
		if (toAdd < 0)
			throw new ArgumentOutOfRangeException("toAdd", toAdd, "toAdd can't be a negative value");
		
		int val = values[y * sizeX + x];
		
		toAdd = Math.Min(mapType.capacity - val, toAdd); 
		
		if (toAdd > 0)
		{
			val += toAdd;
				
			values[y * sizeX + x] = val;
			
			mapListener.OnMapModified(this, x, y, val);
		}
	}	
	
	public void Add(int x, int y, int radius, int toAdd)
	{
		if (x < 0 || x >= sizeX)
			throw new ArgumentOutOfRangeException("x", x, "Invalid X Position");
		
		if (y < 0 || y >= sizeY)
			throw new ArgumentOutOfRangeException("y", y, "Invalid Y Position");
		
		if (radius < 0)
			throw new ArgumentOutOfRangeException("radius", radius, "radius can't be a negative value");
		
		if (toAdd < 0)
			throw new ArgumentOutOfRangeException("toAdd", toAdd, "toAdd can't be a negative value");
		
		coordinatesInsideRadius.Init(radius, x, y, 0, sizeX, 0, sizeY, true);
		
		int remainingToAdd = toAdd;
		
		while(remainingToAdd > 0 && coordinatesInsideRadius.GetNextCoordinate(out x, out y))
		{
			int val = values[y * sizeX + x];
			
			toAdd = Math.Min(mapType.capacity - val, remainingToAdd); 
			
			if (toAdd > 0)
			{
				val += toAdd;
			
				remainingToAdd -= toAdd;
			
				values[y * sizeX + x] = val;
				
				mapListener.OnMapModified(this, x, y, val);
			}
		}
	}
	
	public void Remove(int x, int y, int toRemove)
	{
		if (x < 0 || x >= sizeX)
			throw new ArgumentOutOfRangeException("x", x, "Invalid X Position");
		
		if (y < 0 || y >= sizeY)
			throw new ArgumentOutOfRangeException("y", y, "Invalid Y Position");
		
		if (toRemove < 0)
			throw new ArgumentOutOfRangeException("toRemove", toRemove, "toRemove can't be a negative value");
		
		int val = values[y * sizeX + x];
		
		toRemove = Math.Min(val, toRemove); 
		
		if (toRemove > 0)
		{
			val -= toRemove;
			
			values[y * sizeX + x] = val;
			
			mapListener.OnMapModified(this, x, y, val);
		}
	}	

	public void Remove(int x, int y, int radius, int toRemove)
	{
		if (x < 0 || x >= sizeX)
			throw new ArgumentOutOfRangeException("x", x, "Invalid X Position");
		
		if (y < 0 || y >= sizeY)
			throw new ArgumentOutOfRangeException("y", y, "Invalid Y Position");
		
		if (radius < 0)
			throw new ArgumentOutOfRangeException("radius", radius, "radius can't be a negative value");
		
		if (toRemove < 0)
			throw new ArgumentOutOfRangeException("toRemove", toRemove, "toRemove can't be a negative value");
		
		coordinatesInsideRadius.Init(radius, x, y, 0, sizeX, 0, sizeY, true);
		
		int remainingToRemove = toRemove;
		
		while(remainingToRemove > 0 && coordinatesInsideRadius.GetNextCoordinate(out x, out y))
		{
			int val = values[y * sizeX + x];
			
			toRemove = Math.Min(val, remainingToRemove); 
			
			if (toRemove > 0)
			{
				val -= toRemove;
				
				remainingToRemove -= toRemove;
				
				values[y * sizeX + x] = val;
				
				mapListener.OnMapModified(this, x, y, val);
			}
		}
	}

	public SimVector3 GetWorldPosition(int x, int y)
	{
		if (x < 0 || x >= sizeX)
			throw new ArgumentOutOfRangeException("x", x, "Invalid X Position");
		
		if (y < 0 || y >= sizeY)
			throw new ArgumentOutOfRangeException("y", y, "Invalid Y Position");
		
		return new SimVector3(x * GRID_SIZE, 0, y * GRID_SIZE);		
	}
	
	public void ExecuteRules()
	{
		ticks++;
		
		SimRuleMap[] rules = mapType.rules;
		
		for (int i = 0; i < rules.Length; i++)
		{
			SimRuleMap rule = rules[i];
			
			if (ticks % rule.rate == 0)
			{
				if (rule.randomTiles)
				{
					int tilesAmount = (rule.randomTilesPercent * sizeX * sizeY) / 100;
					
					randomCoordinates.Init(sizeX, sizeY);
					
					for (int j = 0; j < tilesAmount; j++)
						if (randomCoordinates.GetNextCoordinate(out context.mapPositionX, out context.mapPositionY))
							rule.Execute(context);
				}
				else
				{
					for (int x = 0; x < sizeX; x++)
					{
						context.mapPositionX = x;
						
						for (int y = 0; y < sizeY; y++)
						{
							context.mapPositionY = y;
							rule.Execute(context);
						}
					}
				}
			}
		}
	}
}
