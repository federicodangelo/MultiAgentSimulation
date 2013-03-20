using System.Collections.Generic;
using System;

public class SimMapRandomCoordinates
{
	private Random rnd = new Random();
	
	private List<int> randomValues;
	private List<int> returnedValues;
	
	private int lastSizeX;
	private int lastSizeY;
	
	public void Init(int mapSizeX, int mapSizeY)
	{
		if (randomValues == null || lastSizeX != mapSizeX || lastSizeY != mapSizeY)
		{
			if (randomValues == null)
				randomValues = new List<int>(mapSizeX * mapSizeY);
			else
				randomValues.Clear();
			
			if (returnedValues == null)
				returnedValues = new List<int>(mapSizeX * mapSizeY);
			else
				returnedValues.Clear();
			
			lastSizeX = mapSizeX;
			lastSizeY = mapSizeY;
			
			for (int x = 0; x < mapSizeX; x++)
				for (int y = 0; y < mapSizeY; y++)
					randomValues.Add((x << 16) | y);
		}
		else
		{
			for (int i = 0; i < returnedValues.Count; i++)
				randomValues.Add(returnedValues[i]);
			returnedValues.Clear();
		}
	}
	
	public bool GetNextCoordinate(out int x, out int y)
	{
		if (randomValues.Count > 0)
		{
			int index = rnd.Next(0, randomValues.Count);
			int val = randomValues[index];
			
			randomValues[index] = randomValues[randomValues.Count - 1];
			randomValues.RemoveAt(randomValues.Count - 1);
			
			returnedValues.Add(val);
			
			x = ((val >> 16) & 0xFFFF);
			y = (val & 0xFFFF);
			
			return true;
		}
		
		x = 0;
		y = 0;
		
		return false;
	}
}


