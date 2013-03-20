using System;

public interface ISimBoxListener
{
	void OnMapAdded(SimMap map);
	void OnMapRemoved(SimMap map);
	
	void OnPathAdded(SimPath path);
	void OnPathRemoved(SimPath path);
	
	void OnUnitAdded(SimUnit unit);
	void OnUnitRemoved(SimUnit unit);
	
	void OnAgentAdded(SimAgent agent);
	void OnAgentRemoved(SimAgent agent);
}


