using System;

public interface ISimulationListener
{
	void OnBoxAdded(SimBox box);
	void OnBoxRemoved(SimBox box);
}


