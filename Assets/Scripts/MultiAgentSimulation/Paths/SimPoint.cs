using System;
using System.Collections.Generic;

public class SimPoint
{
	public int id;
	
	public SimVector3 worldPosition;
	
	public SimPath path;
	
	public List<SimSegment> segments = new List<SimSegment>(2);
	
	public List<SimUnit> units = new List<SimUnit>();
	
	public void Init(SimPath path, int id, SimVector3 worldPosition)
	{
		this.path = path;
		this.id = id;
		this.worldPosition = worldPosition;
	}
	
	public void Destroy()
	{
		while(units.Count > 0)
			path.box.RemoveUnit(units[units.Count - 1]);
		
		while(segments.Count > 0)
			path.RemoveSegment(segments[segments.Count - 1]);
	}
	
	public void GetMapPosition(out int x, out int y)
	{
		SimVector3 worldPos = worldPosition;
		
		x = ((int) worldPos.x) / SimMap.GRID_SIZE;
		y = ((int) worldPos.z) / SimMap.GRID_SIZE;
		
		if (x < 0)
			x = 0;
		else if (x >= path.box.gridSizeX)
			x = path.box.gridSizeX - 1;
		
		if (y < 0)
			y = 0;
		else if (y >= path.box.gridSizeY)
			y = path.box.gridSizeY  -1;
	}
	
	public SimSegment GetSegmentToPoint(SimPoint point)
	{
		for (int i = 0; i < segments.Count; i++)
			if (segments[i].point1 == point || segments[i].point2 == point)
				return segments[i];
		
		return null;
	}
	
	public SimUnit GetUnitWithTargetAndCapacity(string searchTarget, SimResourceBinCollection resources)
	{
		for (int i = 0; i < units.Count; i++)
			if (units[i].Accepts(searchTarget, resources))
				return units[i];
		
		return null;
	}
}


