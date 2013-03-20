using System;
using System.Collections.Generic;

public class SimSegment
{
	public SimSegmentType segmentType;
	
	public int id;
	
	public SimPoint point1;
	
	public SimPoint point2;
	
	public SimPath path;
	
	public float length;
	
	public void Init(SimPath path, SimSegmentType segmentType, int id, SimPoint point1, SimPoint point2)
	{
		this.path = path;
		
		this.segmentType = segmentType;
		
		this.id = id;
		
		this.point1 = point1;
		this.point2 = point2;
		
		point1.segments.Add(this);
		point2.segments.Add(this);
		
		UpdateLength();
	}
	
	public void Destroy()
	{
		point1.segments.Remove(this);
		point2.segments.Remove(this);
	}
	
	private void UpdateLength()
	{
		length = (point2.worldPosition - point1.worldPosition).Len;
	}
	
	public void ChangePoint2(SimPoint newPoint2)
	{
		point2.segments.Remove(this);
		
		this.point2 = newPoint2;
		
		point2.segments.Add(this);
		
		UpdateLength();
	}
}

