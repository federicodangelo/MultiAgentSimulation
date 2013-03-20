using System.Collections.Generic;

public class SimulationDefinition
{
	public Dictionary<string, SimResource> resourceTypes = new Dictionary<string, SimResource>();
	
	public Dictionary<string, SimMapType> mapTypes = new Dictionary<string, SimMapType>();
	
	public Dictionary<string, SimPathType> pathTypes = new Dictionary<string, SimPathType>();
	
	public Dictionary<string, SimSegmentType> segmentTypes = new Dictionary<string, SimSegmentType>();
	
	public Dictionary<string, SimAgentType> agentTypes = new Dictionary<string, SimAgentType>();
	
	public Dictionary<string, SimUnitType> unitTypes = new Dictionary<string, SimUnitType>();
	
	public SimResource GetResource(string id)
	{
		SimResource val;
		
		if (resourceTypes.TryGetValue(id, out val))
			return val;
		
		return null;
	}
	
	public SimMapType GetMapType(string id)	
	{
		SimMapType val;
		
		if (mapTypes.TryGetValue(id, out val))
			return val;
		
		return null;
	}
	
	public SimPathType GetPathType(string id)
	{
		SimPathType val;
		
		if (pathTypes.TryGetValue(id, out val))
			return val;
		
		return null;
	}

	public SimSegmentType GetSegmentType(string id)
	{
		SimSegmentType val;
		
		if (segmentTypes.TryGetValue(id, out val))
			return val;
		
		return null;
	}
	
	public SimAgentType GetAgentType(string id)
	{
		SimAgentType val;
		
		if (agentTypes.TryGetValue(id, out val))
			return val;
		
		return null;
	}

	public SimUnitType GetUnitType(string id)
	{
		SimUnitType val;
		
		if (unitTypes.TryGetValue(id, out val))
			return val;
		
		return null;
	}
}


