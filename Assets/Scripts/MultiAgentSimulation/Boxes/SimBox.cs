using System;
using System.Collections.Generic;

public class SimBox
{
	public string id;
	
	public Simulation simulation;
	
	public SimVector3 worldPosition;
	public int gridSizeX;
	public int gridSizeY;
	
	public SimResourceBinCollection globals = new SimResourceBinCollection();
	
	public ISimBoxListener boxListener = new SimBoxListenerNull();
	
	private Dictionary<string, SimMap> maps = new Dictionary<string, SimMap>();
	private Dictionary<string, SimPath> paths = new Dictionary<string, SimPath>();
	
	private SimList<SimUnit> units = new SimList<SimUnit>();
	private SimList<SimAgent> agents = new SimList<SimAgent>();
	
	private int nextUnitId;
	private int nextAgentId;
	
	public void Init(string id, SimVector3 worldPosition, Simulation simulation, int gridSizeX, int gridSizeY)
	{
		this.id = id;
		this.worldPosition = worldPosition;
		this.simulation = simulation;
		this.gridSizeX = gridSizeX;
		this.gridSizeY = gridSizeY;
		
		foreach(SimMapType mapType in simulation.simulationDefinition.mapTypes.Values)
			AddMap(mapType);
		
		foreach(SimPathType pathType in simulation.simulationDefinition.pathTypes.Values)
			AddPath(pathType);
	}
	
	public SimMap AddMap(SimMapType mapType)
	{
		if (mapType == null)
			throw new ArgumentNullException("mapType");
		
		if (GetMap(mapType.id) != null)
			throw new ArgumentException("Duplicated mapType", "mapType");
		
		SimMap map = new SimMap();
		
		map.Init(mapType, this, gridSizeX, gridSizeY);
		
		maps.Add(map.id, map);
		
		boxListener.OnMapAdded(map);
		
		return map;
	}
	
	public void RemoveMap(SimMap map)
	{
		maps.Remove(map.id);
		
		boxListener.OnMapRemoved(map);
	}
	
	public SimMap GetMap(string id)
	{
		SimMap val;
		if (maps.TryGetValue(id, out val))
			return val;
			
		return null;
	}
	
	public IEnumerable<SimMap> GetMaps()
	{
		return maps.Values;
	}
	
	public SimPath AddPath(SimPathType pathType)
	{
		if (pathType == null)
			throw new ArgumentNullException("pathType");
		
		if (GetPath(pathType.id) != null)
			throw new ArgumentException("Duplicated pathType", "pathType");
		
		SimPath path = new SimPath();
		
		path.Init(pathType, this);
		
		paths.Add(path.id, path);
		
		boxListener.OnPathAdded(path);
		
		return path;		
	}
	
	public void RemovePath(SimPath path)
	{
		paths.Remove(path.id);
		
		boxListener.OnPathRemoved(path);
	}
	
	public SimPath GetPath(string id)
	{
		SimPath val;
		if (paths.TryGetValue(id, out val))
			return val;
		
		return null;
	}
	
	public IEnumerable<SimPath> GetPaths()
	{
		return paths.Values;
	}
	
	public SimUnit AddUnit(SimUnitType unitType, SimSegmentPosition position)
	{
		SimPoint newPoint = position.segment.path.SplitSegment(position);
		
		return AddUnit(unitType, newPoint);		
	}
	
	public SimUnit AddUnit(SimUnitType unitType, SimPoint position)
	{
		SimUnit unit = new SimUnit();
		
		unit.Init(unitType, nextUnitId++, position);
		
		units.Add(unit);
		
		boxListener.OnUnitAdded(unit);
		
		return unit;
	}

	public void RemoveUnit(SimUnit unit)
	{
		unit.Destroy();
		
		units.Remove(unit);
		
		boxListener.OnUnitRemoved(unit);
	}
	
	public IEnumerable<SimUnit> GetUnits()
	{
		return units.items;
	}
	
	public SimAgent AddAgent(SimAgentType agentType, SimPoint position, SimUnit owner, SimResourceBinCollection resources, string searchTarget)
	{
		SimAgent agent = new SimAgent();
		
		agent.Init(agentType, nextAgentId++, position, owner, resources, searchTarget);
		
		agents.Add(agent);
		
		boxListener.OnAgentAdded(agent);
		
		return agent;
	}
	
	public void RemoveAgent(SimAgent agent)
	{
		agents.Remove(agent);
		
		boxListener.OnAgentRemoved(agent);
	}
	
	public IEnumerable<SimAgent> GetAgents()
	{
		return agents.items;
	}
	
	public void Update()
	{
		//Agents move all the time
		agents.StartIterating();
		for (int i = 0; i < agents.items.Count; i++)
			agents.items[i].Move();
		agents.StopIterating();
		
		units.StartIterating();
		for (int i = 0; i < units.items.Count; i++)
			units.items[i].ExecuteRules();
		units.StopIterating();
			
		foreach(SimMap map in maps.Values)
			map.ExecuteRules();
	}
}
