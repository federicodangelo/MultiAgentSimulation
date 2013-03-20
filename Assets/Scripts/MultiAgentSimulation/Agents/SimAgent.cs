using System;

public class SimAgent
{
	public SimAgentType agentType;
	
	public int id;
	
	public float radius;
	
	public SimVector3 worldPosition;
	
	public SimResourceBinCollection resources = new SimResourceBinCollection();
	
	public SimUnit owner;
	
	public string searchTarget;
	
	private SimPoint lastPoint;
	
	private SimSegmentPosition currentPosition;
	
	private SimPoint nextPoint;
	
	public void Init(SimAgentType agentType, int id, SimPoint position, SimUnit owner, SimResourceBinCollection resources, string searchTarget)
	{
		this.agentType = agentType;
		this.id = id;
		this.owner = owner;
		this.searchTarget = searchTarget;
		this.resources.AddResources(resources);
		
		this.worldPosition = position.worldPosition;
		
		this.lastPoint = position;
	}
	
	public void Move()
	{
		if (nextPoint == null)
		{
			if (UnloadResources())
			{
				lastPoint.path.box.RemoveAgent(this);
			}
			else
			{
				FindNextPoint();
			}
		}
		else
		{
			MoveTowardsNextPoint();
		}
	}
	
	private void MoveTowardsNextPoint()
	{
		float direction;
		
		if (nextPoint == currentPosition.segment.point2)
			direction = 1.0f; //moving from point1 to point2 
		else
			direction = -1.0f; //moving from point2 to point1
	
		currentPosition.offset += direction * (agentType.speed / Simulation.TICKS_PER_SECOND) / currentPosition.segment.length;
		if (currentPosition.offset < 0.0f)
		{
			currentPosition.offset = 0.0f;
			
			//Reached point1
			lastPoint = currentPosition.segment.point1;
			nextPoint = null;
		}
		else if (currentPosition.offset > 1.0f)
		{
			currentPosition.offset = 1.0f;
			
			//Reached point2
			lastPoint = currentPosition.segment.point2;
			nextPoint = null;
		}
		
		worldPosition = currentPosition.WorldPosition;
	}
	
	private void FindNextPoint()
	{
		nextPoint = lastPoint.path.FindNextPoint(lastPoint, searchTarget, resources);
		
		if (nextPoint != null)
		{
			currentPosition.segment = lastPoint.GetSegmentToPoint(nextPoint);
			if (lastPoint == currentPosition.segment.point1)
				currentPosition.offset = 0;
			else
				currentPosition.offset = 1;
		}
	}
	
	private bool UnloadResources()
	{
		SimUnit targetUnit = lastPoint.GetUnitWithTargetAndCapacity(searchTarget, resources);
		
		if (targetUnit != null)
			resources.TransferResourcesTo(targetUnit.resources);
		
		return resources.IsEmpty();
	}
}


