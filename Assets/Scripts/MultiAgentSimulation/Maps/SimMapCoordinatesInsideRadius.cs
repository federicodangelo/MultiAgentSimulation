using System;
using System.Collections.Generic;

public class SimMapCoordinatesInsideRadius
{
	private const int MAX_RADIUS = 255;
	
	static private Dictionary<int, int[]> cachedCoordinates = new Dictionary<int, int[]>();
	
	private Random rnd = new Random();
	
	private int[] values;
	
	private int startingIndex;
	private int offset;
	
	private int centerX;
	private int centerY;
	
	private int minX;
	private int maxX;
	
	private int minY;
	private int maxY;
	
	public void Init(
		int radius, 
		int centerX, int centerY, 
		int minX, int maxX, 
		int minY, int maxY,
		bool random)
	{
		if (!cachedCoordinates.TryGetValue(radius, out values))
		{
			values = CreateCoordinates(radius);
			
			cachedCoordinates.Add(radius, values);
		}
		
		this.offset = 0;
		
		if (random)
			this.startingIndex = rnd.Next(0, values.Length);
		else
			this.startingIndex = 0;
		
		this.centerX = centerX;
		this.centerY = centerY;
		
		this.minX = minX;
		this.maxX = maxX;
		
		this.minY = minY;
		this.maxY = maxY;
	}
	
	static private int[] CreateCoordinates(int radius)
	{
		List<int> points = new List<int>();
		
		for (int x = -radius; x <= radius; x++)
			for (int y = -radius; y <= radius; y++)
				if (Math.Abs(x) + Math.Abs(y) <= radius)
					points.Add(((x + MAX_RADIUS) << 16) | (y + MAX_RADIUS)); 
		
		return points.ToArray();
	}
	
	public bool GetNextCoordinate(out int x, out int y)
	{
		while (offset < values.Length)
		{
			int val = values[(startingIndex + offset++) % values.Length];
			
			x = ((val >> 16) & 0xFFFF) - MAX_RADIUS;
			y = (val & 0xFFFF) - MAX_RADIUS;
			
			x += centerX;
			y += centerY;
			
			if (x >= minX && x < maxX && y >= minY && y < maxY)
				return true;
		}
		
		x = 0;
		y = 0;
		return false;
	}
}


