using UnityEngine;
using System.Collections.Generic;

public class SimulationUnity : MonoBehaviour, ISimulationListener
{
	public Simulation simulation;
	
	private List<SimBoxUnity> boxesUnity = new List<SimBoxUnity>();
	
	public void Init(Simulation simulation)
	{
		this.simulation = simulation;
		
		foreach(SimBox box in simulation.boxes)
			OnBoxAdded(box);
		
		simulation.simulationListener = this;
	}
	
	public void OnBoxAdded (SimBox box)
	{
		GameObject goBox = new GameObject();
		goBox.transform.parent = transform;
		SimBoxUnity boxUnity = goBox.AddComponent<SimBoxUnity>();
		
		boxUnity.Init(box);
		
		boxesUnity.Add(boxUnity);
	}

	public void OnBoxRemoved (SimBox box)
	{
		for(int i = 0; i < boxesUnity.Count; i++)
		{
			if (boxesUnity[i].box == box)
			{
				GameObject.Destroy(boxesUnity[i].gameObject);
				boxesUnity.RemoveAt(i);
				break;
			}
		}
	}
	
	public void Update()
	{
		if (simulation != null)
			simulation.Update(Time.deltaTime);
		
		OnUpdate();
	}
	
	protected virtual void OnUpdate()
	{
		
	}
}

