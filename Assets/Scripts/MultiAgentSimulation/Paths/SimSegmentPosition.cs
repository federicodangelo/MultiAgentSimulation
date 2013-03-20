using System;

public struct SimSegmentPosition
{
	public SimSegment segment;
	public float offset;
	
	public SimVector3 WorldPosition
	{
		get 
		{
			return segment.point1.worldPosition + (segment.point2.worldPosition - segment.point1.worldPosition) * offset;
		}
	}
	
	public void GetMapPosition(out int x, out int y)
	{
		SimVector3 worldPos = WorldPosition;
		
		x = ((int) worldPos.x) / SimMap.GRID_SIZE;
		y = ((int) worldPos.z) / SimMap.GRID_SIZE;
		
		if (x < 0)
			x = 0;
		else if (x >= segment.path.box.gridSizeX)
			x = segment.path.box.gridSizeX - 1;
		
		if (y < 0)
			y = 0;
		else if (y >= segment.path.box.gridSizeY)
			y = segment.path.box.gridSizeY  -1;
	}
}


