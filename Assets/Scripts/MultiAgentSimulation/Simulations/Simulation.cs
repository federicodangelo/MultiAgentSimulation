using System;
using System.Collections.Generic;

public class Simulation
{
	public const int TICKS_PER_SECOND = 20;
	public const int MAX_ITERATIONS_PER_UPDATE = 200;
	
	public SimulationDefinition simulationDefinition;
	
	public List<SimBox> boxes = new List<SimBox>();
	
	public ISimulationListener simulationListener = new SimulationListenerNull();
	
	private float time;
	
	public SimBox AddBox(string id, SimVector3 center, int gridSizeX, int gridSizeY)
	{
		if (id == null)
			throw new ArgumentNullException("id");
		
		if (GetBox(id) != null)
			throw new ArgumentException("Duplicated id", "id");
			
		SimBox box = new SimBox();
		
		box.Init(id, center, this, gridSizeX, gridSizeY);
		
		boxes.Add(box);
		
		simulationListener.OnBoxAdded(box);
		
		return box;
	}
	
	public void RemoveBox(SimBox box)
	{
		if (boxes.Remove(box))
			simulationListener.OnBoxRemoved(box);
	}
	
	public SimBox GetBox(string id)
	{
		for (int i = 0; i < boxes.Count; i++)
			if (boxes[i].id == id)
				return boxes[i];
		
		return null;
	}
	
	public void Update(float deltaTime)
	{
		time += deltaTime;
		
		//Rules are execute at TICKS_PER_SECOND intervals
		int maxIterations = MAX_ITERATIONS_PER_UPDATE;
		while (time >= 1.0f / TICKS_PER_SECOND && maxIterations-- > 0)
		{
			time -= 1.0f / TICKS_PER_SECOND;
		
			for (int i = 0; i < boxes.Count; i++)
				boxes[i].Update();
		}
	}
}
