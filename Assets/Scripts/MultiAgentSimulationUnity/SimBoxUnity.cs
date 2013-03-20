using UnityEngine;
using System.Collections.Generic;

public class SimBoxUnity : MonoBehaviour, ISimBoxListener
{
	public SimBox box;
	
	private List<SimMapUnity> mapsUnity = new List<SimMapUnity>();
	private List<SimPathUnity> pathsUnity = new List<SimPathUnity>();
	private List<SimUnitUnity> unitsUnity = new List<SimUnitUnity>();
	private List<SimAgentUnity> agentsUnity = new List<SimAgentUnity>();
	
	private Transform mapsContainer;
	private Transform pathsContainer;
	private Transform unitsContainer;
	private Transform agentsContainer;
	
	public void Init(SimBox box)
	{
		this.box = box;
		
		box.boxListener = this;
		gameObject.name = box.id;
		transform.localPosition = new Vector3(box.worldPosition.x, box.worldPosition.y, box.worldPosition.z);
		
		mapsContainer = new GameObject("maps").transform;
		pathsContainer = new GameObject("paths").transform;
		unitsContainer = new GameObject("units").transform;
		agentsContainer = new GameObject("agents").transform;
		
		mapsContainer.parent = transform;
		pathsContainer.parent = transform;
		unitsContainer.parent = transform;
		agentsContainer.parent = transform;
		
		mapsContainer.localPosition = Vector3.zero;
		pathsContainer.localPosition = Vector3.zero;
		unitsContainer.localPosition = Vector3.zero;
		agentsContainer.localPosition = Vector3.zero;
		
		foreach(SimMap map in box.GetMaps())
			OnMapAdded(map);
		
		foreach(SimPath path in box.GetPaths())
			OnPathAdded(path);
		
		foreach(SimUnit unit in box.GetUnits())
			OnUnitAdded(unit);
		
		foreach(SimAgent agent in box.GetAgents())
			OnAgentAdded(agent);
	}
	
	public void OnMapAdded (SimMap map)
	{
		GameObject goMap = new GameObject();
		goMap.transform.parent = mapsContainer;
		SimMapUnity mapUnity = goMap.AddComponent<SimMapUnity>();
		
		mapUnity.Init(map);
		
		mapsUnity.Add(mapUnity);
	}

	public void OnMapRemoved (SimMap map)
	{
		for(int i = 0; i < mapsUnity.Count; i++)
		{
			if (mapsUnity[i].map == map)
			{
				GameObject.Destroy(mapsUnity[i].gameObject);
				mapsUnity.RemoveAt(i);
				break;
			}
		}
	}

	public void OnPathAdded (SimPath path)
	{
		GameObject goPath = new GameObject();
		goPath.transform.parent = pathsContainer;
		SimPathUnity pathUnity = goPath.AddComponent<SimPathUnity>();
		
		pathUnity.Init(path);
		
		pathsUnity.Add(pathUnity);
	}

	public void OnPathRemoved (SimPath path)
	{
		for(int i = 0; i < pathsUnity.Count; i++)
		{
			if (pathsUnity[i].path == path)
			{
				GameObject.Destroy(pathsUnity[i].gameObject);
				pathsUnity.RemoveAt(i);
				break;
			}
		}
	}

	public void OnUnitAdded (SimUnit unit)
	{
		GameObject goUnit = new GameObject();
		goUnit.transform.parent = unitsContainer;
		SimUnitUnity unitUnity = goUnit.AddComponent<SimUnitUnity>();
		
		unitUnity.Init(unit);
		
		unitsUnity.Add(unitUnity);
	}

	public void OnUnitRemoved (SimUnit unit)
	{
		for(int i = 0; i < unitsUnity.Count; i++)
		{
			if (unitsUnity[i].unit == unit)
			{
				GameObject.Destroy(unitsUnity[i].gameObject);
				unitsUnity.RemoveAt(i);
				break;
			}
		}
	}

	public void OnAgentAdded (SimAgent agent)
	{
		GameObject goAgent = new GameObject();
		goAgent.transform.parent = agentsContainer;
		SimAgentUnity agentUnity = goAgent.AddComponent<SimAgentUnity>();
		
		agentUnity.Init(agent);
		
		agentsUnity.Add(agentUnity);
	}

	public void OnAgentRemoved (SimAgent agent)
	{
		for(int i = 0; i < agentsUnity.Count; i++)
		{
			if (agentsUnity[i].agent == agent)
			{
				GameObject.Destroy(agentsUnity[i].gameObject);
				agentsUnity.RemoveAt(i);
				break;
			}
		}
	}
}

