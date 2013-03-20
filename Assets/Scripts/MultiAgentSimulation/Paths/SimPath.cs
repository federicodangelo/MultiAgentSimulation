using System;
using System.Collections.Generic;

public class SimPath
{
	public string id;
	public SimPathType pathType;
	
	public SimBox box;
	
	public List<SimPoint> points = new List<SimPoint>();
	public List<SimSegment> segments = new List<SimSegment>();
	
	public ISimPathListener pathListener = new SimPathListenerNull();
	
	private int nextPointId;
	private int nextSegmentId;
	
	public void Init(SimPathType pathType, SimBox box)
	{
		this.pathType = pathType;
		this.box = box;
		this.id = pathType.id;
	}
	
	public SimPoint AddPoint(SimVector3 worldPosition)
	{
		SimPoint point = new SimPoint();
		
		point.Init(this, nextPointId++, worldPosition);
		
		points.Add(point);
		
		pathListener.OnPointAdded(this, point);
		
		return point;
	}
	
	public void RemovePoint(SimPoint point)
	{
		point.Destroy();
		
		points.Remove(point);
		
		pathListener.OnPointRemoved(this, point);
	}
	
	public SimSegment AddSegment(SimSegmentType segmentType, SimPoint p1, SimPoint p2)
	{
		SimSegment segment = new SimSegment();
		
		segment.Init(this, segmentType, nextSegmentId++, p1, p2);
		
		segments.Add(segment);
		
		pathListener.OnSegmentAdded(this, segment);
		
		return segment;
	}
	
	public void RemoveSegment(SimSegment segment)
	{
		segment.Destroy();
		
		segments.Remove(segment);
		
		pathListener.OnSegmentRemoved(this, segment);
	}
	
	public SimPoint SplitSegment(SimSegmentPosition positionToSplit)
	{
		if (positionToSplit.offset == 0)
			return positionToSplit.segment.point1;
		else if (positionToSplit.offset == 1)
			return positionToSplit.segment.point2;
		
		SimPoint newPoint = AddPoint(positionToSplit.WorldPosition);
		
		AddSegment(positionToSplit.segment.segmentType, newPoint, positionToSplit.segment.point2);
		
		positionToSplit.segment.ChangePoint2(newPoint);
		
		pathListener.OnSegmentModified(this, positionToSplit.segment);
		
		return newPoint;		
	}
	
	static private Random rnd = new Random();
	
	static private HashSet<SimPoint> closedSet = new HashSet<SimPoint>();
	static private List<SimPoint> openSet = new List<SimPoint>();
	static private Dictionary<SimPoint, SimPoint> cameFrom = new Dictionary<SimPoint, SimPoint>();
	static private Dictionary<SimPoint, float> scoreFromStart = new Dictionary<SimPoint, float>();
	static private Dictionary<SimPoint, float> scorePlusHeuristicFromStart = new Dictionary<SimPoint, float>();
	
	public SimPoint FindNextPoint(SimPoint fromPoint, string searchTarget, SimResourceBinCollection resources)
	{
		//This implementation MUST be replaced with something that is really fast.. right now
		//we are not even using a priority queue!!
		//We should store as much information as possible in each SimPoint to make this search as
		//fast as possible.
		closedSet.Clear();
		openSet.Clear();
		cameFrom.Clear();
		scoreFromStart.Clear();
		scorePlusHeuristicFromStart.Clear();
		
		openSet.Add(fromPoint);
		scoreFromStart[fromPoint] = 0;
		scorePlusHeuristicFromStart[fromPoint] = scoreFromStart[fromPoint] + Heuristic(fromPoint, fromPoint);
		
		while(openSet.Count > 0)
		{
			SimPoint current = GetPointWithLowestScorePlusHeuristicFromStart();
			
			if (current.GetUnitWithTargetAndCapacity(searchTarget, resources) != null)
			{
				if (current == fromPoint)
					return current;
				
				while(cameFrom[current] != fromPoint)
					current = cameFrom[current];
				
				return current;
			}
			
			openSet.Remove(current);
			closedSet.Add(current);
			
			foreach(SimSegment segment in current.segments)
			{
				SimPoint neighbor;
				if (segment.point1 == current)
					neighbor = segment.point2;
				else
					neighbor = segment.point1;
				
				float neighborScoreFromStart = scoreFromStart[current] + segment.length;
				
				if (closedSet.Contains(neighbor))
					if (neighborScoreFromStart >= scoreFromStart[neighbor])
						continue;
				
				if (!openSet.Contains(neighbor) || neighborScoreFromStart < scoreFromStart[neighbor])
				{
					cameFrom[neighbor] = current;
					scoreFromStart[neighbor] = neighborScoreFromStart;
					scorePlusHeuristicFromStart[neighbor] = neighborScoreFromStart + Heuristic(neighbor, fromPoint);
					if (!openSet.Contains(neighbor))
						openSet.Add(neighbor);
				}
			}
		}
		
		//No path found.. return random point!
		if (fromPoint.segments.Count > 0)
		{
			SimSegment randomSegment = fromPoint.segments[rnd.Next(0, fromPoint.segments.Count)];
			
			if (randomSegment.point1 == fromPoint)
				return randomSegment.point2;
			else if (randomSegment.point2 == fromPoint)
				return randomSegment.point1;
		}
		
		return null;
	}
	
	private SimPoint GetPointWithLowestScorePlusHeuristicFromStart()
	{
		float lowestValue = float.MaxValue;
		SimPoint lowestPoint = null;
		
		foreach(KeyValuePair<SimPoint, float> entry in scorePlusHeuristicFromStart)
		{
			if (entry.Value < lowestValue)
			{
				lowestValue = entry.Value;
				lowestPoint = entry.Key;
			}
		}
		
		if (lowestPoint != null)
			scorePlusHeuristicFromStart.Remove(lowestPoint);
		
		return lowestPoint;
	}
	
	private float Heuristic(SimPoint p1, SimPoint p2)
	{
		return (p2.worldPosition - p1.worldPosition).Len;
	}
}


